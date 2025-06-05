using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser;

namespace Ssa.CarSharing.Users.API.Endpoints
{
    public class RegisterUser :ICarterModule
    {
        internal record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/register", async (RegisterUserRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password));

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            })
            .WithName("RegisterUser")
            .WithTags("Users")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
