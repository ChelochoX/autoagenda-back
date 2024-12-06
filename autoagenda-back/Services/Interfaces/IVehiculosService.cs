using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface IVehiculosService
{
    Task<IEnumerable<AnhoDTO>> ObtenerAnhosAsync();
    Task<IEnumerable<MarcaDTO>> ObtenerMarcasAsync();
    Task<IEnumerable<ModeloDTO>> ObtenerModelosPorMarcaAsync(int idMarca);
    Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo);  

}
