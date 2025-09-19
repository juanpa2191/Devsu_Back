using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Domain.Interfaces;

public interface IMovimientoRepository : IRepository<Movimiento>
{
    Task<IEnumerable<Movimiento>> GetByCuentaIdAsync(int cuentaId);
    Task<IEnumerable<Movimiento>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<Movimiento>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Movimiento>> GetByTipoAsync(TipoMovimiento tipoMovimiento);
    Task<decimal> GetTotalRetirosDelDiaAsync(int cuentaId, DateTime fecha);
    Task<IEnumerable<Movimiento>> GetMovimientosDelDiaAsync(int cuentaId, DateTime fecha);
}
