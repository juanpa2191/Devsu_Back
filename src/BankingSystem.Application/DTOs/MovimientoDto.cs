using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.DTOs;

public class MovimientoDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public TipoMovimiento TipoMovimiento { get; set; }
    public decimal Valor { get; set; }
    public decimal Saldo { get; set; }
    public int CuentaId { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
