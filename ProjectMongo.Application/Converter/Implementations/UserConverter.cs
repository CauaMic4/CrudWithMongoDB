using ProjectMongo.Application.Converter.Contract;
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;

namespace ProjectMongo.Application.Converter.Implementations
{
    public class UserConverter : IParser<UserVO, User>, IParser<User, UserVO>
    {

        public User Parse(UserVO origin)
        {
            if (origin == null)
                return null;

            return new User
            {
                Id = origin.Id,
                UserName = origin.FirstName,
                FullName = origin.LastName,
                PasswordHash = origin.Password,
                Ativo = origin.Ativo
            };
        }

        public UserVO Parse(User origin)
        {
            if (origin == null)
                return null;

            return new UserVO
            {
                Id = origin.Id,
                FirstName = origin.UserName,
                LastName = origin.FullName,
                Ativo = origin.Ativo
            };
        }

        public List<UserVO> Parse(List<User> origin)
        {
            if (origin == null)
                return null;

            return origin.Select(item => Parse(item)).Where(x => x.Ativo == true).ToList();
        }


        public List<User> Parse(List<UserVO> origin)
        {
            if (origin == null)
                return null;

            return origin.Select(item => Parse(item)).Where(x => x.Ativo == true).ToList();
        }
    }
}
