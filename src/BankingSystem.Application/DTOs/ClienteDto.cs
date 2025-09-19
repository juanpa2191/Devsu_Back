using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.DTOs;

public class ClienteDto
{
    public int Id { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public Genero Genero { get; set; }
    public int Edad { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
