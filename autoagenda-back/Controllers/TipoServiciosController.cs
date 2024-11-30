using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace autoagenda_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoServiciosController : ControllerBase
{
    private readonly ITipoServiciosService _service;

    public TipoServiciosController(ITipoServiciosService service)
    {
        _service = service;
    }

    [HttpGet("tiposservicios")]
    public async Task<IActionResult> ObtenerTodosLosTiposDeServicioAsync()
    {        
        var tiposDeServicio = await _service.ObtenerTodosLosTiposDeServicioAsync();
        return Ok(tiposDeServicio);      
    }
}
