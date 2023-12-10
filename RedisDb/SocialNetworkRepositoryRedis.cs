namespace DAL
{
    public class SocialNetworkRepositoryRedis
    {

        private readonly UserRepository _userRepository;
        private readonly RedisCacheService _redisCacheService;

        public SocialNetworkRepositoryRedis(UserRepository userRepository, RedisCacheService redisCacheService)
        {
            _userRepository = userRepository;
            _redisCacheService = redisCacheService;
        }

        public User GetUserById(string userId)
        {
            // Finding user in cache Redis
            var user = _redisCacheService.Get<User>(userId);

            if (user == null)
            {
                user = _userRepository.GetUserById(userId);

                _redisCacheService.Set(userId, user, TimeSpan.FromMinutes(10));
            }
            else
            {
                _redisCacheService.Set(userId, user, TimeSpan.FromMinutes(10));
            }

            return user;
        }
    }
}

