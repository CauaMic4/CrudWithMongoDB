using ProjectMongo.Application.Configurations;
using ProjectMongo.Application.Services;
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectMongo.Application.Converter.Contract; 
using ProjectMongo.Domain.Entities;
 
namespace ProjectMongo.Application.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _cofiguration;

        private IUserRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IParser<LoginVO, User> _converter; 
              
        public LoginBusinessImplementation(TokenConfiguration cofiguration, IUserRepository repository, ITokenService tokenService, IParser<LoginVO, User> converter)
        {
            _cofiguration = cofiguration;
            _repository = repository;
            _tokenService = tokenService;
            _converter = converter; 
        }

        public async Task<TokenVO> ValidateCredentials(LoginVO userCredential)
        {
            var userEntity = _converter.Parse(userCredential);

            var user = await _repository.ValidateCredentials(userEntity);

             if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GerenateAccessToken(claims);
            var refreshToken = _tokenService.GerenateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_cofiguration.DaysToExpiry);

            await _repository.RefreshUserInfo(user);

            DateTime createTime = DateTime.Now;
            DateTime expirationData = createTime.AddMinutes(_cofiguration.Minutes);

            return new TokenVO(
                true,
                createTime.ToString(DATE_FORMAT),
                expirationData.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public async Task<TokenVO> ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity.Name;

            var user = await _repository.GetByUsername(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return null;


            accessToken = _tokenService.GerenateAccessToken(principal.Claims);
            refreshToken = _tokenService.GerenateRefreshToken();

            user.RefreshToken = refreshToken;

            await _repository.RefreshUserInfo(user);

            DateTime createTime = DateTime.Now;
            DateTime expirationData = createTime.AddMinutes(_cofiguration.Minutes);

            return new TokenVO(
                true,
                createTime.ToString(DATE_FORMAT),
                expirationData.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public async Task<bool> RevokeToken(string userName)
        {
            return await _repository.RevokeToken(userName);
        }
    }

}
