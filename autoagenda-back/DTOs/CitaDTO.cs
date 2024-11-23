namespace autoagenda_back.DTOs;

public class CitaDTO
{
    public int IdCita { get; set; } 
    public int IdVehiculo { get; set; } 
    public DateTime Fecha { get; set; } 
    public TimeSpan Hora { get; set; } 
    public int IdTipoServicio { get; set; } 
    public string Estado { get; set; } 

}
