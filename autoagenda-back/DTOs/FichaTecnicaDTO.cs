namespace autoagenda_back.DTOs;

public class FichaTecnicaDTO
{
    public int IdFicha { get; set; }
    public int IdCita { get; set; }
    public int KilometrajeIngreso { get; set; }
    public int KilometrajeProximo { get; set; }
    public required string DetallesServicio { get; set; }
    public required string MecanicoResponsable { get; set; }
    public required string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public required string PlacaVehiculo { get; set; }
    public required string NombreMarca { get; set; }
    public required string NombreModelo { get; set; }
    public int AnhoVehiculo { get; set; }
    public required string NombreCliente { get; set; }
    public required string CorreoCliente { get; set; }
    public required string TelefonoCliente { get; set; }
    public DateTime FechaCita { get; set; }
    public required string HoraCita { get; set; }
    public required string EstadoCita { get; set; }
    public required List<DetallesServicioDTO> DetallesServicios { get; set; }
}

public class DetallesServicioDTO
{
    public int IdDetallesCita { get; set; }
    public int IdTipoServicio { get; set; }
    public required string TipoServicio { get; set; }
    public required string DescripcionServicio { get; set; }
    public decimal PrecioServicio { get; set; }
}

