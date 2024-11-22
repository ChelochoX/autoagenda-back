using autoagenda_back.Repositories;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Services;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IClientesRepository, ClientesRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
           services.AddSingleton<IClientesService, ClientesService>();
            return services;
        }
    }
}
