using autoagenda_back.DTOs;

namespace autoagenda_back.Repositories.Interfaces
{
    public interface ICitasRepository
    {        
        Task<int> InsertarCita(CitaDTO cita);
        
        Task<CitaDTO> ObtenerCitaPorId(int idCita);
       
        Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculo(int idVehiculo);
       
        Task ActualizarCita(int idCita, CitaDTO cita);
        
        Task EliminarCita(int idCita);

        Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita);
    }
}
