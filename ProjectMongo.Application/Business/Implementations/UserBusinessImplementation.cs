using ProjectMongo.Application.Business;
using ProjectMongo.Application.Converter.Contract;
using ProjectMongo.Application.Converter.Implementations;
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;
using ProjectMongo.Domain.Repositories;
using System.Threading.Tasks;

namespace ProjectMongo.Application.Business.Implementations
{
    public class UserBusinessImplementation : IUserBusiness
    {
        private readonly IUserRepository _repository;
        private readonly IParser<UserVO, User> _voConverter; 
        private readonly IParser<User, UserVO> _entityConverter; 

        public UserBusinessImplementation(IUserRepository repository, IParser<UserVO, User> voConverter, IParser<User, UserVO> entityConverter)
        {
            _repository = repository;
            _voConverter = voConverter;
            _entityConverter = entityConverter;
        }

        public async Task<List<UserVO>> FindAllAsync()
        {
            var peopleList = await _repository.FindAllAsync();

            return _entityConverter.Parse(peopleList);
        }

        public async Task<UserVO> FindByIdAsync(string id) 
        {
            var user = await _repository.FindByIdAsync(id);

            return _entityConverter.Parse(user);
        }

        public async Task<UserVO> CreateAsync(UserVO user) 
        {
            try
            {
                var userEntity = _voConverter.Parse(user);
                userEntity = await _repository.CreateAsync(userEntity);

                return _entityConverter.Parse(userEntity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UserVO> UpdateAsync(UserVO user) 
        {
            try
            {
                var userEntity = _voConverter.Parse(user);
                userEntity = await _repository.UpdateAsync(userEntity);

                return _entityConverter.Parse(userEntity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteAsync(string id) 
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task<TokenVO> ValidateCredentials(LoginVO userCredential)
        {
            throw new NotImplementedException();
        }

        public Task<TokenVO> ValidateCredentials(TokenVO token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeToken(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
