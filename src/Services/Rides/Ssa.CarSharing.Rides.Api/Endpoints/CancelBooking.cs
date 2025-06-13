using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Rides.Application.Rides.Commands.CancelBooking;
using static Ssa.CarSharing.Rides.Api.Endpoints.CreateRide;

namespace Ssa.CarSharing.Rides.Api.Endpoints;

public class CancelBooking : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/rides/{rideId}/bookings/mine", async (Guid rideId, ISender sender) =>
        {
            Result result = await sender.Send(new CancelBookingCommand(rideId));

            return result.IsSuccess ? Results.Ok() : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("CancelRideBooking")
        .WithSummary("Cancel ride booking")
        .WithDescription("Cancel ride booking");
    }
}
