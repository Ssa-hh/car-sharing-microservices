using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Users.Queries.GetLoggedInUser
{
    public class UserResponse
    {
        public Guid Id { get; init; }

        public string Email { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
    }
}
