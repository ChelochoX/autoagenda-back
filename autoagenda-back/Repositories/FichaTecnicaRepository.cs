using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Request;
using Dapper;

namespace autoagenda_back.Repositories;

public class FichaTecnicaRepository : IFichaTecnicaRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<CitasRepository> _logger;

    public FichaTecnicaRepository(ILogger<CitasRepository> logger, DbConnections conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<FichaTecnicaDTO> GenerarFichaTecnicaAsync(FichaTecnicaRequest request)
    {
        _logger.LogInformation("Generando ficha técnica para la cita con ID {IdCita}.", request.IdCita);

        string insertQuery = @"
        INSERT INTO [FichaTecnicaVehiculo] (
            IdCita, 
            KilometrajeIngreso, 
            KilometrajeProximo, 
            DetallesServicio, 
            MecanicoResponsable, 
            Estado, 
            FechaCreacion
        )
        VALUES (
            @IdCita, 
            @KilometrajeIngreso, 
            @KilometrajeProximo, 
            @DetallesServicio, 
            @MecanicoResponsable, 
            'En Proceso', 
            GETDATE()
        );
        SELECT SCOPE_IDENTITY();";

        try
        {
            using System.Data.IDbConnection connection = _conexion.CreateSqlConnection();
            // Insertar la ficha técnica y obtener el ID generado
            int idFicha = await connection.ExecuteScalarAsync<int>(insertQuery, new
            {
                request.IdCita,
                request.KilometrajeIngreso,
                request.KilometrajeProximo,
                request.DetallesServicio,
                request.MecanicoResponsable
            });

            _logger.LogInformation("Ficha técnica generada con ID {IdFicha}.", idFicha);

            // Retornar los datos completos
            FichaTecnicaDTO fichaTecnica = await ObtenerFichaTecnicaCompletaAsync(idFicha);
            return fichaTecnica;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar la ficha técnica para la cita con ID {IdCita}.", request.IdCita);
            throw new RepositoryException("Error al generar la ficha técnica.", ex);
        }
    }

    public async Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha)
    {
        _logger.LogInformation("Obteniendo detalles completos de la ficha técnica con ID {IdFicha}.", idFicha);

        string query = @"
        SELECT 
            ft.IdFicha,
            ft.IdCita,
            ft.KilometrajeIngreso,
            ft.KilometrajeProximo,
            ft.DetallesServicio,
            ft.MecanicoResponsable,
            ft.Estado,
            ft.FechaCreacion,
            v.placa AS PlacaVehiculo,
            v.id_marca AS MarcaVehiculo,
            v.id_modelo AS ModeloVehiculo,
            u.nombre_completo AS NombreCliente,
            u.correo AS CorreoCliente,
            u.telefono AS TelefonoCliente
        FROM [FichaTecnicaVehiculo] ft
        INNER JOIN Citas c ON ft.IdCita = c.id_cita
        INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
        INNER JOIN Usuarios u ON c.id_usuario = u.id_usuario
        WHERE ft.IdFicha = @IdFicha";

        try
        {
            using System.Data.IDbConnection connection = _conexion.CreateSqlConnection();
            FichaTecnicaDTO? fichaTecnica = await connection.QueryFirstOrDefaultAsync<FichaTecnicaDTO>(query, new { IdFicha = idFicha });

            if (fichaTecnica == null)
            {
                _logger.LogWarning("No se encontró la ficha técnica con ID {IdFicha}.", idFicha);
                throw new KeyNotFoundException($"No se encontró la ficha técnica con ID {idFicha}.");
            }

            return fichaTecnica;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la ficha técnica con ID {IdFicha}.", idFicha);
            throw new RepositoryException("Error al obtener los detalles de la ficha técnica.", ex);
        }
    }

}
