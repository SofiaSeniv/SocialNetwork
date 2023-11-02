using DAL.Neo4JModels;
using Neo4j.Driver;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DAL
{
    public class SocialNetworkRepositoryNeo
    { 
        private GraphClient _client;

        public SocialNetworkRepositoryNeo(GraphClient client)
        {
            _client = client;
        }

        public void CreateNewUser(UserDto newUser)
        {
            _client.Cypher
                .Create("(user:User $newUser)")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void DeleteUser(string email)
        {
            _client.Cypher
               .Match("(user:User {email: $userEmail})")
               .WithParam("userEmail", email)
               .Delete("user")
               .ExecuteWithoutResultsAsync().Wait();
        }

        public void CreateRelationship(string email1, string email2)
        {
            _client.Cypher
                .Match("(user1:User {email: $email1})", "(user2:User {email:$email2})")
                .WithParam("email1", email1)
                .WithParam("email2", email2)
                .Create("(user1)-[:FRIENS_WITH]->(user2)")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void DeleteRelationship(string email1, string email2)
        {
            _client.Cypher
                .Match("(user1:User {email: $email1})-[r:FRIENDS_WITH]->(user2:User {email:$email2})")
                .WithParam("email1", email1)
                .WithParam("email2", email2)
                .Delete("r")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public bool AreConnected(string currentUserEmail, string otherUserEmail)
        {
            var query = _client.Cypher
                .Match($"(currentUser:User {{email: '{currentUserEmail}'}})-[:FRIENDS_WITH]-(otherUser:User {{email: '{otherUserEmail}'}})")
                .Return(() => Return.As<bool>("COUNT(otherUser) > 0"));

            return query.ResultsAsync.As<bool>();
        }


        //public int CalculateDistance(string currentUserEmail, string otherUserEmail)
        //{
        //    var query = _client.Cypher
        //        .Match($"(currentUser:User {{email: '{currentUserEmail}'}})")
        //        .Match($"(otherUser:User {{email: '{otherUserEmail}'}})")
        //        .OptionalMatch("(currentUser)-[:FRIENDS_WITH*..5]-(otherUser)")  // Задайте максимальну глибину шляху
        //        .ReturnDistinct<int>("LENGTH(FRIENDS_WITH) AS distance")
        //        .OrderBy("distance ASC")
        //        .Limit(1);

        //    var result = query.Results.SingleOrDefault();
        //    return result ?? -1;  // -1, якщо не знайдено зв'язку
        //}
    }

}
