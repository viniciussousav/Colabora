using Colabora.Application.Shared;
using Colabora.Infrastructure.Persistence;
using Colabora.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.IntegrationTests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
            
            services.AddInfrastructure(configuration);
            services.AddApplicationDependencies();
        });

        // Apply migration to database
        builder.Configure(configureApp: applicationBuilder =>
        {
            var service = applicationBuilder.ApplicationServices.GetRequiredService<AppDbContext>();
            service.Database.Migrate();
        });
    }
}