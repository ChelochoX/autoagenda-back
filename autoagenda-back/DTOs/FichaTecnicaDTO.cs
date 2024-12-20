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

    // Datos del vehículo
    public required string PlacaVehiculo { get; set; }
    public int IdMarca { get; set; }
    public int IdModelo { get; set; }
    public required string NombreMarca { get; set; }
    public required string NombreModelo { get; set; }
    public required string AnhoVehiculo { get; set; } // Año del vehículo

    // Datos del cliente
    public required string NombreCliente { get; set; }
    public required string CorreoCliente { get; set; }
    public required string TelefonoCliente { get; set; }

    // Datos adicionales de la cita
    public required DateTime FechaCita { get; set; }
    public required string HoraCita { get; set; }
    public required string TipoServicio { get; set; }
    public required string DescripcionServicio { get; set; }
    public decimal CostoServicio { get; set; }
}
