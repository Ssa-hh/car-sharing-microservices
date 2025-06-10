using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ssa.CarSharing.Common.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddCommonInfrastructure(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}