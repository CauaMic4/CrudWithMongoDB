using System.Text.Json.Serialization;

namespace ProjectMongo.Application.VOs
{
    public class UserVO
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public bool Ativo { get; set; }
    }
}
