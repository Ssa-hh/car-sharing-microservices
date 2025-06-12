using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ssa.CarSharing.Common.Infrastructure;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Domain.Rides;
using Ssa.CarSharing.Rides.Infrastructure.Database;
using Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;
using System.Reflection;

namespace Ssa.CarSharing.Rides.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddCommonInfrastructure();

        builder.AddDatabase(builder.Configuration);

        return builder;
    }

    private static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddMongoDBClient("ridedb");

        builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDb"));

        builder.Services.AddScoped<IMemberRepository, MemberRepository>();

        builder.Services.AddScoped<IRideRepository, RideRepository>();

        return builder;
    }
}
