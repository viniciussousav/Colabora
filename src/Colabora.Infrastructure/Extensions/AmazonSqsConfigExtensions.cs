﻿using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Extensions;

public static class AmazonSqsConfigExtensions
{
    public static void AddAmazonSqsConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var credentials = new BasicAWSCredentials(
            accessKey: configuration["Aws:Sqs:AccessKey"],
            secretKey: configuration["Aws:Sqs:SecretKey"]);

        serviceCollection.AddSingleton<IAmazonSQS>(new AmazonSQSClient(credentials, RegionEndpoint.SAEast1));
    }
}