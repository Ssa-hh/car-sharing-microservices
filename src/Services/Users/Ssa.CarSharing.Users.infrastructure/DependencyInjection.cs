using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.infrastructure.Authentication;
using Ssa.CarSharing.Users.infrastructure.Database;
using Ssa.CarSharing.Users.infrastructure.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService = Ssa.CarSharing.Users.infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Ssa.CarSharing.Users.Application.Abstructions.IAuthenticationService;

namespace Ssa.CarSharing.Users.infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            String connectionString = configuration.GetConnectionString(connectionName)!;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddAuthentication(configuration);

            return services;
        }

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            KeycloakOptions keycloakOptions;

            services.AddScoped<JwtService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.Configure<KeycloakOptions>(configuration.GetSection("KeycloakSettings"));

            keycloakOptions = services.BuildServiceProvider().GetRequiredService<IOptions<KeycloakOptions>>().Value;
            services.AddHttpClient<IAuthenticationService, AuthenticationService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            });

            services.AddHttpClient<JwtService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });

            services.AddAuthentication()
                .AddKeycloakJwtBearer(serviceName: "keycloak",
                realm: keycloakOptions.Realm,
                options =>
                {
                    options.Audience = configuration["Authentication:Audience"];
                    options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"]!);
                });

            services.AddAuthorization();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();
        }
  }
}
