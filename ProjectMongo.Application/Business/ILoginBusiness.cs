using ProjectMongo.Application.VOs;

namespace ProjectMongo.Application.Business
{
    public interface ILoginBusiness
    {
        Task<TokenVO> ValidateCredentials(LoginVO userCredential);
        Task<TokenVO> ValidateCredentials(TokenVO token);
        Task<bool> RevokeToken(string userName);
    }
}
