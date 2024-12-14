using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class UsuarioService : IUsuariosService
{
    private readonly IUsuariosRepository _repository;
    private readonly ILogger<CitasService> _logger;

    public UsuarioService(ILogger<CitasService> logger, IUsuariosRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int idUsuario)
    {
        return await _repository.ObtenerUsuarioPorIdAsync(idUsuario);
    }
}
