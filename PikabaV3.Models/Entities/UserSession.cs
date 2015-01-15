using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PikabaV3.Models.Entities
{
    public class UserSession
    {
        public ObjectId Id { get; set; }
        public ObjectId User_Id { get; set; }
        public string CookieUuid { get; set; }
        [BsonRepresentation(BsonType.String)]
        public UserRole UserRole { get; set; }
    }
}