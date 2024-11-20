namespace autoagenda_back.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IMotoRepository, MotoRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMotoService, MotoService>();
            return services;
        }
    }
}
