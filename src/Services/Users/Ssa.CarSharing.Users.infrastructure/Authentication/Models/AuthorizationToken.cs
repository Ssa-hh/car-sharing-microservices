using System.Text.Json.Serialization;

namespace Ssa.CarSharing.Users.infrastructure.Authentication.Models;

public class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}
