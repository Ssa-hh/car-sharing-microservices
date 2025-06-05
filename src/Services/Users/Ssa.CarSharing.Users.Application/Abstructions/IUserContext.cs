using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Abstructions
{
    public interface IUserContext
    {
        string UserEmail { get; }

        string IdentityId { get; }
    }
}
