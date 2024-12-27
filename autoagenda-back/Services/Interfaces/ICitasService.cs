using autoagenda_back.DTOs;

namespace autoagenda_back.Services.Interfaces;

public interface ICitasService
{
    Task<int> InsertarCitaConDetallesAsync(CitaConDetallesDTO citaConDetalles);
    Task<CitaDTO> ObtenerCitaPorIdAsync(int idCita);
    Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculoAsync(int idVehiculo);
    Task ActualizarCitaAsync(int idCita, ActualizarCitaDTO citaActualizada);
    Task EliminarCitaAsync(int idCita);
    Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita);
    Task<IEnumerable<CitaDetalleDTO>> ObtenerCitasPorFechaYClienteAsync(DateTime fecha, int idCliente);
    Task ActualizarEstadoCitaAsync(int idCita, string estadoCita);
}
