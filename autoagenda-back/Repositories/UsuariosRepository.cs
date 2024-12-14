using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using Dapper;

namespace autoagenda_back.Repositories;

public class UsuariosRepository : IUsuariosRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<CitasRepository> _logger;

    public UsuariosRepository(ILogger<CitasRepository> logger, DbConnections conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int idUsuario)
    {
        _logger.LogInformation("Buscando usuario con ID: {IdUsuario}", idUsuario);

        string query = @"
        SELECT 
            u.id_usuario AS IdUsuario, 
            u.nombre_completo AS NombreCompleto, 
            u.correo AS Correo, 
            u.telefono AS Telefono,
            u.direccion AS Direccion,
            u.cedula AS Cedula,
            u.ruc AS Ruc,
            u.DIV AS DIV
        FROM Usuarios u
        WHERE u.id_usuario = @IdUsuario";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<UsuarioDTO>(query, new { idUsuario });

                if (usuario == null)
                {
                    throw new NoDataFoundException($"No se encontró ningún usuario con el ID: {idUsuario}");
                }

                _logger.LogInformation("Usuario encontrado con ID: {IdUsuario}", idUsuario);
                return usuario;
            }
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró el usuario con ID: {IdUsuario}", idUsuario);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al obtener el usuario con ID: {IdUsuario}", idUsuario);
            throw new RepositoryException("Error al obtener el usuario.", ex);
        }
    }

}
