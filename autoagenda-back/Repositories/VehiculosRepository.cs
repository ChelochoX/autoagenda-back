using autoagenda_back.Data;
using autoagenda_back.DTOs;
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

    public async Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo)
    {
        _logger.LogInformation("Inicio del proceso para insertar un nuevo vehículo.");

        string query = @"
        INSERT INTO Vehiculos (id_cliente, marca, modelo, anho, placa)
        VALUES (@IdCliente, @Marca, @Modelo, @Anho, @Placa);
        SELECT CAST(SCOPE_IDENTITY() AS INT)";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var idVehiculo = await connection.QuerySingleAsync<int>(query, vehiculo);
                _logger.LogInformation("Vehículo insertado exitosamente con ID: {IdVehiculo}", idVehiculo);
                return idVehiculo;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar insertar un nuevo vehículo.");
            throw;
        }
    }

    public async Task<VehiculoDTO> ObtenerVehiculoPorIdAsync(int idVehiculo)
    {
        _logger.LogInformation("Inicio del proceso para obtener un vehículo con ID: {IdVehiculo}", idVehiculo);

        string query = @"
        SELECT id_vehiculo AS IdVehiculo, id_cliente AS IdCliente, 
               marca AS Marca, modelo AS Modelo, anho AS Anho, placa AS Placa
        FROM Vehiculos WHERE id_vehiculo = @IdVehiculo";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var vehiculo = await connection.QueryFirstOrDefaultAsync<VehiculoDTO>(query, new { idVehiculo });

                if (vehiculo == null)
                {
                    _logger.LogWarning("No se encontró un vehículo con ID: {IdVehiculo}", idVehiculo);
                    return null;
                }

                _logger.LogInformation("Vehículo encontrado con ID: {IdVehiculo}", idVehiculo);
                return vehiculo;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener el vehículo con ID: {IdVehiculo}", idVehiculo);
            throw;
        }
    }

    public async Task<IEnumerable<VehiculoDTO>> ObtenerVehiculosPorClienteAsync(int idCliente)
    {
        _logger.LogInformation("Inicio del proceso para obtener vehículos del cliente con ID: {IdCliente}", idCliente);

        string query = @"
        SELECT id_vehiculo AS IdVehiculo, id_cliente AS IdCliente, 
               marca AS Marca, modelo AS Modelo, anho AS Anho, placa AS Placa
        FROM Vehiculos WHERE id_cliente = @IdCliente";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var vehiculos = await connection.QueryAsync<VehiculoDTO>(query, new { idCliente });

                if (!vehiculos.Any())
                {
                    _logger.LogWarning("No se encontraron vehículos para el cliente con ID: {IdCliente}", idCliente);
                    return Enumerable.Empty<VehiculoDTO>();
                }

                _logger.LogInformation("Vehículos obtenidos exitosamente para el cliente con ID: {IdCliente}", idCliente);
                return vehiculos;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener los vehículos del cliente con ID: {IdCliente}", idCliente);
            throw;
        }
    }

    public async Task ActualizarVehiculoAsync(int idVehiculo, VehiculoDTO vehiculo)
    {
        _logger.LogInformation("Inicio del proceso para actualizar el vehículo con ID: {IdVehiculo}", idVehiculo);

        string query = @"
        UPDATE Vehiculos SET
            marca = @Marca,
            modelo = @Modelo,
            anho = @Anho,
            placa = @Placa
        WHERE id_vehiculo = @IdVehiculo";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new
                {
                    vehiculo.Marca,
                    vehiculo.Modelo,
                    vehiculo.Anho,
                    vehiculo.Placa,
                    idVehiculo
                });

                if (filasAfectadas == 0)
                {
                    _logger.LogWarning("No se encontró un vehículo con ID: {IdVehiculo} para actualizar.", idVehiculo);
                    throw new KeyNotFoundException($"No se encontró un vehículo con ID: {idVehiculo} para actualizar.");
                }

                _logger.LogInformation("Vehículo con ID: {IdVehiculo} actualizado exitosamente.", idVehiculo);
            }
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró un vehículo con ID: {IdVehiculo} para actualizar.", idVehiculo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar actualizar el vehículo con ID: {IdVehiculo}", idVehiculo);
            throw;
        }
    }

    public async Task EliminarVehiculoAsync(int idVehiculo)
    {
        _logger.LogInformation("Inicio del proceso para eliminar el vehículo con ID: {IdVehiculo}", idVehiculo);

        string query = "DELETE FROM Vehiculos WHERE id_vehiculo = @IdVehiculo";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new { idVehiculo });

                if (filasAfectadas == 0)
                {
                    _logger.LogWarning("No se encontró un vehículo con ID: {IdVehiculo} para eliminar.", idVehiculo);
                    throw new KeyNotFoundException($"No se encontró un vehículo con ID: {idVehiculo} para eliminar.");
                }

                _logger.LogInformation("Vehículo con ID: {IdVehiculo} eliminado exitosamente.", idVehiculo);
            }
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró un vehículo con ID: {IdVehiculo} para eliminar.", idVehiculo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar eliminar el vehículo con ID: {IdVehiculo}", idVehiculo);
            throw;
        }
    }
}
