using MongoDB.Driver;
using ProjectMongo.Domain.Entities;
using ProjectMongo.Domain.Repositories;
using ProjectMongo.Infrastructure.Context;
using System.Security.Cryptography;
using System.Text;

namespace ProjectMongo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
			_users = context.Users;
        }

        #region CRUD Methods

        public async Task<List<User>> FindAllAsync()
        {
            return await _users.Find(x => x.Ativo == true).ToListAsync();
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _users.Find(p => p.Id == id && p.Ativo == true).FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(User item)
        {
            if (!string.IsNullOrEmpty(item.PasswordHash))
                item.PasswordHash = ComputeHash(item.PasswordHash, SHA256.Create());

            item.Ativo = true;

            await _users.InsertOneAsync(item);

            return item;
        }

        public async Task<User> UpdateAsync(User item)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Id, item.Id);

            var result = await _users.ReplaceOneAsync(filter, item);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
                return item;

            return null;
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Id, id);

            var update = Builders<User>.Update.Set(p => p.Ativo, false);

            await _users.UpdateOneAsync(filter, update);
        }

        #endregion

        public async Task<User> ValidateCredentials(User user) 
        {
            var passHash = ComputeHash(user.PasswordHash, SHA256.Create()); 

            var filter = Builders<User>.Filter.Eq(u => u.UserName, user.UserName) &
                         Builders<User>.Filter.Eq(u => u.PasswordHash, passHash);

            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsername(string userName)
        {
            return await _users.Find(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> RefreshUserInfo(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);

            var update = Builders<User>.Update
                .Set(u => u.RefreshToken, user.RefreshToken)
                .Set(u => u.RefreshTokenExpiryTime, user.RefreshTokenExpiryTime);

            var result = await _users.UpdateOneAsync(filter, update);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return user;
            }
            return null;
        }

        public async Task<bool> RevokeToken(string userName)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, userName);
            var update = Builders<User>.Update.Set(u => u.RefreshToken, null);

            var result = await _users.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        private string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            var builder = new StringBuilder();
            foreach (var item in hashedBytes)
            {
                builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
