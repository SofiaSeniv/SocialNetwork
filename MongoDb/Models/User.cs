using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SocialNetwork.Models
{
    internal class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }



        [BsonElement("Interests")]
        public List<string> Interests { get; set; }

        [BsonElement("friends")]

        public List<string> Friends { get; set; }
    }
}
