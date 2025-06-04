using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser
{
    internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }
        public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            User user = User.Create(command.FirstName, command.LastName, command.Email);

            string identityId = await _authenticationService.RegisterUser(user, command.Password, cancellationToken);

            user.IdentityId = identityId;

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}
