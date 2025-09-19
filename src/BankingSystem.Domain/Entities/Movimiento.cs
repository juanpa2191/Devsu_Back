using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities;

public class Movimiento : BaseEntity
{
    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public TipoMovimiento TipoMovimiento { get; set; }

    [Required]
    public Dinero Valor { get; set; } = null!;

    [Required]
    public Dinero Saldo { get; set; } = null!;

    [Required]
    public int CuentaId { get; set; }

    // Navegación
    public virtual Cuenta Cuenta { get; set; } = null!;

    // Constructor para Entity Framework
    protected Movimiento() { }

    public Movimiento(TipoMovimiento tipoMovimiento, Dinero valor, Dinero saldo, int cuentaId)
    {
        Fecha = DateTime.UtcNow;
        TipoMovimiento = tipoMovimiento;
        Valor = valor ?? throw new ArgumentNullException(nameof(valor));
        Saldo = saldo ?? throw new ArgumentNullException(nameof(saldo));
        CuentaId = cuentaId;
    }

    public bool EsCredito() => TipoMovimiento == TipoMovimiento.Credito;
    public bool EsDebito() => TipoMovimiento == TipoMovimiento.Debito;

    public string ObtenerDescripcion()
    {
        return EsCredito() ? $"Depósito de {Valor}" : $"Retiro de {Valor}";
    }
}
