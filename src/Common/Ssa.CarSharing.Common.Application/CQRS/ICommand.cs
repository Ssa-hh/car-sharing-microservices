using MediatR;
using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Application.CQRS
{
    public interface ICommand : IRequest<Result>, ICommandBase;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase;

    public interface ICommandBase;
}
