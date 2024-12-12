using autoagenda_back.Data;
using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using Dapper;

namespace autoagenda_back.Repositories;

public class CitasRepository : ICitasRepository
{
    private readonly DbConnections _conexion;
    private readonly ILogger<CitasRepository> _logger;

    public CitasRepository(ILogger<CitasRepository> logger, DbConnections conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<int> InsertarCita(CitaDTO cita)
    {
        _logger.LogInformation("Inicio del proceso para insertar una nueva cita.");

        string query = @"
                INSERT INTO Citas (id_vehiculo, fecha, hora, id_tipo_servicio, estado, descripcion, id_usuario)
                VALUES (@IdVehiculo, @Fecha, @Hora, @IdTipoServicio, @Estado, @Descripcion, @IdUsuario);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var idCita = await connection.QuerySingleAsync<int>(query, cita);
                _logger.LogInformation("Cita insertada exitosamente con ID: {IdCita}", idCita);
                return idCita;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al insertar la cita.");
            throw new RepositoryException("Error al insertar la cita.", ex);
        }
    }

    public async Task<CitaDTO> ObtenerCitaPorId(int idCita)
    {
        _logger.LogInformation("Buscando cita con ID: {IdCita}", idCita);

        string query = @"
            SELECT c.id_cita AS IdCita, c.id_vehiculo AS IdVehiculo, c.fecha, c.hora, 
                   c.id_tipo_servicio AS IdTipoServicio, c.estado, c.descripcion,
                   ts.nombre AS TipoServicioNombre, ts.descripcion AS TipoServicioDescripcion, ts.costo AS TipoServicioCosto
            FROM Citas c
            INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
            WHERE c.id_cita = @IdCita";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var cita = await connection.QueryFirstOrDefaultAsync<CitaDTO>(query, new { idCita });

                if (cita == null)
                {
                    throw new NoDataFoundException($"No se encontró ninguna cita con el ID: {idCita}");
                }

                _logger.LogInformation("Cita encontrada con ID: {IdCita}", idCita);
                return cita;
            }
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning(ex, "No se encontró la cita con ID: {IdCita}", idCita);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al obtener la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al obtener la cita.", ex);
        }
    }

    public async Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculo(int idVehiculo)
    {
        _logger.LogInformation("Buscando citas para el vehículo con ID: {IdVehiculo}", idVehiculo);

        string query = @"
            SELECT c.id_cita AS IdCita, c.id_vehiculo AS IdVehiculo, c.fecha, c.hora, 
                   c.id_tipo_servicio AS IdTipoServicio, c.estado, c.descripcion,
                   ts.nombre AS TipoServicioNombre, ts.descripcion AS TipoServicioDescripcion, ts.costo AS TipoServicioCosto
            FROM Citas c
            INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
            WHERE c.id_vehiculo = @IdVehiculo";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var citas = await connection.QueryAsync<CitaDTO>(query, new { idVehiculo });
                _logger.LogInformation("Citas obtenidas para el vehículo con ID: {IdVehiculo}", idVehiculo);
                return citas;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al obtener las citas para el vehículo con ID: {IdVehiculo}", idVehiculo);
            throw new RepositoryException("Error al obtener las citas del vehículo.", ex);
        }
    }

    public async Task ActualizarCitaAsync(int idCita, ActualizarCitaDTO citaActualizada)
    {
        _logger.LogInformation("Actualizando hora y descripción de la cita con ID: {IdCita}", idCita);

        string query = @"
        UPDATE Citas
        SET
            hora = @Hora,
            descripcion = @Descripcion
        WHERE id_cita = @IdCita";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new
                {
                    citaActualizada.Hora,
                    citaActualizada.Descripcion,
                    idCita
                });

                if (filasAfectadas == 0)
                {
                    throw new NoDataFoundException($"No se encontró ninguna cita con el ID: {idCita} para actualizar.");
                }

                _logger.LogInformation("Hora y descripción de la cita con ID: {IdCita} actualizadas exitosamente.", idCita);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al actualizar la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al actualizar la cita.", ex);
        }
    }


    public async Task EliminarCita(int idCita)
    {
        _logger.LogInformation("Eliminando cita con ID: {IdCita}", idCita);

        string query = @"
            DELETE FROM Citas WHERE id_cita = @IdCita";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new { idCita });

                if (filasAfectadas == 0)
                {
                    throw new NoDataFoundException($"No se encontró ninguna cita con el ID: {idCita} para eliminar.");
                }

                _logger.LogInformation("Cita con ID: {IdCita} eliminada exitosamente.", idCita);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error al eliminar la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al eliminar la cita.", ex);
        }
    }

    public async Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita)
    {
        _logger.LogInformation("Consultando los detalles de la cita con ID {IdCita}.", idCita);

        string query = @"
                SELECT 
                    c.id_cita AS IdCita,                  -- Identificador único de la cita
                    c.fecha AS Fecha,                    -- Fecha programada de la cita
                    c.hora AS Hora,                      -- Hora programada de la cita
                    c.estado AS Estado,                  -- Estado de la cita (pendiente, aprobado, rechazado)
                    c.descripcion AS Descripcion,        -- Descripción adicional de la cita
                    c.id_usuario AS IdUsuario,           -- Identificador del cliente asociado a la cita
                    ts.nombre AS TipoServicio,           -- Tipo de servicio solicitado
                    v.placa AS Placa,                    -- Placa del vehículo
                    m.nombre AS Marca,                   -- Marca del vehículo
                    mo.nombre AS Modelo,                 -- Modelo del vehículo
                    a.anho AS Anho                       -- Año del vehículo
                FROM Citas c
                INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
                INNER JOIN Marcas m ON v.id_marca = m.id_marca
                INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
                INNER JOIN Anhos a ON v.id_anho = a.id_anho
                INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
                WHERE c.id_cita = @IdCita
                ORDER BY c.hora";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var detalleCita = await connection.QueryFirstOrDefaultAsync<CitaDetalleDTO>(query, new { IdCita = idCita });
                return detalleCita;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la cita con ID {IdCita}.", idCita);
            throw new RepositoryException("Error al obtener los detalles de la cita.", ex);
        }
    }

    public async Task<IEnumerable<CitaDetalleDTO>> ObtenerCitasPorFechaYClienteAsync(DateTime fecha, int idUsuario)
    {
        _logger.LogInformation("Consultando las citas para el cliente {IdUsuario} en la fecha {Fecha}.", idUsuario, fecha);

        string query = @"
                SELECT 
                    c.id_cita AS IdCita,                  -- Identificador único de la cita
                    c.fecha AS Fecha,                    -- Fecha programada de la cita
                    c.hora AS Hora,                      -- Hora programada de la cita
                    c.estado AS Estado,                  -- Estado de la cita (pendiente, aprobado, rechazado)
                    c.descripcion AS Descripcion,        -- Descripción adicional de la cita
                    c.id_usuario AS IdUsuario,           -- Identificador del cliente asociado a la cita
                    ts.nombre AS TipoServicio,           -- Tipo de servicio solicitado
                    v.placa AS Placa,                    -- Placa del vehículo
                    m.nombre AS Marca,                   -- Marca del vehículo
                    mo.nombre AS Modelo,                 -- Modelo del vehículo
                    a.anho AS Anho                       -- Año del vehículo
                FROM Citas c
                INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
                INNER JOIN Marcas m ON v.id_marca = m.id_marca
                INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
                INNER JOIN Anhos a ON v.id_anho = a.id_anho
                INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
                WHERE c.fecha = @Fecha AND c.id_usuario = @IdUsuario
                ORDER BY c.hora";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var citas = await connection.QueryAsync<CitaDetalleDTO>(query, new { Fecha = fecha.Date, IdUsuario= idUsuario });
                return citas;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener citas para el cliente {IdUsuario} en la fecha {Fecha}.", idUsuario, fecha);
            throw new RepositoryException("Error al obtener las citas.", ex);
        }
    }

}
