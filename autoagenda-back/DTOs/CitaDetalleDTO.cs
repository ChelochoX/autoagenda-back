namespace autoagenda_back.DTOs;

public class CitaDetalleDTO
{
    public int IdCita { get; set; }
    public DateTime Fecha { get; set; }
    public string Hora { get; set; }
    public string Estado { get; set; }
    public int IdUsuario { get; set; }
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Anho { get; set; }
    public List<DetalleCita> DetallesCita { get; set; }
}
public class DetalleCita
{
    public string TipoServicio { get; set; }  // Nombre del servicio
    public string Descripcion { get; set; }  // Descripción opcional del servicio
    public decimal PrecioServicio { get; set; } // Precio del servicio
}
