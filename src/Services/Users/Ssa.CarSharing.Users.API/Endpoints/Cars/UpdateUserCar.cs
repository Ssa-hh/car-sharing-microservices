using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Users.Application.Cars.Commands.UpdateUSerCar;

namespace Ssa.CarSharing.Users.API.Endpoints.Cars;

public class UpdateUserCar : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/me/cars/{id}", async (Guid id, UpdateUserCarRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateUserCarCommand(id, request.Brand, request.Model, request.ColorHexCode));

            return result.IsSuccess ? Results.Ok() : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("UpdateUserCar")
        .WithSummary("Update user car")
        .WithDescription("Update user car");
    }

    record UpdateUserCarRequest(string Brand, string Model, string ColorHexCode);
}
