namespace autoagenda_back.DTOs;

public class CitaConDetallesDTO
{
    public CitaDTO Cita { get; set; } // Datos de la cita principal
    public List<DetalleCitaDTO> DetallesCita { get; set; } // Lista de los servicios relacionados
}
