using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ssa.CarSharing.Common.Infrastructure;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;
using System.Reflection;

namespace Ssa.CarSharing.Rides.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonInfrastructure(configuration);

        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}
