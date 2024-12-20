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
        _logger.LogInformation("Procesando generación de ficha técnica para la cita con ID: {IdCita}", request.IdCita);

        // Insertar la ficha técnica y obtener el ID generado o existente
        int idFicha = await _repository.CrearFichaTecnicaAsync(request);

        // Obtener los datos completos de la ficha técnica
        return await _repository.ObtenerFichaTecnicaCompletaAsync(idFicha);
    }

    public async Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha)
    {
        return await _repository.ObtenerFichaTecnicaCompletaAsync(idFicha);
    }

    public async Task<IEnumerable<MecanicoDTO>> ObtenerTodosLosMecanicosAsync()
    {
        return await _repository.ObtenerTodosLosMecanicosAsync();
    }
}
