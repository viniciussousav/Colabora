using Colabora.Application.Shared;
using Colabora.Infrastructure.Extensions;
using Colabora.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOptionsConfig();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddSecuritySettings(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioningConfiguration();

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ApplyMigration();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
namespace Colabora.WebAPI
{
    public class Program
    {
    }
}