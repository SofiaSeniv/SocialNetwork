using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    internal class Comment
    {
        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("user")]
        public string User { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}