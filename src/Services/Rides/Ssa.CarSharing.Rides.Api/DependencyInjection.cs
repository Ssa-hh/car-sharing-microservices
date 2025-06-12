using Carter;

namespace Ssa.CarSharing.Rides.Api
{
    public static class DependencyInjection
    {
        public static IHostApplicationBuilder AddApi(this IHostApplicationBuilder builder)
        {
            builder.Services.AddCarter();

            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen();

            return builder;
        }
    }
}
