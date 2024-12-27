using autoagenda_back.DTOs;
using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace autoagenda_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ICitasService _service;
    private readonly ILogger<CitasController> _logger;

    public CitasController(ICitasService service, ILogger<CitasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("crear")]
    [SwaggerOperation(
        Summary = "Crea una nueva cita con detalles",
        Description = "Permite registrar una nueva cita asociada a un vehículo, incluyendo múltiples servicios.")]
    public async Task<IActionResult> InsertarCitaConDetallesAsync([FromBody] CitaConDetallesDTO citaConDetalles)
    {
        _logger.LogInformation("Solicitud para insertar una nueva cita con detalles.");

        int idCita = await _service.InsertarCitaConDetallesAsync(citaConDetalles);
        return Ok(new { mensaje = "Cita creada exitosamente con sus detalles.", idCita });
    }

    [HttpPut("{idCita}")]
    [SwaggerOperation(
     Summary = "Actualiza solo la hora y descripción de una cita",
     Description = "Permite actualizar la hora y la descripción de una cita existente utilizando su ID.")]
    public async Task<IActionResult> ActualizarCitaAsync([FromRoute] int idCita, [FromBody] ActualizarCitaDTO citaActualizada)
    {
        await _service.ActualizarCitaAsync(idCita, citaActualizada);
        return Ok(new { mensaje = "Cita actualizada exitosamente." });
    }

    [HttpDelete("{idCita}")]
    [SwaggerOperation(
        Summary = "Elimina una cita",
        Description = "Permite eliminar una cita específica utilizando su identificador único.")]
    public async Task<IActionResult> EliminarCitaAsync([FromRoute] int idCita)
    {
        _logger.LogInformation("Solicitud para eliminar la cita con ID: {IdCita}", idCita);

        await _service.EliminarCitaAsync(idCita);
        return Ok(new { mensaje = "Cita eliminada exitosamente." });

    }


    [HttpGet("{idCita}")]
    [SwaggerOperation(
     Summary = "Obtiene los detalles de una cita",
     Description = "Devuelve los detalles completos de una cita, incluyendo información del vehículo y tipo de servicio.")]
    public async Task<IActionResult> ObtenerDetalleCitaAsync(int idCita)
    {
        CitaDetalleDTO detalleCita = await _service.ObtenerDetalleCitaAsync(idCita);
        return detalleCita == null ? NoContent() : Ok(detalleCita);
    }

    [HttpGet("buscarcita")]
    [SwaggerOperation(
    Summary = "Obtiene citas filtradas por fecha e ID del cliente",
    Description = "Devuelve las citas de un cliente para una fecha específica, incluyendo información del vehículo y tipo de servicio.")]
    public async Task<IActionResult> ObtenerCitasPorFechaYClienteAsync([FromQuery] DateTime fecha, [FromQuery] int idUsuario)
    {
        var citas = await _service.ObtenerCitasPorFechaYClienteAsync(fecha, idUsuario);

        if (citas == null || !citas.Any())
        {
            return NoContent(); // Retorna 204 si no hay citas
        }

        return Ok(citas);
    }


    [HttpPut("{idCita}/estado")]
    public async Task<IActionResult> ActualizarEstadoCita(int idCita, [FromBody] string estadoDTO)
    {
        _logger.LogInformation("Iniciando actualización de estado de la cita con ID: {IdCita}", idCita);

        if (string.IsNullOrWhiteSpace(estadoDTO) ||
            (estadoDTO != "aprobado" && estadoDTO != "rechazado"))
        {
            return BadRequest("El estado debe ser 'aprobado' o 'rechazado'.");
        }

        await _service.ActualizarEstadoCitaAsync(idCita, estadoDTO);
        return Ok(new { Message = "Estado de la cita actualizado correctamente." });

    }

}
