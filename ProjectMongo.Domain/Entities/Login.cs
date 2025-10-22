using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectMongo.Domain.Entities
{
    public class Login
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int IdUsuario { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
