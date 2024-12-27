namespace autoagenda_back.DTOs;

public class DetalleCitaDTO
{
    public int IdCita { get; set; }
    public int IdTipoServicio { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioServicio { get; set; }
}
