using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectMongo.Domain.Entities
{
    public class User
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("refresh_token")]
        public string? RefreshToken { get; set; }

        [BsonElement("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [BsonElement("ativo")]
        public bool Ativo { get; set; }
    }
}
