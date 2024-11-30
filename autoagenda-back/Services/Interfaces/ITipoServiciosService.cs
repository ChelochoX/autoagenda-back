using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface ITipoServiciosService
{
    Task<IEnumerable<TipoServicioDTO>> ObtenerTodosLosTiposDeServicioAsync();
}
