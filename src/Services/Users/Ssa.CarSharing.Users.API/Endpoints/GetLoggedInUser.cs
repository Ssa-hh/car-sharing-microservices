using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Users.Application.Users.Dtos;
using Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser;

namespace Ssa.CarSharing.Users.API.Endpoints;

public class GetLoggedInUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/me", async (ISender sender) =>
        {
            Result<UserDto> result = await sender.Send(new GetLoggedInUserQuery());

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("GetLoggedInUser")
        .WithTags("Users")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
