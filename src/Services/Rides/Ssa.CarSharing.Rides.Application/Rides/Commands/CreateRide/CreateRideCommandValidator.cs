using FluentValidation;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.CreateRide;

internal class CreateRideCommandValidator:AbstractValidator<CreateRideCommand>
{
    public CreateRideCommandValidator()
    {
        RuleFor(r => r.StartsAtUtc).GreaterThan(DateTime.UtcNow).WithMessage("Departure time must in the future.");
        RuleFor(r => r.EndsAtUtc).GreaterThan(r=>r.StartsAtUtc).WithMessage("Arrival time must be later than departure time.");
        RuleFor(r => r.PickupCity).NotEmpty().WithMessage("Pick up city is required");
        RuleFor(r => r.DropOffCity).NotEmpty().WithMessage("Drop off city is required");
        RuleFor(r => r.NumberOfProposedSeats).GreaterThan((short)0).WithMessage("The proposed number of seats must be at least one");
        
        // Car validation
        RuleFor(r=>r.Car).NotNull().WithMessage("Car is required.");
        RuleFor(r => r.Car.Brand).NotEmpty().WithMessage("Car brand is required.");
        RuleFor(r => r.Car.Model).NotEmpty().WithMessage("Car model is required.");
        RuleFor(r => r.Car.NumberOfSeats).GreaterThan((short)0).WithMessage("The number of the car seats must be at least one");
        RuleFor(r =>r.Car.ColorHExCode).Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .When(r => !string.IsNullOrWhiteSpace(r.Car.ColorHExCode))
                .WithMessage("Color must be a valid hex color code.");

        RuleFor(r => r.NumberOfProposedSeats).LessThan(r => r.Car.NumberOfSeats)
            .WithMessage("The proposed number of seats must be less than the total number of seats in the car.");
    }
}
