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

    public async Task<int> InsertarCitaConDetallesAsync(CitaConDetallesDTO citaConDetalles)
    {
        _logger.LogInformation("Inicio del proceso para insertar una nueva cita con detalles.");

        using (var connection = _conexion.CreateSqlConnection())
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Inserción de la cita
                    string queryCita = @"
                    INSERT INTO Citas (id_vehiculo, fecha, hora, estado, id_usuario)
                    VALUES (@IdVehiculo, @Fecha, @Hora, @Estado, @IdUsuario);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    var idCita = await connection.QuerySingleAsync<int>(queryCita, citaConDetalles.Cita, transaction);

                    // Inserción de los detalles
                    string queryDetalle = @"
                    INSERT INTO DetallesCita (id_cita, id_tipo_servicio, descripcion, precio_servicio)
                    VALUES (@IdCita, @IdTipoServicio, @Descripcion, @PrecioServicio);";
                    foreach (var detalle in citaConDetalles.DetallesCita)
                    {
                        detalle.IdCita = idCita; // Asignar el ID de la cita maestra
                        await connection.ExecuteAsync(queryDetalle, detalle, transaction);
                    }

                    transaction.Commit();
                    _logger.LogInformation("Cita con detalles insertada exitosamente con ID: {IdCita}", idCita);
                    return idCita;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Error al insertar la cita con detalles.");
                    throw new RepositoryException("Error al insertar la cita con detalles.", ex);
                }
            }
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

        string queryCitas = @"
                SELECT 
                    c.id_cita AS IdCita,
                    c.fecha AS Fecha,
                    c.hora AS Hora,
                    c.estado AS Estado,
                    c.id_usuario AS IdUsuario,
                    v.placa AS Placa,
                    m.nombre AS Marca,
                    mo.nombre AS Modelo,
                    a.anho AS Anho
                FROM Citas c
                INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
                INNER JOIN Marcas m ON v.id_marca = m.id_marca
                INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
                INNER JOIN Anhos a ON v.id_anho = a.id_anho
                WHERE c.fecha = @Fecha AND c.id_usuario = @IdUsuario
                ORDER BY c.hora";

        string queryDetalles = @"
                SELECT 
                    d.id_cita AS IdCita,
                    ts.nombre AS TipoServicio,
                    d.descripcion AS Descripcion,
                    d.precio_servicio AS PrecioServicio
                FROM DetallesCita d
                INNER JOIN TipoServicio ts ON d.id_tipo_servicio = ts.id_tipo_servicio
                WHERE d.id_cita = @IdCita";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                // Obtener las citas principales
                var citas = (await connection.QueryAsync<CitaDetalleDTO>(
                    queryCitas,
                    new { Fecha = fecha.Date, IdUsuario = idUsuario }
                )).ToList();

                // Obtener y asignar los detalles para cada cita
                foreach (var cita in citas)
                {
                    var detalles = await connection.QueryAsync<DetalleCita>(
                        queryDetalles,
                        new { IdCita = cita.IdCita }
                    );

                    cita.DetallesCita = detalles.ToList();
                }

                return citas;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener citas para el cliente {IdUsuario} en la fecha {Fecha}.", idUsuario, fecha);
            throw new RepositoryException("Error al obtener las citas.", ex);
        }
    }



    public async Task ActualizarEstadoCitaAsync(int idCita, string estadoCita)
    {
        _logger.LogInformation("Actualizando el estado de la cita con ID: {IdCita} a {Estado}", idCita, estadoCita);

        string query = @"
            UPDATE Citas
            SET
                estado = @estadoCita
            WHERE id_cita = @idCita";

        try
        {
            using (var connection = _conexion.CreateSqlConnection())
            {
                var filasAfectadas = await connection.ExecuteAsync(query, new
                {
                    estadoCita,
                    idCita
                });

                if (filasAfectadas == 0)
                {
                    throw new NoDataFoundException($"No se encontró ninguna cita con el ID: {idCita} para actualizar.");
                }

                _logger.LogInformation("El estado de la cita con ID: {IdCita} fue actualizado a {Estado}.", idCita, estadoCita);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el estado de la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al actualizar el estado de la cita.", ex);
        }
    }


}
