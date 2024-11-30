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

        [HttpGet("anhos")]
        public async Task<IActionResult> ObtenerAnhosAsync()
        {           
            var anhos = await _service.ObtenerAnhosAsync();
            return Ok(anhos);           
        }

        // Endpoint para obtener marcas
        [HttpGet("marcas")]
        public async Task<IActionResult> ObtenerMarcasAsync()
        {           
            var marcas = await _service.ObtenerMarcasAsync();
            return Ok(marcas);           
        }

        // Endpoint para obtener modelos por marca
        [HttpGet("modelos/{idMarca}")]
        public async Task<IActionResult> ObtenerModelosPorMarcaAsync(int idMarca)
        {           
            var modelos = await _service.ObtenerModelosPorMarcaAsync(idMarca);
            return Ok(modelos);           
        }

    }
}
