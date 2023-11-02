using DAL.Neo4JModels;
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

            return query.ResultsAsync.Result.Single();

        }


        public int CalculateDistance(string currentUserEmail, string otherUserEmail)
        {
            var query = _client.Cypher
                .Match("path = shortestPath((startNode:User {email: currentUserEmail})-[*]->(endNode:User {email: otherUserEmail}))")
                .WithParam("currentUserEmail", currentUserEmail)
                .WithParam("otherUserEmail", otherUserEmail)
                .Return(() => Return.As<int>("length(path)"));
            return query.ResultsAsync.Result.Single();
        }

    }
}
