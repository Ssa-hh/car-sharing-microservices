using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser;
using Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser;

namespace Ssa.CarSharing.Users.API.Endpoints
{
    public class GetLoggedInUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users/me", async (ISender sender) =>
            {
                Result<UserResponse> result = await sender.Send(new GetLoggedInUserQuery());

                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
            })
            .RequireAuthorization()
            .WithName("GetLoggedInUser")
            .WithTags("Users")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
