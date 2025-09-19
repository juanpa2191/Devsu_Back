using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Domain.Services;

public interface ITransaccionService
{
    Task<Movimiento> RealizarMovimientoAsync(Cuenta cuenta, TipoMovimiento tipoMovimiento, Dinero monto);
    Task<bool> ValidarLimiteDiarioAsync(Cuenta cuenta, Dinero monto, DateTime fecha);
    Task<Dinero> CalcularSaldoDisponibleAsync(Cuenta cuenta, Dinero monto, TipoMovimiento tipoMovimiento);
}
