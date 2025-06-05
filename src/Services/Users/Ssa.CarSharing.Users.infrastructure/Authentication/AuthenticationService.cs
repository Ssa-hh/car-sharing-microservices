using Microsoft.AspNetCore.Authentication.JwtBearer;
using Npgsql.TypeMapping;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Abstructions;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.infrastructure.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtService _jwtService;

        public AuthenticationService(HttpClient httpClient, IJwtService jwtService)
        {
            _httpClient = httpClient;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterUser(User user, string password, CancellationToken cancellationToken = default)
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

            Result<string> accessToken = await _jwtService.GetAdminAccessTokenAsync(cancellationToken);

            if (accessToken.IsFailure)
            {
                throw new ApplicationException("Failed to get admin access token from identity provider");
            }

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
}
