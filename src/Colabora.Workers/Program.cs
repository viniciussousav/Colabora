using Colabora.Application.Shared;
using Colabora.Infrastructure.Extensions;
using Colabora.Workers.Configuration;
using Colabora.Workers.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAwsServices(hostContext.Configuration);
        services.AddInfrastructure(hostContext.Configuration);
        services.AddApplicationDependencies();
        services.AddHostedService<EmailVerificationRequestConsumer>();
        services.AddOptionsConfiguration(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();