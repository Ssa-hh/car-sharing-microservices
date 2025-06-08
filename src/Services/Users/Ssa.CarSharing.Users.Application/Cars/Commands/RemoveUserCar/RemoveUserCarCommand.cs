using Ssa.CarSharing.Common.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.RemoveUserCar;

public record RemoveUserCarCommand(Guid CarId) : ICommand;
