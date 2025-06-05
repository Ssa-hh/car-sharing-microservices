using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser
{
    internal class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            
            User? user = await _userRepository.GetByEmailAsync(_userContext.UserEmail);

            if (user == null) 
                return Result.Failure<UserResponse>(Error.NotFound("Users.NotFound", "The current user not found"));

            return new Result<UserResponse>(new UserResponse { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email }, true, Error.None);
        }
    }
}
