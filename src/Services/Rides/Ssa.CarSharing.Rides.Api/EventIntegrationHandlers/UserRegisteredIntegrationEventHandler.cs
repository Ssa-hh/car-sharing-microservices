using MassTransit;
using MediatR;
using Ssa.CarSharing.Common.Application.EventBus.Events;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Application.Members.Commands.CreateMember;

namespace Ssa.CarSharing.Rides.Api.EventIntegrationHandlers;

public class UserRegisteredIntegrationEventHandler : IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ISender _sender;

    public UserRegisteredIntegrationEventHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        // TODO: log error validation and exception handling
        await _sender.Send(new CreateMemberCommand(context.Message.FirstName,context.Message.LastName, context.Message.Email), context.CancellationToken);
    }
}
