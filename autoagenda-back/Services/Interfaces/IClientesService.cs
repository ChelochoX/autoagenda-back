using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces
{
    public interface IClientesService
    {
        Task<ClienteDTO> BuscarClientePorCorreo(string correo);
        Task ActualizarCliente(int idCliente, ClienteDTO cliente);
        Task<int> InsertarClienteAsync(ClienteDTO cliente);
    }
}
