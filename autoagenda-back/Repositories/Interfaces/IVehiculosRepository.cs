using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces;

public interface IVehiculosRepository
{
    Task<IEnumerable<AnhoDTO>> ObtenerAnhosAsync();
    Task<IEnumerable<MarcaDTO>> ObtenerMarcasAsync();
    Task<IEnumerable<ModeloDTO>> ObtenerModelosPorMarcaAsync(int idMarca);
    Task<int> InsertarVehiculoAsync(VehiculoDTO vehiculo);   
}
