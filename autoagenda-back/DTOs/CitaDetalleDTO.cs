namespace autoagenda_back.DTOs;

public class CitaDetalleDTO
{
    public int IdCita { get; set; }
    public DateTime Fecha { get; set; }
    public string Hora { get; set; }
    public string Estado { get; set; }
    public string Descripcion { get; set; }
    public int IdUsuario { get; set; }
    public string TipoServicio { get; set; }
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Anho { get; set; }
}
