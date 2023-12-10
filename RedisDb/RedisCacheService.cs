using Newtonsoft.Json;
using StackExchange.Redis;

namespace DAL
{
    public class RedisCacheService
    {

        private readonly ConnectionMultiplexer _redisConnection;

        public RedisCacheService(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        }

        public T Get<T>(string key)
        {
            var database = _redisConnection.GetDatabase();
            var value = database.StringGet(key);

            if (!value.IsNull)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default(T);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var database = _redisConnection.GetDatabase();
            var serializedValue = JsonConvert.SerializeObject(value);

            database.StringSet(key, serializedValue, expiration);
        }
    }
}
