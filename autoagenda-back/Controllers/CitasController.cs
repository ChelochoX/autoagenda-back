using autoagenda_back.DTOs;
using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace autoagenda_back.Controllers
{
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

        /// <summary>
        /// Inserta una nueva cita.
        /// </summary>
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

        /// <summary>
        /// Obtiene una cita por su ID.
        /// </summary>
        [HttpGet("{idCita}")]
        [SwaggerOperation(
            Summary = "Obtiene una cita por ID",
            Description = "Permite buscar una cita específica utilizando su identificador único.")]
        public async Task<IActionResult> ObtenerCitaPorIdAsync([FromRoute] int idCita)
        {
            _logger.LogInformation("Solicitud para obtener una cita con ID: {IdCita}", idCita);

            try
            {
                var cita = await _service.ObtenerCitaPorIdAsync(idCita);

                if (cita == null)
                    return NotFound(new { mensaje = "No se encontró ninguna cita con el ID especificado." });

                return Ok(cita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar obtener la cita con ID: {IdCita}", idCita);
                return StatusCode(500, new { mensaje = "Ocurrió un error al intentar obtener la cita." });
            }
        }

        /// <summary>
        /// Obtiene todas las citas de un vehículo.
        /// </summary>
        [HttpGet("vehiculo/{idVehiculo}")]
        [SwaggerOperation(
            Summary = "Obtiene todas las citas de un vehículo",
            Description = "Permite buscar todas las citas asociadas a un vehículo específico.")]
        public async Task<IActionResult> ObtenerCitasPorVehiculoAsync([FromRoute] int idVehiculo)
        {
            _logger.LogInformation("Solicitud para obtener citas del vehículo con ID: {IdVehiculo}", idVehiculo);

            try
            {
                var citas = await _service.ObtenerCitasPorVehiculoAsync(idVehiculo);

                if (citas == null || !citas.Any())
                    return NotFound(new { mensaje = "No se encontraron citas para el vehículo especificado." });

                return Ok(citas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar obtener las citas del vehículo con ID: {IdVehiculo}", idVehiculo);
                return StatusCode(500, new { mensaje = "Ocurrió un error al intentar obtener las citas." });
            }
        }

        /// <summary>
        /// Actualiza los datos de una cita existente.
        /// </summary>
        [HttpPut("{idCita}")]
        [SwaggerOperation(
            Summary = "Actualiza una cita",
            Description = "Permite actualizar los datos de una cita existente utilizando su ID.")]
        public async Task<IActionResult> ActualizarCitaAsync([FromRoute] int idCita, [FromBody] CitaDTO cita)
        {
            _logger.LogInformation("Solicitud para actualizar la cita con ID: {IdCita}", idCita);

            try
            {
                await _service.ActualizarCitaAsync(idCita, cita);
                return Ok(new { mensaje = "Cita actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar actualizar la cita con ID: {IdCita}", idCita);
                return StatusCode(500, new { mensaje = "Ocurrió un error al intentar actualizar la cita." });
            }
        }

        /// <summary>
        /// Elimina una cita por su ID.
        /// </summary>
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
    }
}
