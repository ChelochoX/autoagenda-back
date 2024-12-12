namespace autoagenda_back.DTOs;

public class CitaDTO
{
    //public int IdCita { get; set; } 
    public int IdVehiculo { get; set; } 
    public DateTime Fecha { get; set; } 
    public string Hora { get; set; }    
    public string Estado { get; set; }
    public int IdTipoServicio { get; set; }
    public string Descripcion { get; set; }
    public int IdUsuario { get; set; }

}
