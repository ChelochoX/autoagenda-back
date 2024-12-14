using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface IUsuariosService
{
    Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int idUsuario);
}
