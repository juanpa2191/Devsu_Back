using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities;

public class Cuenta : BaseEntity
{
    [Required]
    [StringLength(20)]
    public string NumeroCuenta { get; set; } = string.Empty;

    [Required]
    public TipoCuenta TipoCuenta { get; set; }

    [Required]
    public Dinero SaldoInicial { get; set; } = null!;

    [Required]
    public Dinero SaldoActual { get; set; } = null!;

    [Required]
    public bool Estado { get; set; } = true;

    [Required]
    public int ClienteId { get; set; }

    // Navegación
    public virtual Cliente Cliente { get; set; } = null!;
    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    // Constructor para Entity Framework
    protected Cuenta() { }

    public Cuenta(string numeroCuenta, TipoCuenta tipoCuenta, Dinero saldoInicial, int clienteId)
    {
        NumeroCuenta = numeroCuenta ?? throw new ArgumentNullException(nameof(numeroCuenta));
        TipoCuenta = tipoCuenta;
        SaldoInicial = saldoInicial ?? throw new ArgumentNullException(nameof(saldoInicial));
        SaldoActual = saldoInicial;
        ClienteId = clienteId;
    }

    public void Activar()
    {
        Estado = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool PuedeRealizarTransacciones()
    {
        return Estado && !IsDeleted;
    }

    public bool TieneSaldoSuficiente(Dinero monto)
    {
        return SaldoActual >= monto;
    }

    public void ActualizarSaldo(Dinero nuevoSaldo)
    {
        SaldoActual = nuevoSaldo;
        UpdatedAt = DateTime.UtcNow;
    }
}
