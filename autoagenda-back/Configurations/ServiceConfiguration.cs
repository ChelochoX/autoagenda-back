using autoagenda_back.Data;
using System.Data;

namespace autoagenda_back.Configurations;

public static class ServiceConfiguration
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton<DbConnections>();
        // Registra IDbConnection como Transient
        _ = services.AddTransient<IDbConnection>(sp =>
        {
            // Obtiene la instancia de DbConnections
            DbConnections dbConnections = sp.GetRequiredService<DbConnections>();
            // Crea y devuelve una conexión SQL configurada
            return dbConnections.CreateSqlConnection();
        });

        _ = services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    _ = policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}

