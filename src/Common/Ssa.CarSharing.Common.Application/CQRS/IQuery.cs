using MediatR;
using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Application.CQRS
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
}
