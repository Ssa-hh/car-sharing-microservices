using MediatR;
using Carter;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Application.Rides;
using Ssa.CarSharing.Rides.Application.Rides.Commands.CreateRide;

namespace Ssa.CarSharing.Rides.Api.Endpoints;

public class CreateRide: ICarterModule
{
    internal record CreateRideRequest(CarDto Car, short NumberOfProposedSeats, DateTime StartsAtUtc, DateTime EndsAtUtc, string PickupCity, string DropOffCity);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/rides", async (CreateRideRequest request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateRideCommand(request.Car, request.NumberOfProposedSeats, request.StartsAtUtc, request.EndsAtUtc, request.PickupCity, request.DropOffCity));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("CreateRide")
        .WithSummary("Create ride")
        .WithDescription("Create ride");
    }
}
