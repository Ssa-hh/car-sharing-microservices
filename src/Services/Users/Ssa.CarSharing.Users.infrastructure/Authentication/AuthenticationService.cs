using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstractions;
using Ssa.CarSharing.Users.infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Ssa.CarSharing.Users.infrastructure.Authentication;

internal class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AdminJwtService _adminJwtService;

    public AuthenticationService(HttpClient httpClient, AdminJwtService adminJwtService)
    {
        _httpClient = httpClient;
        _adminJwtService = adminJwtService;
    }

    public async Task<string> RegisterUser(Domain.Users.User user, string password, CancellationToken cancellationToken = default)
    {
        UserModel userModel = UserModel.FromUser(user);

        userModel.Credentials = new[]
        {
            new CredentialModel
            {
                Type = "password",
                Value = password,
                Temporary = false
            }
        };

        Result<string> accessToken = await _adminJwtService.GetAdminAccessTokenAsync(cancellationToken);

        _httpClient.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken.Value);

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("users", userModel, cancellationToken);

        response.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header can't be null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string userIdentityId = locationHeader.Substring(
            userSegmentValueIndex + usersSegmentName.Length);

        return userIdentityId;
    }
}
