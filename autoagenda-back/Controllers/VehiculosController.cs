using autoagenda_back.DTOs;
using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace autoagenda_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculosService _service;

        public VehiculosController(IVehiculosService service)
        {
            _service = service;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> InsertarVehiculoAsync([FromBody] VehiculoDTO vehiculo)
        {
            var idVehiculo = await _service.InsertarVehiculoAsync(vehiculo);
            return Ok(new { mensaje = "Vehículo creado exitosamente.", idVehiculo });
        }

        [HttpGet("{idVehiculo}")]
        public async Task<IActionResult> ObtenerVehiculoPorIdAsync(int idVehiculo)
        {
            var vehiculo = await _service.ObtenerVehiculoPorIdAsync(idVehiculo);
            return vehiculo != null ? Ok(vehiculo) : NotFound(new { mensaje = "Vehículo no encontrado." });
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> ObtenerVehiculosPorClienteAsync(int idCliente)
        {
            var vehiculos = await _service.ObtenerVehiculosPorClienteAsync(idCliente);
            return Ok(vehiculos);
        }

        [HttpPut("{idVehiculo}")]
        public async Task<IActionResult> ActualizarVehiculoAsync(int idVehiculo, [FromBody] VehiculoDTO vehiculo)
        {
            await _service.ActualizarVehiculoAsync(idVehiculo, vehiculo);
            return Ok(new { mensaje = "Vehículo actualizado exitosamente." });
        }

        [HttpDelete("{idVehiculo}")]
        public async Task<IActionResult> EliminarVehiculoAsync(int idVehiculo)
        {
            await _service.EliminarVehiculoAsync(idVehiculo);
            return Ok(new { mensaje = "Vehículo eliminado exitosamente." });
        }

        [HttpGet("modelos")]
        public async Task<IActionResult> ObtenerTodosLosVehiculosAsync()
        {          
            var vehiculos = await _service.ObtenerTodosLosVehiculosAsync();             
            return Ok(vehiculos);          
        }

    }
}
