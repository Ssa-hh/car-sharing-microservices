using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ssa.CarSharing.Common.Infrastructure;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Infrastructure.Database;
using Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;
using System.Reflection;

namespace Ssa.CarSharing.Rides.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddCommonInfrastructure(configuration);

        builder.AddDatabase(configuration);

        return builder;
    }

    private static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddMongoDBClient("ridedb");

        builder.Services.Configure<MongoDbOptions>(configuration.GetSection("MongoDb"));

        builder.Services.AddScoped<IMemberRepository, MemberRepository>();

        return builder;
    }
}
