namespace autoagenda_back.DTOs;

public class FichaTecnicaDTO
{
    public int IdFicha { get; set; }
    public int IdCita { get; set; }
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }
    public int KilometrajeIngreso { get; set; }
    public int KilometrajeProximo { get; set; }
    public required string DetallesServicio { get; set; }
    public required string MecanicoResponsable { get; set; }
    public required string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public required string PlacaVehiculo { get; set; }
    public int IdMarca { get; set; }
    public int IdModelo { get; set; }
    public int IdAnho { get; set; }
    public required string NombreCliente { get; set; }
    public required string CorreoCliente { get; set; }
    public required string TelefonoCliente { get; set; }
}
