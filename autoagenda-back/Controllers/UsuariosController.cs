using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace autoagenda_back.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly IUsuariosService _service;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuariosService service)
    {
        _service = service;
    }

    [HttpGet("{idUsuario}")]
    [SwaggerOperation(
    Summary = "Obtiene los detalles de un usuario",
    Description = "Devuelve los detalles completos de un usuario")]
    public async Task<IActionResult> ObtenerDetalleUsuarioAsync(int idUsuario)
    {
        DTOs.UsuarioDTO detalleUsuario = await _service.ObtenerUsuarioPorIdAsync(idUsuario);

        return detalleUsuario == null ? NoContent() : Ok(detalleUsuario);
    }

}
