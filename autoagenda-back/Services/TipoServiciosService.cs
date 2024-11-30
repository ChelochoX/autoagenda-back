using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class TipoServiciosService : ITipoServiciosService
{
    private readonly ITipoServiciosRepository _repository;
    private readonly ILogger<TipoServiciosService> _logger;

    public TipoServiciosService(ILogger<TipoServiciosService> logger, ITipoServiciosRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IEnumerable<TipoServicioDTO>> ObtenerTodosLosTiposDeServicioAsync()
    {
        return await _repository.ObtenerTodosLosTiposDeServicioAsync();
    }
}
