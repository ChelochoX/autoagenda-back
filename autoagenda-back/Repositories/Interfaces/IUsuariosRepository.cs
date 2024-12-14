using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces;

public interface IUsuariosRepository
{
    Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int idUsuario);
}
