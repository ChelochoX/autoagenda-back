using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Request;
using Dapper;
using Microsoft.Data.SqlClient;

namespace autoagenda_back.Repositories;

public class ClientesRepository : IClientesRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<ClientesRepository> _logger; 

    public ClientesRepository(ILogger<ClientesRepository> logger, DbConnections conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<ClienteDTO> BuscarClientePorCorreo(string correo)
    {
        _logger.LogInformation("Inicio del proceso de búsqueda de cliente con correo: {Correo}", correo);

        string query = @"
        SELECT id_cliente AS IdCliente, 
               nombre_completo AS NombreCompleto, 
               correo AS Correo, 
               telefono AS Telefono, 
               direccion AS Direccion
        FROM Clientes 
        WHERE correo = @correo";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var cliente = await connection.QueryFirstOrDefaultAsync<ClienteDTO>(query, new { correo });

                if (cliente == null)
                {
                    throw new NoDataFoundException($"No se encontró ningún cliente con el correo: {correo}");
                }

                _logger.LogInformation("Cliente encontrado con correo: {Correo}", correo);
                return cliente;
            }
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró un cliente con el correo proporcionado: {Correo}", correo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado al buscar el cliente con correo: {Correo}", correo);
            throw new RepositoryException("Ocurrió un error inesperado al buscar el cliente", ex);
        }
    }

    public async Task ActualizarCliente(int idCliente, ClienteDTO cliente)
    {
        _logger.LogInformation("Inicio del proceso para actualizar el cliente con ID: {IdCliente}", idCliente);

        string query = @"
        UPDATE Clientes 
        SET nombre_completo = @nombreCompleto, 
            telefono = @telefono, 
            direccion = @direccion
        WHERE id_cliente = @idCliente";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new
                {
                    idCliente,
                    cliente.NombreCompleto,
                    cliente.Telefono,
                    cliente.Direccion
                });

                if (filasAfectadas == 0)
                {
                    throw new NoDataFoundException($"No se encontró ningún cliente con el ID: {idCliente} para actualizar.");
                }

                _logger.LogInformation("Cliente con ID: {IdCliente} actualizado exitosamente.", idCliente);
            }
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró un cliente con el ID: {IdCliente} para actualizar.", idCliente);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado al actualizar el cliente con ID: {IdCliente}", idCliente);
            throw new RepositoryException("Ocurrió un error inesperado al actualizar el cliente.", ex);
        }
    }

    public async Task<int> InsertarCliente(ClienteRequest cliente)
    {
        _logger.LogInformation("Inicio del proceso para insertar un nuevo cliente: {NombreCompleto}", cliente.NombreCompleto);

        string query = @"
        INSERT INTO Clientes (nombre_completo, correo, telefono, direccion) 
        VALUES (@nombreCompleto, @correo, @telefono, @direccion);
        SELECT CAST(SCOPE_IDENTITY() AS INT)";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var idCliente = await connection.QuerySingleAsync<int>(query, new
                {
                    cliente.NombreCompleto,
                    cliente.Correo,
                    cliente.Telefono,
                    cliente.Direccion
                });

                _logger.LogInformation("Nuevo cliente insertado exitosamente con ID: {IdCliente}", idCliente);

                return idCliente;
            }
        }      
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado al insertar un nuevo cliente: {NombreCompleto}", cliente.NombreCompleto);
            throw new RepositoryException("Ocurrió un error inesperado al insertar un nuevo cliente.", ex);
        }
    }




}
