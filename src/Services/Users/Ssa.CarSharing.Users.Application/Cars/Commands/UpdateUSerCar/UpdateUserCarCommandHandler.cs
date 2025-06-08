using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.UpdateUSerCar;

internal class UpdateUserCarCommandHandler : ICommandHandler<UpdateUserCarCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    public UpdateUserCarCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IUserContext userContext)
    {
        this._unitOfWork = unitOfWork;
        this._userRepository = userRepository;
        this._userContext = userContext;
    }

    public async Task<Result> Handle(UpdateUserCarCommand command, CancellationToken cancellationToken)
    {
        string userEmail = _userContext.UserEmail;

        User? currentUser = await _userRepository.GetByEmailAsync(userEmail, true);

        if (currentUser == null)
            return Result.Failure(Error.NotFound("Users.NotFound", "Current user not found"));

        Result result = currentUser.UpdateCar(command.carId, command.Brand, command.Model, ColorTranslator.FromHtml(command.ColorHexCode));

        if(result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
