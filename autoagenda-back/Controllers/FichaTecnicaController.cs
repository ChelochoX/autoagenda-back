using autoagenda_back.Request;
using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace autoagenda_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FichaTecnicaController : ControllerBase
{
    private readonly IFichaTecnicaService _service;
    private readonly ILogger<FichaTecnicaController> _logger;

    public FichaTecnicaController(ILogger<FichaTecnicaController> logger, IFichaTecnicaService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("crear")]
    public async Task<IActionResult> CrearFichaTecnica([FromBody] FichaTecnicaRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { Message = "La solicitud está vacía." });
        }

        // Generar la ficha técnica y obtener el DTO completo
        DTOs.FichaTecnicaDTO fichaTecnica = await _service.GenerarFichaTecnicaAsync(request);

        // Retornar directamente el DTO completo con HTTP 201 (Created)
        return Created("", fichaTecnica);
    }

    [HttpGet("{idFicha}")]
    public async Task<IActionResult> ObtenerFichaTecnica(int idFicha)
    {
        try
        {
            _logger.LogInformation("Consultando ficha técnica con ID {IdFicha}.", idFicha);

            DTOs.FichaTecnicaDTO fichaTecnica = await _service.ObtenerFichaTecnicaCompletaAsync(idFicha);

            return fichaTecnica == null ? NotFound(new { Message = $"No se encontró la ficha técnica con ID {idFicha}." }) : Ok(fichaTecnica);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar la ficha técnica con ID {IdFicha}.", idFicha);
            return StatusCode(500, new { Message = "Error interno del servidor." });
        }
    }
}

