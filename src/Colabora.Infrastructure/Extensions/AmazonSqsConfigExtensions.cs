using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Extensions;

public static class AmazonSqsConfigExtensions
{
    private static void AddDynamoDb(this IServiceCollection serviceCollection)
    {
        var credentials = new BasicAWSCredentials(
            accessKey: Environment.GetEnvironmentVariable("AWS_COLABORA_ACCESS_KEY"),
            secretKey: Environment.GetEnvironmentVariable("AWS_COLABORA_SECRET_KEY"));

        serviceCollection.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(credentials, RegionEndpoint.SAEast1));
    }

    private static void AddSqs(this IServiceCollection serviceCollection)
    {
        var credentials = new BasicAWSCredentials(
            accessKey: Environment.GetEnvironmentVariable("AWS_COLABORA_ACCESS_KEY"),
            secretKey: Environment.GetEnvironmentVariable("AWS_COLABORA_SECRET_KEY"));

        serviceCollection.AddSingleton<IAmazonSQS>(new AmazonSQSClient(credentials, RegionEndpoint.SAEast1));
    }

    public static void AddAwsServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSqs();
        serviceCollection.AddDynamoDb();
    }
}