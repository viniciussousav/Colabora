using Colabora.Infrastructure.Extensions;
using Colabora.Workers.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAwsServices(hostContext.Configuration);
        services.AddHostedService<EmailVerificationRequestConsumer>();
    })
    .Build();

await host.RunAsync();