using MediatR;
using Microsoft.Extensions.Logging;
using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Application.Behavios;

public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;
    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string serviceName = GetServiceName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing request {RequestName} of {ServiceName} service", requestName, serviceName);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            _logger.LogInformation("Completed request {RequestName}", requestName);
        }
        else
        {
            _logger.LogError("Completed request {RequestName} with error", requestName);   
        }
        return result;
    }

    private static string GetServiceName(string requestName) => requestName.Split('.')[2];
}
