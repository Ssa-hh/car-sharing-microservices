using Ssa.CarSharing.Common.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.AddCarToUser;

public record AddCarToUserCommand(string Brand, string Model, short NumberOfSeats, string ColorHexCode) : ICommand<Guid>;
