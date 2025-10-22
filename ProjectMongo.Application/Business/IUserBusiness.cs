using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMongo.Application.Business
{
    public interface IUserBusiness
    {
        Task<List<UserVO>> FindAllAsync();
        Task<UserVO> FindByIdAsync(string id);
        Task<UserVO> CreateAsync(UserVO user);
        Task<UserVO> UpdateAsync(UserVO user);
        Task DeleteAsync(string id);

        Task<TokenVO> ValidateCredentials(LoginVO userCredential);
        Task<TokenVO> ValidateCredentials(TokenVO token);
        Task<bool> RevokeToken(string userName);
    }
}
