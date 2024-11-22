using autoagenda_back.DTOs;
using autoagenda_back.Request;

namespace autoagenda_back.Repositories.Interfaces;

public interface IClientesRepository
{
    Task<ClienteDTO> BuscarClientePorCorreo(string correo);
    Task ActualizarCliente(int idCliente, ClienteDTO cliente);
    Task<int> InsertarCliente(ClienteRequest cliente);
}
