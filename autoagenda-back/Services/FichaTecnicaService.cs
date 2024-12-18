using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Request;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class FichaTecnicaService : IFichaTecnicaService
{
    private readonly IFichaTecnicaRepository _repository;
    private readonly ILogger<FichaTecnicaService> _logger;

    public FichaTecnicaService(ILogger<FichaTecnicaService> logger, IFichaTecnicaRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<FichaTecnicaDTO> GenerarFichaTecnicaAsync(FichaTecnicaRequest request)
    {
        return await _repository.GenerarFichaTecnicaAsync(request);
    }

    public async Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha)
    {
        return await _repository.ObtenerFichaTecnicaCompletaAsync(idFicha);
    }
}
