using autoagenda_back.Data;

namespace autoagenda_back.Configurations;

public static class ServiceConfiguration
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<DbConnections>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }    
}

