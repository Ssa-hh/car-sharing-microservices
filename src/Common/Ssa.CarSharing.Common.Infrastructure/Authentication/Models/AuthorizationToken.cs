using System.Text.Json.Serialization;

namespace Ssa.CarSharing.Common.infrastructure.Authentication.Models;

public class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}
