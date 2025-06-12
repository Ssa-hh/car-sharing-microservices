using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Rides.Application.Rides.Commands.DeleteRide;

namespace Ssa.CarSharing.Rides.Api.Endpoints;

public class DeleteRide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/rides/{id}", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteRideCommand(id));

            return result.IsSuccess ? Results.Ok() : ApiResults.Problem(result);
        })
        .RequireAuthorization()
        .WithName("DeleteRide")
        .WithSummary("Delete ride")
        .WithDescription("Delete ride");
    }
}
