using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using Dapper;

namespace autoagenda_back.Repositories;

public class TipoServiciosRepository : ITipoServiciosRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<TipoServiciosRepository> _logger;

    public TipoServiciosRepository(ILogger<TipoServiciosRepository> logger, DbConnections conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<IEnumerable<TipoServicioDTO>> ObtenerTodosLosTiposDeServicioAsync()
    {
        _logger.LogInformation("Inicio del proceso para obtener todos los tipos de servicio.");

        string query = @"
            SELECT 
                id_tipo_servicio AS IdTipoServicio,
                nombre AS Nombre,
                descripcion AS Descripcion,
                costo AS Costo
            FROM TipoServicio
            ORDER BY nombre ASC";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var tiposDeServicio = await connection.QueryAsync<TipoServicioDTO>(query);
                _logger.LogInformation("Se han obtenido {Cantidad} tipos de servicio.", tiposDeServicio.Count());
                return tiposDeServicio;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener los tipos de servicio.");
            throw new RepositoryException("Error al intentar obtener los tipos de servicio.", ex);
        }
    }
}
