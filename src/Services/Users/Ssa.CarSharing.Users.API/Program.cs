
using Carter;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Ssa.CarSharing.Common.Presentation.Middlewares;
using Ssa.CarSharing.Users.API.Endpoints;
using Ssa.CarSharing.Users.Application;
using Ssa.CarSharing.Users.infrastructure;
using Ssa.CarSharing.Users.infrastructure.Database.Extensions;
namespace Ssa.CarSharing.Users.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        string postgresConnectionName = "userdb";
        // Aspire client configuration
        builder.AddNpgsqlDataSource(postgresConnectionName);

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(builder.Configuration, postgresConnectionName);
        builder.Services.AddApi();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigration();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<CustomExceptionHandler>();

        app.UseAuthorization();

        app.MapCarter();

        app.Run();
    }
}
