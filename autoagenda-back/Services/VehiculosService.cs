using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class VehiculosService : IVehiculosService
{
    private readonly IVehiculosRepository _repository;

    public VehiculosService(IVehiculosRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo) => await _repository.InsertarVehiculoAsync(vehiculo);

    public async Task<VehiculoDTO> ObtenerVehiculoPorIdAsync(int idVehiculo) => await _repository.ObtenerVehiculoPorIdAsync(idVehiculo);

    public async Task<IEnumerable<VehiculoDTO>> ObtenerVehiculosPorClienteAsync(int idCliente) => await _repository.ObtenerVehiculosPorClienteAsync(idCliente);

    public async Task ActualizarVehiculoAsync(int idVehiculo, VehiculoDTO vehiculo) => await _repository.ActualizarVehiculoAsync(idVehiculo, vehiculo);

    public async Task EliminarVehiculoAsync(int idVehiculo) => await _repository.EliminarVehiculoAsync(idVehiculo);

    public async Task<IEnumerable<VehiculoDTO>> ObtenerTodosLosVehiculosAsync() => await _repository.ObtenerTodosLosVehiculosAsync();

}
