using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces;

public interface IVehiculosRepository
{
    Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo);
    Task<VehiculoDTO> ObtenerVehiculoPorIdAsync(int idVehiculo);
    Task<IEnumerable<VehiculoDTO>> ObtenerVehiculosPorClienteAsync(int idCliente);
    Task ActualizarVehiculoAsync(int idVehiculo, VehiculoDTO vehiculo);
    Task EliminarVehiculoAsync(int idVehiculo);
    Task<IEnumerable<VehiculoDTO>> ObtenerTodosLosVehiculosAsync();
}
