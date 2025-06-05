using Carter;

namespace Ssa.CarSharing.Users.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services) {
            services.AddCarter();

            return services;
        }
    }
}
