using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SocialNetwork.Models
{
    internal class Post
    {
        [BsonId]

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("views")]
        public int Views { get; set; }

        [BsonElement("user")]
        public string User { get; set; }



        //Likes

        [BsonElement("comments")]

        public List<Comment> Comments { get; set; }

    }
}
