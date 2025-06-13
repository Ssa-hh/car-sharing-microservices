using MediatR;
using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Domain.Rides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.BookRide
{
    internal class BookRideCommandHandler : ICommandHandler<BookRideCommand, Guid>
    {
        private readonly IRideRepository _rideRepository;
        private readonly IUserContext _userContext;
        private readonly IMemberRepository _memberRepository;

        public BookRideCommandHandler(IRideRepository rideRepository, IUserContext userContext, IMemberRepository memberRepository)
        {
            _rideRepository = rideRepository;
            _userContext = userContext;
            _memberRepository = memberRepository;
        }

        public async Task<Result<Guid>> Handle(BookRideCommand command, CancellationToken cancellationToken)
        {
            string currentMemberEmail = _userContext.UserEmail;

            Member? currentMember = await _memberRepository.GetByEmailAsync(currentMemberEmail);

            if (currentMember == null)
                return Result.Failure<Guid>(Error.NotFound("Members.NotFound", "Current member not found."));

            Ride? rideToBook = await _rideRepository.GetByIdAsync(command.RideId, cancellationToken);

            if(rideToBook is null)
                return Result.Failure<Guid>(Error.NotFound("Rides.NotFound", "Requested ride not found."));

            Result<Guid> result = rideToBook.Book(currentMember, command.NumberOfRequestedSeats);

            if(result.IsFailure)
                return result;

            var nbrModified = await _rideRepository.ReplaceAsync(rideToBook, cancellationToken);

            if(nbrModified==0)
                return Result.Failure<Guid>(Error.NotFound("Rides.FailUpdate", "Fail to update database."));

            return Result.Success(result.Value);
        }
    }
}
