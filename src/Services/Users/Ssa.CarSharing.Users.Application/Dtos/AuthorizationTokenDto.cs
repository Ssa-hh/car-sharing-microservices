namespace Ssa.CarSharing.Users.Application.Dtos;

public record AuthorizationTokenDto(string AccessToken, String IdToken, int ExpireIn, string RefreshToken, int RefreshExpireIn, string TokenType);