using MongoDB.Driver;

namespace DAL
{
    public class UserRepository
    {

        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("users");
        }

        public User GetUserById(string userId)
        {

            return _userCollection.Find(user => user.UserId == userId).SingleOrDefault();
        }
    }
}
