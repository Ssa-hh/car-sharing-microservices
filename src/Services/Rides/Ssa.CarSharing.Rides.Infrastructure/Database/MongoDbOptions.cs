using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Infrastructure.Database
{
    internal class MongoDbOptions
    {
        public string DatabaseName { get; init; }

        public string MemberCollectionName { get; init; }

        public string RideCollectionName { get; init; }
    }
}
