using Amazon.DynamoDBv2;
using DAL;
using Neo4jClient;
using SocialNetwork.UI;

namespace SocialNetwork
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Menu menu = new Menu();
            //menu.ShowMenu();


            //var client = new GraphClient(new Uri("http://localhost:7474"), "neo4j", "12345678");
            //client.ConnectAsync().Wait();

            //var socialRepository = new SocialNetworkRepositoryNeo(client);

            var repository = new SocialNetworkRepositoryDynamo();
            var consoleApp = new UIDynamo(repository);
            await consoleApp.Run();
        }
    }
}