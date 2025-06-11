using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Common.infrastructure.Authentication.Models;
using Ssa.CarSharing.Common.Application.Authentication;
using System.Net.Http.Json;

namespace Ssa.CarSharing.Common.infrastructure.Authentication;

internal class JwtService : IJwtService
{
    private static readonly Error AuthenticationFailed = new("Keycloak.AuthenticationFailed", "Failed to get access token due to authentication failure", ErrorType.Failure);

    private readonly HttpClient _httpClient;

    private readonly KeycloakOptions _keycloakOptions;

    public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions.Value;
    }

    public async Task<Result<string>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keycloakOptions.AuthClientId),
            new("client_secret", _keycloakOptions.AuthClientSecret),
            new("scope", "openid email"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };

        try
        {
            using var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);

            HttpResponseMessage response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            AuthorizationToken? authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken);

            if (authorizationToken is null)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }

            return authorizationToken.AccessToken;
        }
        catch (HttpRequestException)
        {

            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}
