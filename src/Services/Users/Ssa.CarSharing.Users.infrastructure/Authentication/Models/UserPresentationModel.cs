using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Authentication.Models
{
    internal class UserModel
    {
        public Dictionary<string, string>? Access { get; set; }

        public required Dictionary<string, List<string>> Attributes { get; set; }

        public Dictionary<string, string>? ClientRoles { get; set; }

        public required long CreatedTimestamp { get; set; }

        public CredentialModel[]? Credentials { get; set; }

        public string[]? DisableableCredentialTypes { get; set; }

        public required string Email { get; set; }

        public required bool EmailVerified { get; set; }

        public bool? Enabled { get; set; }

        public string? FederationLink { get; set; }

        public string? Id { get; set; }

        public string[]? Groups { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public int? NotBefore { get; set; }

        public string? Origin { get; set; }

        public string[]? RealmRoles { get; set; }

        public required string[] RequiredActions { get; set; }

        public string? Self { get; set; }

        public string? ServiceAccountClientId { get; set; }

        public required string Username { get; set; }

        internal static UserModel FromUser(Domain.Users.User user) =>
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Email,
                Enabled = true,
                EmailVerified = false,
                CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Attributes = new Dictionary<string, List<string>>(),
                RequiredActions = Array.Empty<string>()
            };
    }
}
