using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Request;
using Dapper;
using System.Data;

namespace autoagenda_back.Repositories;

public class FichaTecnicaRepository : IFichaTecnicaRepository
{
    private readonly IDbConnection _conexion;
    private readonly ILogger<CitasRepository> _logger;

    public FichaTecnicaRepository(ILogger<CitasRepository> logger, IDbConnection conexion)
    {
        _logger = logger;
        _conexion = conexion;
    }

    public async Task<int> CrearFichaTecnicaAsync(FichaTecnicaRequest request)
    {
        _logger.LogInformation("Generando ficha técnica para la cita con ID: {IdCita}", request.IdCita);

        string checkQuery = @"
            SELECT 
                IdFicha 
            FROM 
                FichaTecnicaVehiculo WITH (UPDLOCK, HOLDLOCK)
            WHERE 
                IdCita = @IdCita";

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
            // Abrimos manualmente la conexión para que esté disponible durante toda la transacción
            if (_conexion.State == ConnectionState.Closed)
            {
                _conexion.Open();
            }

            // Dapper abrirá y cerrará la conexión automáticamente
            using IDbTransaction transaction = _conexion.BeginTransaction(System.Data.IsolationLevel.Serializable);

            // Verificar si ya existe la ficha técnica
            int? fichaExistenteId = await _conexion.QueryFirstOrDefaultAsync<int?>(
                checkQuery,
                new { request.IdCita },
                transaction
            );

            if (fichaExistenteId.HasValue)
            {
                _logger.LogWarning("Ya existe una ficha técnica para la cita con ID: {IdCita}.", request.IdCita);
                transaction.Rollback();
                return fichaExistenteId.Value;
            }

            // Insertar una nueva ficha técnica
            int idFicha = await _conexion.ExecuteScalarAsync<int>(
                insertQuery,
                new
                {
                    request.IdCita,
                    request.KilometrajeIngreso,
                    request.KilometrajeProximo,
                    request.DetallesServicio,
                    request.MecanicoResponsable
                },
                transaction
            );

            transaction.Commit();
            _logger.LogInformation("Ficha técnica creada con ID: {IdFicha}", idFicha);
            return idFicha;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar la ficha técnica para la cita con ID: {IdCita}", request.IdCita);
            throw new RepositoryException("Error al generar la ficha técnica.", ex);
        }
    }

    public async Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha)
    {
        _logger.LogInformation("Obteniendo detalles completos de la ficha técnica con ID: {IdFicha}.", idFicha);

        string query = @"
        SELECT 
            -- Datos de la ficha técnica
            ft.IdFicha,
            ft.IdCita,
            ft.KilometrajeIngreso,
            ft.KilometrajeProximo,
            ft.DetallesServicio,
            ft.MecanicoResponsable,
            ft.Estado,
            ft.FechaCreacion,

            -- Datos del vehículo
            v.placa AS PlacaVehiculo,
            v.id_marca AS IdMarca,
            v.id_modelo AS IdModelo,
            m.nombre AS NombreMarca,
            mo.nombre AS NombreModelo,
            a.anho AS AnhoVehiculo,

            -- Datos del cliente
            u.nombre_completo AS NombreCliente,
            u.correo AS CorreoCliente,
            u.telefono AS TelefonoCliente,

            -- Datos adicionales de la cita
            c.fecha AS FechaCita,
            c.hora AS HoraCita,
            dc.id_detallescita AS IdDetallesCita,
            dc.descripcion AS DescripcionServicio,
            dc.precio_servicio AS PrecioServicio,
            ts.nombre AS TipoServicio

        FROM [FichaTecnicaVehiculo] ft
        INNER JOIN Citas c ON ft.IdCita = c.id_cita
        INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
        INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
        INNER JOIN Marcas m ON mo.id_marca = m.id_marca
        INNER JOIN Anhos a ON v.id_anho = a.id_anho
        INNER JOIN Usuarios u ON c.id_usuario = u.id_usuario
        INNER JOIN DetallesCita dc ON c.id_cita = dc.id_cita
        INNER JOIN TipoServicio ts ON dc.id_tipo_servicio = ts.id_tipo_servicio
        WHERE ft.IdFicha = @IdFicha";

        try
        {
            IEnumerable<FichaTecnicaDTO> fichaTecnica = await _conexion.QueryAsync<FichaTecnicaDTO, DetallesServicioDTO, FichaTecnicaDTO>(
                query,
                (ficha, detalle) =>
                {
                    ficha.DetallesServicios ??= [];
                    ficha.DetallesServicios.Add(detalle);
                    return ficha;
                },
                new { IdFicha = idFicha },
                splitOn: "IdDetallesCita"
            );

            FichaTecnicaDTO? fichaAgrupada = fichaTecnica
                .GroupBy(f => f.IdFicha)
                .Select(g =>
                {
                    FichaTecnicaDTO ficha = g.First();
                    ficha.DetallesServicios = g.Select(f => f.DetallesServicios.FirstOrDefault()).ToList();
                    return ficha;
                })
                .FirstOrDefault();

            if (fichaAgrupada == null)
            {
                _logger.LogWarning("No se encontró la ficha técnica con ID: {IdFicha}.", idFicha);
                throw new KeyNotFoundException($"No se encontró la ficha técnica con ID: {idFicha}.");
            }

            return fichaAgrupada;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la ficha técnica con ID: {IdFicha}", idFicha);
            throw new RepositoryException("Error al obtener los detalles de la ficha técnica.", ex);
        }
    }


    public async Task<IEnumerable<MecanicoDTO>> ObtenerTodosLosMecanicosAsync()
    {
        _logger.LogInformation("Inicio del proceso para obtener todos los mecánicos disponibles.");

        string query = @"
            SELECT 
                IdMecanico,
                Nombre,
                Apellido,
                Especialidad,
                Telefono,
                Email,
                Estado
            FROM Mecanicos
            WHERE Estado = 1 -- Solo mecánicos activos
            ORDER BY Nombre ASC";

        try
        {
            IEnumerable<MecanicoDTO> mecanicos = await _conexion.QueryAsync<MecanicoDTO>(query);
            _logger.LogInformation("Se han obtenido {Cantidad} mecánicos.", mecanicos.Count());
            return mecanicos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener los mecánicos.");
            throw new RepositoryException("Error al intentar obtener los mecánicos.", ex);
        }
    }

    public async Task<FichaTecnicaDTO> ObtenerFichaTecnicaPorIdCitaAsync(int idCita)
    {
        _logger.LogInformation("Obteniendo detalles completos de la ficha técnica asociada a la cita con ID: {IdCita}.", idCita);

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
            v.id_marca AS IdMarca,
            v.id_modelo AS IdModelo,
            m.nombre AS NombreMarca,
            mo.nombre AS NombreModelo,
            a.anho AS AnhoVehiculo,
            u.nombre_completo AS NombreCliente,
            u.correo AS CorreoCliente,
            u.telefono AS TelefonoCliente,
            c.fecha AS FechaCita,
            c.hora AS HoraCita,
            c.estado AS EstadoCita,
            dc.id_detallescita AS IdDetallesCita,
            dc.id_tipo_servicio AS IdTipoServicio,
            ts.nombre AS TipoServicio,
            dc.descripcion AS DescripcionServicio,
            dc.precio_servicio AS PrecioServicio
        FROM [FichaTecnicaVehiculo] ft
        INNER JOIN Citas c ON ft.IdCita = c.id_cita
        INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
        INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
        INNER JOIN Marcas m ON mo.id_marca = m.id_marca
        INNER JOIN Anhos a ON v.id_anho = a.id_anho
        INNER JOIN Usuarios u ON c.id_usuario = u.id_usuario
        LEFT JOIN DetallesCita dc ON c.id_cita = dc.id_cita -- Cambiado a LEFT JOIN
        LEFT JOIN TipoServicio ts ON dc.id_tipo_servicio = ts.id_tipo_servicio
        WHERE ft.IdCita = @IdCita";

        try
        {
            // Consulta y mapeo
            IEnumerable<FichaTecnicaDTO> fichaTecnica = await _conexion.QueryAsync<FichaTecnicaDTO, DetallesServicioDTO, FichaTecnicaDTO>(
                query,
                (ficha, detalle) =>
                {
                    ficha.DetallesServicios ??= [];
                    if (detalle != null && detalle.IdDetallesCita > 0)
                    {
                        ficha.DetallesServicios.Add(detalle);
                    }
                    return ficha;
                },
                new { IdCita = idCita },
                splitOn: "IdDetallesCita"
            );

            // Agrupación de servicios asociados
            FichaTecnicaDTO? fichaAgrupada = fichaTecnica
                .GroupBy(f => f.IdFicha)
                .Select(g =>
                {
                    FichaTecnicaDTO ficha = g.First();
                    ficha.DetallesServicios = g.Select(f => f.DetallesServicios.FirstOrDefault()).Where(d => d != null).ToList();
                    return ficha;
                })
                .FirstOrDefault();

            if (fichaAgrupada == null)
            {
                _logger.LogWarning("No se encontró la ficha técnica asociada a la cita con ID: {IdCita}.", idCita);
                throw new KeyNotFoundException($"No se encontró la ficha técnica asociada a la cita con ID: {idCita}.");
            }

            return fichaAgrupada;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la ficha técnica asociada a la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al obtener los detalles de la ficha técnica.", ex);
        }
    }





}

