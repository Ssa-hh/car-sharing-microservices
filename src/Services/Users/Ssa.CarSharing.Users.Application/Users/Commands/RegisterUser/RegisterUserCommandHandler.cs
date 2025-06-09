using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using MassTransit;
using Ssa.CarSharing.Common.Application.EventBus.Events;

namespace Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser
{
    internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPublishEndpoint _publishEndpoint;
        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if(await _userRepository.ExistsAsync(command.Email))
                return Result.Failure<Guid>(Error.Conflict("User.Conflict", "User with this email already exists."));

            User user = User.Create(command.FirstName, command.LastName, command.Email);

            string identityId = await _authenticationService.RegisterUser(user, command.Password, cancellationToken);

            user.IdentityId = identityId;

            // TODO: Implement Outbox pattern to ensure that the event is published only after the user is successfully registered
            await _publishEndpoint.Publish(new UserRegisteredIntegrationEvent { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email }
                , cancellationToken);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}
