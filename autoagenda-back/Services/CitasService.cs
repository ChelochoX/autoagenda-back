﻿using autoagenda_back.DTOs;
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

    public async Task<int> InsertarCitaAsync(CitaDTO cita)
    {     
        var idCita = await _repository.InsertarCita(cita);
      
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

    public async Task ActualizarCitaAsync(int idCita, CitaDTO cita)
    {        
        await _repository.ActualizarCita(idCita, cita);       
    }

    public async Task EliminarCitaAsync(int idCita)
    {     
        await _repository.EliminarCita(idCita);      
    }

}