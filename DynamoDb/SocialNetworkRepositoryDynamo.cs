using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DAL
{
    public class SocialNetworkRepositoryDynamo
    {

        AmazonDynamoDBClient _client;
        private const string TableName = "SocialMediaPosts";


        public SocialNetworkRepositoryDynamo()
        {
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            ddbConfig.ServiceURL = "http://localhost:8000";
            _client = new AmazonDynamoDBClient(ddbConfig); 
        }


        public async Task CreateSocialMediaTable()
        {
            CreateTableRequest createTableRequest = new CreateTableRequest
            {
                TableName = TableName,
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("PostId", ScalarAttributeType.N),
                new AttributeDefinition("CommentId", ScalarAttributeType.N),
                new AttributeDefinition("ModifiedDateTime", ScalarAttributeType.S) // Sort key for sorting comments
            },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("PostId", KeyType.HASH), // Partition key
                new KeySchemaElement("CommentId", KeyType.RANGE) // Sort key for comments
            },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
            {
                new GlobalSecondaryIndex
                {
                    IndexName = "PostModifiedDateTimeIndex",
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("PostId", KeyType.HASH),
                        new KeySchemaElement("ModifiedDateTime", KeyType.RANGE)
                    },
                    Projection = new Projection { ProjectionType = ProjectionType.ALL },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                }
            }
            };

            CreateTableResponse createTableResponse = await _client.CreateTableAsync(createTableRequest);
        }

        public async Task EditPost(string postId, string newContent)
        {
            UpdateItemRequest updatePostRequest = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "PostId", new AttributeValue { N = postId } }
            },
                UpdateExpression = "SET Content = :content",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":content", new AttributeValue { S = newContent } }
            }
            };

            UpdateItemResponse updatePostResponse = await _client.UpdateItemAsync(updatePostRequest);
        }

        public async Task EditComment(string postId, string commentId, string newCommentText)
        {
            UpdateItemRequest updateCommentRequest = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "PostId", new AttributeValue { N = postId } },
                { "CommentId", new AttributeValue { N = commentId } }
            },
                UpdateExpression = "SET CommentText = :commentText",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":commentText", new AttributeValue { S = newCommentText } }
            }
            };

            UpdateItemResponse updateCommentResponse = await _client.UpdateItemAsync(updateCommentRequest);
        }

        public async Task<List<Dictionary<string, AttributeValue>>> GetSortedCommentsForPost(string postId)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = TableName,
                KeyConditionExpression = "PostId = :postId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":postId", new AttributeValue { N = postId } }
                },
                IndexName = "PostModifiedDateTimeIndex",
            };

            QueryResponse queryResponse = await _client.QueryAsync(queryRequest);

            return queryResponse.Items;
        }

    }
}
