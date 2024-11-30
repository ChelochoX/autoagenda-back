using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using Dapper;

namespace autoagenda_back.Repositories;

public class VehiculosRepository : IVehiculosRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<VehiculosRepository> _logger;

    public VehiculosRepository(DbConnections conexion, ILogger<VehiculosRepository> logger)
    {
        _conexion = conexion;
        _logger = logger;
    }

    public async Task<IEnumerable<AnhoDTO>> ObtenerAnhosAsync()
    {
        _logger.LogInformation("Inicio del proceso para obtener años.");
        string query = @"
            SELECT 
                id_anho AS IdAnho, 
                anho AS Anho 
            FROM Anhos
            ORDER BY anho ASC";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                return await connection.QueryAsync<AnhoDTO>(query);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los años.");
            throw new RepositoryException("Error al obtener los años.", ex);
        }
    }

    public async Task<IEnumerable<MarcaDTO>> ObtenerMarcasAsync()
    {
        _logger.LogInformation("Inicio del proceso para obtener marcas.");
        string query = @"
            SELECT 
                id_marca AS IdMarca, 
                nombre AS Nombre 
            FROM Marcas
            ORDER BY nombre ASC";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                return await connection.QueryAsync<MarcaDTO>(query);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las marcas.");
            throw new RepositoryException("Error al obtener las marcas.", ex);
        }
    }

    public async Task<IEnumerable<ModeloDTO>> ObtenerModelosPorMarcaAsync(int idMarca)
    {
        _logger.LogInformation("Inicio del proceso para obtener modelos de la marca {IdMarca}.", idMarca);
        string query = @"
            SELECT 
                id_modelo AS IdModelo, 
                nombre AS Nombre, 
                id_marca AS IdMarca
            FROM Modelos
            WHERE id_marca = @IdMarca
            ORDER BY nombre ASC";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                return await connection.QueryAsync<ModeloDTO>(query, new { IdMarca = idMarca });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los modelos.");
            throw new RepositoryException("Error al obtener los modelos.", ex);
        }
    }
}
