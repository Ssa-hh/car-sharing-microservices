using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.Domain.Users.Cars;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.RemoveUserCar;

internal class RemoveUserCarCommandHandler : ICommandHandler<RemoveUserCarCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public RemoveUserCarCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(RemoveUserCarCommand command, CancellationToken cancellationToken)
    {
        string userEmail = _userContext.UserEmail;

        User? currentUser = await _userRepository.GetByEmailAsync(userEmail, true);

        if (currentUser == null)
            return Result.Failure(Error.NotFound("User.NotFound", "The current user not found"));

        Result result = currentUser.RemoveCar(command.CarId);

        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
