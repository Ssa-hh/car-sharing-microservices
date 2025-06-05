using Carter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ssa.CarSharing.Common.Presentation.Middlewares;

namespace Ssa.CarSharing.Users.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services) {
        services.AddCarter();

        return services;
    }
}