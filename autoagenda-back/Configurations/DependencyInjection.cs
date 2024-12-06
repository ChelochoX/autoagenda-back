﻿using autoagenda_back.Repositories;
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
            services.AddSingleton<ICitasRepository, CitasRepository>();
            services.AddSingleton<ITipoServiciosRepository, TipoServiciosRepository>();
            services.AddSingleton<IVehiculosRepository, VehiculosRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
           services.AddSingleton<IClientesService, ClientesService>();
           services.AddSingleton<ICitasService, CitasService>();
           services.AddSingleton<ITipoServiciosService, TipoServiciosService>();
           services.AddSingleton<IVehiculosService, VehiculosService>();
            return services;
        }
    }
}
