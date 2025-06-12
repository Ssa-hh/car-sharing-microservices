using Carter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ssa.CarSharing.Common.Presentation.Middlewares;

namespace Ssa.CarSharing.Users.API;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCarter();

        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen();

        return builder;
    }
}