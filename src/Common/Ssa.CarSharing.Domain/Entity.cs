using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Domain
{
    public class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; init; }
    }
}
