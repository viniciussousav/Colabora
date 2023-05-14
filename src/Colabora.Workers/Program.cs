using Colabora.Workers.Consumers;
using Colabora.Workers.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAmazonSqsConfiguration(hostContext.Configuration);
        services.AddHostedService<EmailVerificationRequestConsumer>();
    })
    .Build();

await host.RunAsync();