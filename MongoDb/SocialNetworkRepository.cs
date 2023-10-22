using AutoMapper;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.Models;
using System.Xml.Linq;

namespace SocialNetwork
{
    internal class SocialNetworkRepository
    {
        private IMongoCollection<BsonDocument> _usersCollection;
        private IMongoCollection<BsonDocument> _postsCollection;
        private readonly IMapper _mapper;

        User _currentUser;
        public User CurrentUser => _currentUser;

        public SocialNetworkRepository(string connectionString, string databaseName, IMapper mapper)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            _usersCollection = db.GetCollection<BsonDocument>("Users");
            _postsCollection = db.GetCollection<BsonDocument>("Posts");

            _mapper = mapper;
        }

        public bool Login(string email, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("email", email) & Builders<BsonDocument>.Filter.Eq("password", password);
            var user = _usersCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                _currentUser = _mapper.Map<User>(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Post> DisplayFriendPosts(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(userId));
            var user = _usersCollection.Find(filter).FirstOrDefault();

            var friendNames = user["friends"].AsBsonArray.Select(f => f.AsString).ToList();

            var filterPosts = Builders<BsonDocument>.Filter.In("user", friendNames);
            //var friendPosts = _postsCollection.Find(filterPosts).ToList();


            var sortDefinition = Builders<BsonDocument>.Sort.Descending("date"); 

            var friendPosts = _postsCollection.Find(filterPosts)
                .Sort(sortDefinition)
                .ToList();

            return _mapper.Map<List<Post>>(friendPosts);
        }

        public User FindUser(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(userId));
            var user = _usersCollection.Find(filter).FirstOrDefault();

            return _mapper.Map<User>(user);
        }

        public string FindUserIdByName(string firstName, string lastName)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("FirstName", firstName),
                Builders<BsonDocument>.Filter.Eq("LastName", lastName)
            );
            var user = _usersCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                var userId = user["_id"].ToString();
                return userId;
            }

            return null;

        }

        public void AddFriend(string name, string surname)
        {
            string fullName = name + " " + surname;
            var userFilter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(_currentUser.Id));

            _currentUser.Friends.Add(fullName);
            var update = Builders<BsonDocument>.Update.Set("friends", _currentUser.Friends);
            _usersCollection.UpdateOne(userFilter, update);
        }

        public void DeleteFriend(string name, string surname)
        {
            string fullName = name + " " + surname;
            var userFilter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(_currentUser.Id));

            _currentUser.Friends.Remove(fullName);
            var update = Builders<BsonDocument>.Update.Set("friends", _currentUser.Friends);
            _usersCollection.UpdateOne(userFilter, update);
        }

        public List<Post> GetUserPosts(string name, string surname)
        {
            string fullName = name + " " + surname;

            var filter = Builders<BsonDocument>.Filter.Eq("user", fullName);
            var userPosts = _postsCollection.Find(filter).ToList();

            if (userPosts.Count != 0)
            {
                return _mapper.Map<List<Post>>(userPosts);
            }

            return null;
        }


        public void AddCommentToPost(string postTitle, string commentBody, string commentUser, DateTime commentDate)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("title", postTitle);
            var post = _postsCollection.Find(filter).FirstOrDefault();

            if (post != null)
            {
                var comment = new BsonDocument
                {
                    { "body", commentBody },
                    { "user", commentUser },
                    { "date", commentDate }
                };

                var commentsArray = post["comments"].AsBsonArray;
                commentsArray.Add(comment);

                var update = Builders<BsonDocument>.Update.Set("comments", commentsArray);
                _postsCollection.UpdateOne(filter, update);
            }
        }

        public void DeleteCommentToPost(string postTitle, string commentUser)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("title", postTitle);
            var post = _postsCollection.Find(filter).FirstOrDefault();

            if (post != null)
            {
                //
                var commentsArray = post["comments"].AsBsonArray;
                for (int i = 0; i < commentsArray.Count; i++)
                {
                    var comment = commentsArray[i].AsBsonDocument;
                    if (comment["user"].AsString == commentUser)
                    {
                        commentsArray.RemoveAt(i);
                        i--;
                    }
                }

                var update = Builders<BsonDocument>.Update.Set("comments", commentsArray);
                _postsCollection.UpdateOne(filter, update);
            }

        }

        public void AddLikeToPost(string postTitle, string likeUser)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("title", postTitle);
            var post = _postsCollection.Find(filter).FirstOrDefault();

            if (post != null)
            {
                var likesArray = post["like"].AsBsonArray;

                bool userLiked = false;
                foreach (var like in likesArray)
                {
                    if (like["user"].AsString == likeUser)
                    {
                        userLiked = true;
                        break;
                    }
                }

                if (!userLiked)
                {
                    var newLike = new BsonDocument { { "user", likeUser } };
                    likesArray.Add(newLike);

                    var update = Builders<BsonDocument>.Update.Set("like", likesArray);
                    _postsCollection.UpdateOne(filter, update);
                }
            }
        }

        public void DeleteLikeFromPost(string postTitle, string likeUser)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("title", postTitle);
            var post = _postsCollection.Find(filter).FirstOrDefault();

            if (post != null)
            {
                var likesArray = post["like"].AsBsonArray;

                int indexToRemove = -1;
                for (int i = 0; i < likesArray.Count; i++)
                {
                    var like = likesArray[i].AsBsonDocument;
                    if (like["user"].AsString == likeUser)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                if (indexToRemove >= 0)
                {
                    likesArray.RemoveAt(indexToRemove);

                    var update = Builders<BsonDocument>.Update.Set("like", likesArray);
                    _postsCollection.UpdateOne(filter, update);
                }
            }
        }

    }
}