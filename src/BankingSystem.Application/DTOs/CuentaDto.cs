using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.DTOs;

public class CuentaDto
{
    public int Id { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public TipoCuenta TipoCuenta { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal SaldoActual { get; set; }
    public bool Estado { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
