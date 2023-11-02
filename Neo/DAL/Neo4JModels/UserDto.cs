using Newtonsoft.Json;

namespace DAL.Neo4JModels
{
    public class UserDto
    {
        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
