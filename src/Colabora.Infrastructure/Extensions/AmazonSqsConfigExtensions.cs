using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Extensions;

public static class AmazonSqsConfigExtensions
{
    private static void AddDynamoDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var credentials = new BasicAWSCredentials(
            accessKey: configuration["Aws:DynamoDb:AccessKey"],
            secretKey: configuration["Aws:DynamoDb:SecretKey"]);

        serviceCollection.AddSingleton<IAmazonDynamoDB>(_ =>
            new AmazonDynamoDBClient(credentials, RegionEndpoint.SAEast1));
    }

    private static void AddSqs(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var credentials = new BasicAWSCredentials(
            accessKey: configuration["Aws:Sqs:AccessKey"],
            secretKey: configuration["Aws:Sqs:SecretKey"]);

        serviceCollection.AddSingleton<IAmazonSQS>(new AmazonSQSClient(credentials, RegionEndpoint.SAEast1));
    }

    public static void AddAwsServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSqs(configuration);
        serviceCollection.AddDynamoDb(configuration);
    }
}