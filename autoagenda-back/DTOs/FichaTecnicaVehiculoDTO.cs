namespace autoagenda_back.DTOs;

public class FichaTecnicaVehiculoDTO
{
    public int IdFicha { get; set; }
    public int KilometrajeIngreso { get; set; }
    public int KilometrajeProximo { get; set; }
    public required string DetallesServicio { get; set; }
    public required string MecanicoResponsable { get; set; }
    public required string Estado { get; set; }
    public DateTime FechaCierre { get; set; }
}
