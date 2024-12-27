using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces;

public interface ICitasRepository
{
    Task<int> InsertarCitaConDetallesAsync(CitaConDetallesDTO citaConDetalles);

    Task<CitaDTO> ObtenerCitaPorId(int idCita);

    Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculo(int idVehiculo);

    Task ActualizarCitaAsync(int idCita, ActualizarCitaDTO citaActualizada);


    Task EliminarCita(int idCita);

    Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita);

    Task<IEnumerable<CitaDetalleDTO>> ObtenerCitasPorFechaYClienteAsync(DateTime fecha, int idCliente);

    Task ActualizarEstadoCitaAsync(int idCita, string estadoCita);
}
