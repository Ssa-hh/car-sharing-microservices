using Microsoft.VisualBasic;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser;
using Ssa.CarSharing.Users.Domain.Users;
using System.Drawing;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.AddCarToUser
{
    internal class AddCarToUserCommandHandler : ICommandHandler<AddCarToUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _usrContext;

        public AddCarToUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IUserContext usrContext)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _usrContext = usrContext;
        }
        public async Task<Result> Handle(AddCarToUserCommand command, CancellationToken cancellationToken)
        {
            string userEmail = _usrContext.UserEmail;

            User? user = await _userRepository.GetByEmailAsync(userEmail, true);

            if (user is null) 
                return Result.Failure<UserResponse>(Error.NotFound("User.NotFound", "The current user not found"));

            Color carColor = command.ColorHexCode is null ? Color.Empty : ColorTranslator.FromHtml(command.ColorHexCode);
            var newCar = user.AddCar(command.Brand, command.Model, carColor, user.Id);

            await _userRepository.AddCarAsync(newCar);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
