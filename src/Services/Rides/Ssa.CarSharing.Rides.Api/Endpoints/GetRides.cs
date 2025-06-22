using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Rides.Application.Rides;
using Ssa.CarSharing.Rides.Application.Rides.Queries;

namespace Ssa.CarSharing.Rides.Api.Endpoints;

public class GetRides : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/rides", async (DateOnly? rideDate, string pickupCity, string dropOffCity, ISender sender) =>
        {
            Result<List<RideDto>> result = await sender.Send(new GetRidesQuery(rideDate, pickupCity, dropOffCity));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResults.Problem(result);
        })
        .WithName("GetRides")
        .WithSummary("Get rides")
        .WithDescription("Get rides");
    }
}
