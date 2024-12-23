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

        _logger.LogInformation("Consultando ficha técnica con ID {IdFicha}.", idFicha);

        DTOs.FichaTecnicaDTO fichaTecnica = await _service.ObtenerFichaTecnicaCompletaAsync(idFicha);

        return fichaTecnica == null ? NotFound(new { Message = $"No se encontró la ficha técnica con ID {idFicha}." }) : Ok(fichaTecnica);

    }

    [HttpGet("mecanicos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        _logger.LogInformation("Solicitando todos los mecánicos.");

        var mecanicos = await _service.ObtenerTodosLosMecanicosAsync();
        return Ok(mecanicos);

    }

    [HttpGet("porcita/{idCita}")]
    public async Task<IActionResult> ObtenerFichaTecnicaPorIdCita(int idCita)
    {
        _logger.LogInformation("Recibida solicitud para obtener ficha técnica por ID de cita: {IdCita}", idCita);

        var fichaTecnica = await _service.ObtenerFichaTecnicaPorIdCitaAsync(idCita);
        return Ok(fichaTecnica);

    }
}

