using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface ICitasService
{
    Task<int> InsertarCitaAsync(CitaDTO cita);
    Task<CitaDTO> ObtenerCitaPorIdAsync(int idCita);
    Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculoAsync(int idVehiculo);
    Task ActualizarCitaAsync(int idCita, CitaDTO cita);
    Task EliminarCitaAsync(int idCita);
    Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita);
}
