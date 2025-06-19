using System.Text.Json.Serialization;

namespace Ssa.CarSharing.Users.infrastructure.Authentication.Models;

public class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;

    [JsonPropertyName("expires_in")]
    public int ExpireIn { get; init; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpireIn { get; init; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = string.Empty;

    [JsonPropertyName("id_token")]
    public string IdToken { get; init; } = string.Empty;
}
