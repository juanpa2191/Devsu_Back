using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Services;

public class TransaccionService : ITransaccionService
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly Dinero _limiteDiario = Dinero.MaximoRetiroDiario;

    public TransaccionService(IMovimientoRepository movimientoRepository)
    {
        _movimientoRepository = movimientoRepository ?? throw new ArgumentNullException(nameof(movimientoRepository));
    }

    public async Task<Movimiento> RealizarMovimientoAsync(Cuenta cuenta, TipoMovimiento tipoMovimiento, Dinero monto)
    {
        if (cuenta == null)
            throw new ArgumentNullException(nameof(cuenta));

        if (monto == null)
            throw new ArgumentNullException(nameof(monto));

        if (!cuenta.PuedeRealizarTransacciones())
            throw new InvalidOperationException("La cuenta no está activa");

        // Validar límite diario para débitos
        if (tipoMovimiento.EsDebito())
        {
            if (!await ValidarLimiteDiarioAsync(cuenta, monto, DateTime.UtcNow))
                throw new CupoDiarioExcedidoException();
        }

        // Calcular nuevo saldo
        var nuevoSaldo = await CalcularSaldoDisponibleAsync(cuenta, monto, tipoMovimiento);

        // Validar saldo suficiente para débitos
        if (tipoMovimiento.EsDebito() && nuevoSaldo < Dinero.Zero)
            throw new SaldoInsuficienteException();

        // Crear movimiento
        var movimiento = new Movimiento(tipoMovimiento, monto, nuevoSaldo, cuenta.Id);

        // Actualizar saldo de la cuenta
        cuenta.ActualizarSaldo(nuevoSaldo);

        return movimiento;
    }

    public async Task<bool> ValidarLimiteDiarioAsync(Cuenta cuenta, Dinero monto, DateTime fecha)
    {
        if (cuenta == null)
            throw new ArgumentNullException(nameof(cuenta));

        if (monto == null)
            throw new ArgumentNullException(nameof(monto));

        var totalRetirosDelDia = await _movimientoRepository.GetTotalRetirosDelDiaAsync(cuenta.Id, fecha);
        var totalRetirosConNuevoMovimiento = totalRetirosDelDia + monto.Monto;

        return totalRetirosConNuevoMovimiento <= _limiteDiario.Monto;
    }

    public async Task<Dinero> CalcularSaldoDisponibleAsync(Cuenta cuenta, Dinero monto, TipoMovimiento tipoMovimiento)
    {
        if (cuenta == null)
            throw new ArgumentNullException(nameof(cuenta));

        if (monto == null)
            throw new ArgumentNullException(nameof(monto));

        if (tipoMovimiento.EsCredito())
        {
            return cuenta.SaldoActual + monto;
        }
        else
        {
            if (cuenta.SaldoActual.Monto < monto.Monto)
            {
                throw new SaldoInsuficienteException("Saldo no disponible");
            }
            return cuenta.SaldoActual - monto;
        }
    }
}
