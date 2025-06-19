using MediatR;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstractions;
using Ssa.CarSharing.Users.Application.Dtos;
using Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Commands.LogInUser
{
    internal class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AuthorizationTokenResponse>
    {
        private readonly IJwtService _jwtService;

        public LogInUserCommandHandler(IJwtService jwtService, ISender sender)
        {
            _jwtService = jwtService;
        }
        public async Task<Result<AuthorizationTokenResponse>> Handle(LogInUserCommand command, CancellationToken cancellationToken)
        {
            Result<AuthorizationTokenDto> authToken = await _jwtService.GetClientAccessTokenAsync(command.Email, command.Password, cancellationToken);

            if (authToken.IsFailure)
            {
                return Result.Failure<AuthorizationTokenResponse>(new Error("Users.InvalidCredentials", "The provided credentials were invalid", ErrorType.Failure));
            }

            return Result.Success(new AuthorizationTokenResponse(authToken.Value.AccessToken, authToken.Value.ExpireIn));
        }
    }
}
