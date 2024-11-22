namespace autoagenda_back.DTOs;

public class ClienteDTO
{
    public int IdCliente { get; set; }
    public string? NombreCompleto { get; set; }
    public string Correo { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
