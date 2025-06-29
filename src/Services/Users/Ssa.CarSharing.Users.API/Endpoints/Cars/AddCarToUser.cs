using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Users.Application.Cars.Commands;
using Ssa.CarSharing.Users.Application.Cars.Commands.AddCarToUser;
using System.Drawing;

namespace Ssa.CarSharing.Users.API.Endpoints.Cars;

public class AddCarToUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/me/cars", async (AddCarToUserRequest request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new AddCarToUserCommand(request.Brand, request.Model, request.NumberOfSeats, request.ColorHexCode));
            return result.IsSuccess ? Results.Ok(result.Value) : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("AddCarToUser")
        .WithTags("Users")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private record AddCarToUserRequest(string Brand, string Model, short NumberOfSeats, string ColorHexCode);
}
