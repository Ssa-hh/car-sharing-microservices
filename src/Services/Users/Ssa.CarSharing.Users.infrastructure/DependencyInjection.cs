using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.Infrastructure;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.infrastructure.Authentication;
using Ssa.CarSharing.Users.infrastructure.Database;
using Ssa.CarSharing.Users.infrastructure.Database.Repositories;
using AuthenticationService = Ssa.CarSharing.Users.infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Ssa.CarSharing.Users.Application.Abstructions.IAuthenticationService;

namespace Ssa.CarSharing.Users.infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        String connectionString, connectionStringName;

        builder.AddCommonInfrastructure();

        connectionStringName = builder.Configuration["Postgres:ConnectionName"]!;
        connectionString = builder.Configuration.GetConnectionString(connectionStringName)!;
        builder.AddNpgsqlDataSource(connectionStringName);
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        builder.AddAuthentication();

        return builder;
    }

    private static IHostApplicationBuilder AddAuthentication(this IHostApplicationBuilder builder)
    {
        KeycloakOptions keycloakOptions;

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("KeycloakSettings"));

        keycloakOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakOptions>>().Value;
        builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        });

        builder.Services.AddHttpClient<IJwtService, JwtService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        builder.Services.AddAuthentication()
            .AddKeycloakJwtBearer(serviceName: "keycloak",
            realm: keycloakOptions.Realm,
            options =>
            {
                options.Audience = builder.Configuration["Authentication:Audience"];
                options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Authentication:RequireHttpsMetadata"]!);
            });

        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<IUserContext, UserContext>();

        return builder;
    }
}
