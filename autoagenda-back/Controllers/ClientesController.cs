using autoagenda_back.DTOs;
using autoagenda_back.Exceptions;
using autoagenda_back.Request;
using autoagenda_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace autoagenda_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClientesService _service;

    public ClientesController(IClientesService service)
    {
        _service = service;
    }


    [HttpGet("cliente/buscar/{correo}")]
    [SwaggerOperation(
     Summary = "Busca un cliente por correo",
     Description = "Permite buscar un cliente utilizando su correo electrónico. Si no se encuentra, devuelve un mensaje indicando que no existe.")]
    public async Task<IActionResult> BuscarClientePorCorreoAsync([FromRoute] string correo)
    {       
        var cliente = await _service.BuscarClientePorCorreo(correo);                   
        return Ok(cliente);      
    }

    [HttpPut("cliente/actualizar/{idCliente}")]
    [SwaggerOperation(
    Summary = "Actualiza un cliente existente",
    Description = "Permite actualizar la información de un cliente existente utilizando su ID.")]
    public async Task<IActionResult> ActualizarClienteAsync([FromRoute] int idCliente, [FromBody] ClienteDTO cliente)
    {        
        await _service.ActualizarCliente(idCliente, cliente);
        return Ok(new { mensaje = "Cliente actualizado exitosamente." });        
    }


    [HttpPost("cliente/insertar")]
    [SwaggerOperation(
    Summary = "Registra un nuevo cliente",
    Description = "Permite registrar un nuevo cliente en el sistema.")]
    public async Task<IActionResult> InsertarClienteAsync([FromBody] ClienteRequest cliente)
    {    
        var idCliente = await _service.InsertarClienteAsync(cliente);

        return Ok(new
        {
            mensaje = "Cliente insertado exitosamente.",
            idCliente
        });     
    }

}
