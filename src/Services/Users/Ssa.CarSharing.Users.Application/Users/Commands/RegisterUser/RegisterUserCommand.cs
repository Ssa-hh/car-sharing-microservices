using Ssa.CarSharing.Common.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string FirstName, string LastName, string Email) : ICommand<Guid>;
}
