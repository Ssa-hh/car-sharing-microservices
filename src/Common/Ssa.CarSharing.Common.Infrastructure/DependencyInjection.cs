using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Common.infrastructure.Authentication;

namespace Ssa.CarSharing.Common.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddCommonInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddAuthentication();
        return builder;
    }

    private static IHostApplicationBuilder AddAuthentication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("KeycloakSettings"));

        builder.Services.AddHttpClient<IJwtService, JwtService>(httpClient =>
        {
            KeycloakOptions keycloakOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<IUserContext, UserContext>();

        return builder;
    }
}