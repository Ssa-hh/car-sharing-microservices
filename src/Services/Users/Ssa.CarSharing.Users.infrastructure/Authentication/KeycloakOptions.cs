namespace Ssa.CarSharing.Users.infrastructure.Authentication;

public class KeycloakOptions
{
    public string TokenUrl { get; set; } = string.Empty;

    public string AuthClientId { get; init; } = string.Empty;

    public string AuthClientSecret { get; init; } = string.Empty;

    public string Realm { get; init; } = string.Empty;
}
