using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Domain
{
    public record Error(string Code, string Description, ErrorType Type)
    {
        public static readonly Error None = new (string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("General.Null", "Null value was provided", ErrorType.Failure);

        public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);

        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

    }
}
