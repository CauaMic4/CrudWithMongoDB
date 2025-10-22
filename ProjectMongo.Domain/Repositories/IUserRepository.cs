using ProjectMongo.Domain.Entities;

namespace ProjectMongo.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> FindAllAsync();
        Task<User> FindByIdAsync(string id); 
        Task<User> CreateAsync(User item);
        Task<User> UpdateAsync(User item);
        Task DeleteAsync(string id);


        Task<User> ValidateCredentials(User user);
        Task<User> GetByUsername(string userName);
        Task<User> RefreshUserInfo(User user);
        Task<bool> RevokeToken(string userName);
    }
}
