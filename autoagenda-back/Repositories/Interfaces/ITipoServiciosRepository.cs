using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces;

public interface ITipoServiciosRepository
{
    Task<IEnumerable<TipoServicioDTO>> ObtenerTodosLosTiposDeServicioAsync();
}
