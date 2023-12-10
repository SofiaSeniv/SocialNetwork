using DAL;
using MongoDB.Driver;

namespace SocialNetwork.UI
{
    internal class UIRedis
    {

        private readonly SocialNetworkRepositoryRedis _socialNetworkService;

        public UIRedis()
        {
            //var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var mongoDatabase = mongoClient.GetDatabase("SocialNetworkDB");

            var userRepository = new UserRepository(mongoDatabase);

            var redisCacheService = new RedisCacheService("localhost");
            _socialNetworkService = new SocialNetworkRepositoryRedis(userRepository, redisCacheService);
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("1. Get User by ID");
                Console.WriteLine("2. Exit");

                Console.Write("Enter option number: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        GetUserById();
                        break;
                    case "2":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }

        private void GetUserById()
        {
            Console.Write("Enter user ID: ");
            string userId = Console.ReadLine();
            var user = _socialNetworkService.GetUserById(userId);

            if (user != null)
            {
                Console.WriteLine($"User ID: {user.UserId}, Username: {user.Username}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }
}
