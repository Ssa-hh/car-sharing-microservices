using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Commands.LogInUser;

internal class LogInUserCommandValidator : AbstractValidator<LogInUserCommand>
{
    public LogInUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
