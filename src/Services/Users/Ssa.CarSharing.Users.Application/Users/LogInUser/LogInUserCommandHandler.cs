using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.LogInUser
{
    internal class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
    {
        private readonly IJwtService _jwtService;

        public LogInUserCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        public async Task<Result<AccessTokenResponse>> Handle(LogInUserCommand command, CancellationToken cancellationToken)
        {
            Result<string> accessToken = await _jwtService.GetClientAccessTokenAsync(command.Email, command.Password, cancellationToken);

            if (accessToken.IsFailure)
            {
                return Result.Failure<AccessTokenResponse>(new Error("Users.InvalidCredentials", "The provided credentials were invalid", ErrorType.Failure));
            }

            return Result.Success(new AccessTokenResponse(accessToken.Value));
        }
    }
}
