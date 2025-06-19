namespace Ssa.CarSharing.Users.Application.Users.Commands.LogInUser;

public record AuthorizationTokenResponse(String AccessToken, int ExpireIn);
