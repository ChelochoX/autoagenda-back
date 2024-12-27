using autoagenda_back.DTOs;
using autoagenda_back.Repositories.Interfaces;
using autoagenda_back.Services.Interfaces;

namespace autoagenda_back.Services;

public class CitasService : ICitasService
{
    private readonly ICitasRepository _repository;
    private readonly ILogger<CitasService> _logger;

    public CitasService(ICitasRepository repository, ILogger<CitasService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> InsertarCitaConDetallesAsync(CitaConDetallesDTO citaConDetalles)
    {
        var idCita = await _repository.InsertarCitaConDetallesAsync(citaConDetalles);

        return idCita;
    }

    public async Task<CitaDTO> ObtenerCitaPorIdAsync(int idCita)
    {
        var cita = await _repository.ObtenerCitaPorId(idCita);

        return cita;
    }

    public async Task<IEnumerable<CitaDTO>> ObtenerCitasPorVehiculoAsync(int idVehiculo)
    {
        var citas = await _repository.ObtenerCitasPorVehiculo(idVehiculo);

        return citas;
    }

    public async Task ActualizarCitaAsync(int idCita, ActualizarCitaDTO citaActualizada)
    {
        await _repository.ActualizarCitaAsync(idCita, citaActualizada);
    }

    public async Task EliminarCitaAsync(int idCita)
    {
        await _repository.EliminarCita(idCita);
    }

    public async Task<CitaDetalleDTO> ObtenerDetalleCitaAsync(int idCita)
    {
        return await _repository.ObtenerDetalleCitaAsync(idCita);
    }

    public async Task<IEnumerable<CitaDetalleDTO>> ObtenerCitasPorFechaYClienteAsync(DateTime fecha, int idusuario)
    {
        return await _repository.ObtenerCitasPorFechaYClienteAsync(fecha, idusuario);
    }

    public async Task ActualizarEstadoCitaAsync(int idCita, string estadoCita)
    {
        await _repository.ActualizarEstadoCitaAsync(idCita, estadoCita);
    }
}
