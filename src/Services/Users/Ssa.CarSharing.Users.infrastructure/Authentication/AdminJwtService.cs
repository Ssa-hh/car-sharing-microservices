using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.infrastructure.Authentication;
using Ssa.CarSharing.Users.infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Ssa.CarSharing.Users.infrastructure.Authentication
{
    internal class AdminJwtService
    {
        private static readonly Error AuthenticationFailed = new("Keycloak.AuthenticationFailed", "Failed to get access token due to authentication failure", ErrorType.Failure);

        private readonly HttpClient _httpClient;

        private readonly KeycloakOptions _keycloakOptions;

        private readonly KeycloakAdminOptions _keycloakAdminOptions;

        public AdminJwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions, IOptions<KeycloakAdminOptions> keycloakAdminOptions)
        {
            _httpClient = httpClient;
            _keycloakOptions = keycloakOptions.Value;
            _keycloakAdminOptions = keycloakAdminOptions.Value;
        }

        public async Task<Result<string>> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var authorizationRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakAdminOptions.AdminClientId),
                new("client_secret", _keycloakAdminOptions.AdminClientSecret),
                new("scope", "openid email"),
                new("grant_type", "client_credentials")
            };

            using var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);

            HttpResponseMessage response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            AuthorizationToken? authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken);

            if (authorizationToken is null)
                throw new ApplicationException("Failed to get admin access token from identity provider");

            return authorizationToken.AccessToken;
        }
    }
}
