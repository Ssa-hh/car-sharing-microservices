
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

        builder.AddDefaultMassTransit();

        builder.Services.AddApplication();

        builder.AddInfrastructure();
        builder.AddApi();

        // TODO: remove it or refine it
        var allowFrontendOrigins = "_allowFrontendOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: allowFrontendOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("*")
                                  .AllowAnyHeader()
                                        .AllowAnyMethod();
                              });
        });

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

        // TODO: remove it or refine it
        app.UseCors(allowFrontendOrigins);

        app.UseHttpsRedirection();

        app.UseMiddleware<CustomExceptionHandler>();

        app.UseAuthorization();

        app.MapCarter();

        app.Run();
    }
}
