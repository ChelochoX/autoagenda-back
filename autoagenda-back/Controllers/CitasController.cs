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
        Summary = "Crea una nueva cita",
        Description = "Permite registrar una nueva cita asociada a un vehículo y un tipo de servicio.")]
    public async Task<IActionResult> InsertarCitaAsync([FromBody] CitaDTO cita)
    {
        _logger.LogInformation("Solicitud para insertar una nueva cita.");

        try
        {
            var idCita = await _service.InsertarCitaAsync(cita);
            return Ok(new { mensaje = "Cita creada exitosamente.", idCita });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar insertar una cita.");
            return StatusCode(500, new { mensaje = "Ocurrió un error al intentar insertar la cita." });
        }
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

        try
        {
            await _service.EliminarCitaAsync(idCita);
            return Ok(new { mensaje = "Cita eliminada exitosamente." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar eliminar la cita con ID: {IdCita}", idCita);
            return StatusCode(500, new { mensaje = "Ocurrió un error al intentar eliminar la cita." });
        }
    }


    [HttpGet("{idCita}")]
    [SwaggerOperation(
     Summary = "Obtiene los detalles de una cita",
     Description = "Devuelve los detalles completos de una cita, incluyendo información del vehículo y tipo de servicio.")]
    public async Task<IActionResult> ObtenerDetalleCitaAsync(int idCita)
    {
        var detalleCita = await _service.ObtenerDetalleCitaAsync(idCita);
        if (detalleCita == null)
        {
            return NotFound(new { mensaje = "No se encontró la cita con el ID especificado." });
        }

        return Ok(detalleCita);
    }

    [HttpGet("buscarcita")]
    [SwaggerOperation(
    Summary = "Obtiene citas filtradas por fecha e ID del cliente",
    Description = "Devuelve las citas de un cliente para una fecha específica, incluyendo información del vehículo y tipo de servicio.")]
    public async Task<IActionResult> ObtenerCitasPorFechaYClienteAsync([FromQuery] DateTime fecha, [FromQuery] int idCliente)
    {
        var citas = await _service.ObtenerCitasPorFechaYClienteAsync(fecha, idCliente);

        if (!citas.Any())
        {
            return NotFound(new { mensaje = "No se encontraron citas para la fecha y cliente especificados." });
        }

        return Ok(citas);
    }

}
