namespace autoagenda_back.DTOs;

public class CitaDTO
{
    //public int IdCita { get; set; } 
    public int IdVehiculo { get; set; }
    public DateTime Fecha { get; set; }
    public required string Hora { get; set; }
    public required string Estado { get; set; }
    public int IdUsuario { get; set; }

}
