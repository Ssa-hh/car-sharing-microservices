using Carter;
using MediatR;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Presentation.Helpers;
using Ssa.CarSharing.Users.Application.Cars.Commands.RemoveUserCar;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.Domain.Users.Cars;

namespace Ssa.CarSharing.Users.API.Endpoints.Cars
{
    public class RemoveUserCar : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/users/me/cars/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(new RemoveUserCarCommand(id));

                return result.IsSuccess ? Results.Ok() : ApiResults.Problem(result);
            })
            .RequireAuthorization()
            .WithName("RemoveUserCar")
            .WithSummary("Remove user car")
            .WithDescription("Remove user car");
        }
    }
}
