using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstractions;
using Ssa.CarSharing.Users.Application.Dtos;
using Ssa.CarSharing.Users.infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Ssa.CarSharing.Users.infrastructure.Authentication;

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

    public async Task<Result<AuthorizationTokenDto>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
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
                return Result.Failure<AuthorizationTokenDto>(AuthenticationFailed);
            }

            AuthorizationTokenDto authorizationDto = new AuthorizationTokenDto(authorizationToken.AccessToken, authorizationToken.IdToken, authorizationToken.ExpireIn, 
                authorizationToken.RefreshToken, authorizationToken.RefreshExpireIn, authorizationToken.TokenType);
            
            return Result.Success<AuthorizationTokenDto>(authorizationDto);
        }
        catch (HttpRequestException)
        {

            return Result.Failure<AuthorizationTokenDto>(AuthenticationFailed);
        }
    }
}
