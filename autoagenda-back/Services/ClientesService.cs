using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Request;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class ClientesService : IClientesService
{
    private readonly ILogger<ClientesService> _logger;
    private readonly IClientesRepository _repository;

    public ClientesService(IClientesRepository repository, ILogger<ClientesService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ClienteDTO> BuscarClientePorCorreo(string correo)
    {          
         return await _repository.BuscarClientePorCorreo(correo);              
    }

    public async Task ActualizarCliente(int idCliente, ClienteDTO cliente)
    {        
        await _repository.ActualizarCliente(idCliente, cliente);        
    }

    public async Task<int> InsertarClienteAsync(ClienteRequest cliente)
    {
        return await _repository.InsertarCliente(cliente);         
      
    }

}
