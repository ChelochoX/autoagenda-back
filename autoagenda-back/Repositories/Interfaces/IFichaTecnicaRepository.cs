using autoagenda_back.DTOs;
using autoagenda_back.Request;

namespace autoagenda_back.Repositories.Interfaces;

public interface IFichaTecnicaRepository
{
    Task<FichaTecnicaDTO> GenerarFichaTecnicaAsync(FichaTecnicaRequest request);
    Task<FichaTecnicaDTO> ObtenerFichaTecnicaCompletaAsync(int idFicha);
}
