using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Repositories;

public class MovimientoRepository : BaseRepository<Movimiento>, IMovimientoRepository
{
    public MovimientoRepository(BankingDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Movimiento>> GetByCuentaIdAsync(int cuentaId)
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .Where(m => m.CuentaId == cuentaId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetByClienteIdAsync(int clienteId)
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .Where(m => m.Cuenta.ClienteId == clienteId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetByTipoAsync(TipoMovimiento tipoMovimiento)
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .Where(m => m.TipoMovimiento == tipoMovimiento)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRetirosDelDiaAsync(int cuentaId, DateTime fecha)
    {
        var inicioDelDia = fecha.Date;
        var finDelDia = inicioDelDia.AddDays(1).AddTicks(-1);

        return await _dbSet
            .Where(m => m.CuentaId == cuentaId &&
                       m.TipoMovimiento == TipoMovimiento.Debito &&
                       m.Fecha >= inicioDelDia &&
                       m.Fecha <= finDelDia)
            .Select(m => m.Valor.Monto)
            .SumAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetMovimientosDelDiaAsync(int cuentaId, DateTime fecha)
    {
        var inicioDelDia = fecha.Date;
        var finDelDia = inicioDelDia.AddDays(1).AddTicks(-1);

        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .Where(m => m.CuentaId == cuentaId &&
                       m.Fecha >= inicioDelDia &&
                       m.Fecha <= finDelDia)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public override async Task<Movimiento?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public override async Task<IEnumerable<Movimiento>> GetAllAsync()
    {
        return await _dbSet
            .Include(m => m.Cuenta)
            .ThenInclude(c => c.Cliente)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }
}
