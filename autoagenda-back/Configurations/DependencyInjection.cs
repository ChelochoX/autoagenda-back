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
            _ = services.AddSingleton<ICitasRepository, CitasRepository>();
            _ = services.AddSingleton<ITipoServiciosRepository, TipoServiciosRepository>();
            _ = services.AddSingleton<IVehiculosRepository, VehiculosRepository>();
            _ = services.AddSingleton<IUsuariosRepository, UsuariosRepository>();
            _ = services.AddSingleton<IFichaTecnicaRepository, FichaTecnicaRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddSingleton<ICitasService, CitasService>();
            _ = services.AddSingleton<ITipoServiciosService, TipoServiciosService>();
            _ = services.AddSingleton<IVehiculosService, VehiculosService>();
            _ = services.AddSingleton<IUsuariosService, UsuarioService>();
            _ = services.AddSingleton<IFichaTecnicaService, FichaTecnicaService>();
            return services;
        }
    }
}
