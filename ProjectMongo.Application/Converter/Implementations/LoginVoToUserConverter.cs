using ProjectMongo.Application.Converter.Contract;
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;

namespace ProjectMongo.Application.Converter.Implementations
{
    public class LoginVoToUserConverter : IParser<LoginVO, User>
    {
        public User Parse(LoginVO origin)
        {
            if (origin == null) 
                return null;

            return new User
            {
                UserName = origin.UserName,
                PasswordHash = origin.Password 
            };
        }

        public List<User> Parse(List<LoginVO> origin)
        {
            throw new NotImplementedException();
        }
    }
}