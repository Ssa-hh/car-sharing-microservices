using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Ssa.CarSharing.Users.Application.Cars.Commands.UpdateUSerCar;

internal class UpdateUserCarCommandValidator : AbstractValidator<UpdateUserCarCommand>
{
    public UpdateUserCarCommandValidator()
    {
        RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(100).WithMessage("Car brand must not exceed 100 characters");
        RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.")
            .MaximumLength(100).WithMessage("Car model must not exceed 100 characters");
        RuleFor(x => x.NumberOfSeats).GreaterThanOrEqualTo((short)2)
            .WithMessage("The car must have a minimum of two seats.");
        RuleFor(x => x.ColorHexCode).Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
            .When(x => string.IsNullOrWhiteSpace(x.ColorHexCode))
            .WithMessage("Color must be a valid hex color code.");
    }
}
