﻿using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Users.Commands.LogInUser;

namespace Ssa.CarSharing.Users.API.Endpoints;

internal record LogInUserRequest(string Email, string Password);
public class LogInUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login", async (LogInUserRequest request, ISender sender) =>
        {
            Result<AuthorizationTokenResponse> result = await sender.Send(new LogInUserCommand(request.Email, request.Password));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        })
        .WithName("LogInUser")
        .WithTags("Users")
        .Produces<AuthorizationTokenResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
