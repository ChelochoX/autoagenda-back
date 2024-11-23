using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface IVehiculosService
{   
    Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo);
       
    Task<VehiculoDTO> ObtenerVehiculoPorIdAsync(int idVehiculo);
       
    Task<IEnumerable<VehiculoDTO>> ObtenerVehiculosPorClienteAsync(int idCliente);
       
    Task ActualizarVehiculoAsync(int idVehiculo, VehiculoDTO vehiculo);
       
    Task EliminarVehiculoAsync(int idVehiculo);
}
