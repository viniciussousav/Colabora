using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;
using Microsoft.AspNetCore.Http;

namespace Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private const string TableName = "EmailVerification";

    public EmailVerificationRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task CreateEmailVerificationRequest(EmailVerificationRequest emailVerificationRequest)
    {
        var customerAsJson = JsonSerializer.Serialize(emailVerificationRequest);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = customerAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);

        if ((int) response.HttpStatusCode != StatusCodes.Status200OK)
            throw new AmazonDynamoDBException(message:"Error while saving email verification request");
    }
    
    public async Task<EmailVerificationRequest?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                {"pk", new () {S = id.ToString()}},
                {"sk", new () {S = id.ToString()}}
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);

        if (!response.Item.Any())
            return null;
        
        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<EmailVerificationRequest>(itemAsDocument.ToJson());
    }
}