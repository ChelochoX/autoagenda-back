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

    public async Task<IEnumerable<AnhoDTO>> ObtenerAnhosAsync()
    {
        return await _repository.ObtenerAnhosAsync();
    }

    public async Task<IEnumerable<MarcaDTO>> ObtenerMarcasAsync()
    {
        return await _repository.ObtenerMarcasAsync();
    }

    public async Task<IEnumerable<ModeloDTO>> ObtenerModelosPorMarcaAsync(int idMarca)
    {
        return await _repository.ObtenerModelosPorMarcaAsync(idMarca);
    }
}
