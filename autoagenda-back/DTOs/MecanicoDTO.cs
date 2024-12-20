namespace autoagenda_back.DTOs;

public class MecanicoDTO
{
    public int IdMecanico { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Especialidad { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public bool Estado { get; set; }
}
