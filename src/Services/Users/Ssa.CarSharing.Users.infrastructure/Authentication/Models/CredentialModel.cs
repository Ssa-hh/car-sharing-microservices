using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Authentication.Models
{
    internal class CredentialModel
    {
        public string? Algorithm { get; set; }

        public string? Config { get; set; }

        public int Counter { get; set; }

        public long CreatedDate { get; set; }

        public string? Device { get; set; }

        public int Digits { get; set; }

        public int HashIterations { get; set; }

        public string? HashedSaltedValue { get; set; }

        public int Period { get; set; }

        public string? Salt { get; set; }

        public bool Temporary { get; set; }

        public string? Type { get; set; }

        public string? Value { get; set; }
    }
}
