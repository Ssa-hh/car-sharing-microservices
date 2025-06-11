using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.infrastructure.Authentication;
using Ssa.CarSharing.Common.Infrastructure;
using Ssa.CarSharing.User.infrastructure.Authentication;
using Ssa.CarSharing.Users.Application.Abstractions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.infrastructure.Authentication;
using Ssa.CarSharing.Users.infrastructure.Database;
using Ssa.CarSharing.Users.infrastructure.Database.Repositories;
using AuthenticationService = Ssa.CarSharing.Users.infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Ssa.CarSharing.Users.Application.Abstractions.IAuthenticationService;

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
        builder.Services.AddScoped<AdminJwtService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

        builder.Services.Configure<KeycloakAdminOptions>(builder.Configuration.GetSection("KeycloakAdminSettings"));

        builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(httpClient =>
        {
            KeycloakAdminOptions keycloakAdminOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakAdminOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakAdminOptions.AdminUrl);
        });

        builder.Services.AddHttpClient<AdminJwtService>(httpClient =>
        {
            KeycloakOptions keycloakOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        return builder;
    }
}
