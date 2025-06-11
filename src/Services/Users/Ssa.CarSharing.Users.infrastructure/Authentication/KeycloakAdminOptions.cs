namespace Ssa.CarSharing.User.infrastructure.Authentication;

public class KeycloakAdminOptions
{
    public string AdminUrl { get; set; } = string.Empty;

    public string AdminClientId { get; init; } = string.Empty;

    public string AdminClientSecret { get; init; } = string.Empty;
}
