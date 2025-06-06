using Ssa.CarSharing.Common.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Commands.LogInUser;

public record class LogInUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;
