using FluentValidation;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.BookRide;

internal class BookRideCommandValidator:AbstractValidator<BookRideCommand>
{
    public BookRideCommandValidator()
    {
        RuleFor(b => b.NumberOfRequestedSeats).GreaterThanOrEqualTo((short)1).WithMessage("The number of requested seats must be less than the number of available seats.");
    }
}
