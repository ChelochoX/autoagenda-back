using autoagenda_back.DTOs;
using autoagenda_back.Request;

namespace autoagenda_back.Services.Interfaces;

public interface IFichaTecnicaService
{
    Task<FichaTecnicaDTO> GenerarFichaTecnicaAsync(FichaTecnicaRequest request);
    Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha);
    Task<IEnumerable<MecanicoDTO>> ObtenerTodosLosMecanicosAsync();
}
