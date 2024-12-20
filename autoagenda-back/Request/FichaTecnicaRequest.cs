namespace autoagenda_back.Request;

public class FichaTecnicaRequest
{
    public int IdCita { get; set; }
    public int KilometrajeIngreso { get; set; }
    public int KilometrajeProximo { get; set; }
    public string? DetallesServicio { get; set; }
    public string? MecanicoResponsable { get; set; }
}
