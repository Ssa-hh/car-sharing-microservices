using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Rides.Application.Rides.Commands.BookRide;

namespace Ssa.CarSharing.Rides.Api.Endpoints;

public class BookRide : ICarterModule
{
    private record BookRideRequest(short numberOfRequestedSeats);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/rides/{rideId}/bookings/", async (Guid rideId, BookRideRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new BookRideCommand(rideId, request.numberOfRequestedSeats));

            return result.IsSuccess ? Results.Ok() : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("Book Ride")
        .WithSummary("Book ride")
        .WithDescription("Book ride");
    }
}