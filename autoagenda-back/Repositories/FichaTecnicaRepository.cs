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
            using System.Data.IDbConnection connection = _conexion.CreateSqlConnection();
            connection.Open();
            using System.Data.IDbTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            // Verificar si ya existe la ficha técnica
            int? fichaExistenteId = await connection.QueryFirstOrDefaultAsync<int?>(
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
            int idFicha = await connection.ExecuteScalarAsync<int>(
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
                    ts.nombre AS TipoServicio,
                    ts.descripcion AS DescripcionServicio,
                    ts.costo AS CostoServicio

                FROM [FichaTecnicaVehiculo] ft
                INNER JOIN Citas c ON ft.IdCita = c.id_cita
                INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
                INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
                INNER JOIN Marcas m ON mo.id_marca = m.id_marca
                INNER JOIN Anhos a ON v.id_anho = a.id_anho -- Relación con el año del vehículo
                INNER JOIN Usuarios u ON c.id_usuario = u.id_usuario
                INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
                WHERE ft.IdFicha = @IdFicha";

        try
        {
            using System.Data.IDbConnection connection = _conexion.CreateSqlConnection();
            FichaTecnicaDTO? fichaTecnica = await connection.QueryFirstOrDefaultAsync<FichaTecnicaDTO>(query, new { IdFicha = idFicha });

            if (fichaTecnica == null)
            {
                _logger.LogWarning("No se encontró la ficha técnica con ID: {IdFicha}.", idFicha);
                throw new KeyNotFoundException($"No se encontró la ficha técnica con ID: {idFicha}.");
            }

            return fichaTecnica;
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
            using (var connection = _conexion.CreateSqlConnection())
            {
                var mecanicos = await connection.QueryAsync<MecanicoDTO>(query);
                _logger.LogInformation("Se han obtenido {Cantidad} mecánicos.", mecanicos.Count());
                return mecanicos;
            }
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
             ts.nombre AS TipoServicio,
             ts.descripcion AS DescripcionServicio,
             ts.costo AS CostoServicio

         FROM [FichaTecnicaVehiculo] ft
         INNER JOIN Citas c ON ft.IdCita = c.id_cita
         INNER JOIN Vehiculos v ON c.id_vehiculo = v.id_vehiculo
         INNER JOIN Modelos mo ON v.id_modelo = mo.id_modelo
         INNER JOIN Marcas m ON mo.id_marca = m.id_marca
         INNER JOIN Anhos a ON v.id_anho = a.id_anho -- Relación con el año del vehículo
         INNER JOIN Usuarios u ON c.id_usuario = u.id_usuario
         INNER JOIN TipoServicio ts ON c.id_tipo_servicio = ts.id_tipo_servicio
         WHERE ft.IdCita = @IdCita";

        try
        {
            using System.Data.IDbConnection connection = _conexion.CreateSqlConnection();
            FichaTecnicaDTO? fichaTecnica = await connection.QueryFirstOrDefaultAsync<FichaTecnicaDTO>(query, new { IdCita = idCita });

            if (fichaTecnica == null)
            {
                _logger.LogWarning("No se encontró la ficha técnica asociada a la cita con ID: {IdCita}.", idCita);
                throw new KeyNotFoundException($"No se encontró la ficha técnica asociada a la cita con ID: {idCita}.");
            }

            return fichaTecnica;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la ficha técnica asociada a la cita con ID: {IdCita}", idCita);
            throw new RepositoryException("Error al obtener los detalles de la ficha técnica.", ex);
        }
    }

}

