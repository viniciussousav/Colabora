﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Configuration;

[ExcludeFromCodeCoverage]
public static class ApiVersioningConfiguration
{
    public static void AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        } );
    } 
}