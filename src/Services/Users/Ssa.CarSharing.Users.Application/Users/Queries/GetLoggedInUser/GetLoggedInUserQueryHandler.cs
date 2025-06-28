using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ssa.CarSharing.Users.Application.Users.Dtos;

namespace Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser
{
    internal class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result<UserDto>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            
            User? user = await _userRepository.GetByEmailAsync(_userContext.UserEmail, true);

            if (user == null) 
                return Result.Failure<UserDto>(Error.NotFound("User.NotFound", "The current user not found"));

            return new Result<UserDto>(UserDto.FromUser(user), true, Error.None);
        }
    }
}
