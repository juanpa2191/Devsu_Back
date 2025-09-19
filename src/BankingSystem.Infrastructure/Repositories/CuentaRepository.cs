using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Repositories;

public class CuentaRepository : BaseRepository<Cuenta>, ICuentaRepository
{
    public CuentaRepository(BankingDbContext context) : base(context)
    {
    }

    public async Task<Cuenta?> GetByNumeroCuentaAsync(string numeroCuenta)
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
    }

    public async Task<IEnumerable<Cuenta>> GetByClienteIdAsync(int clienteId)
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cuenta>> GetByTipoAsync(TipoCuenta tipoCuenta)
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .Where(c => c.TipoCuenta == tipoCuenta)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNumeroCuentaAsync(string numeroCuenta)
    {
        return await _dbSet.AnyAsync(c => c.NumeroCuenta == numeroCuenta);
    }

    public async Task<IEnumerable<Cuenta>> GetActiveAccountsAsync()
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .Where(c => c.Estado && !c.IsDeleted)
            .ToListAsync();
    }

    public override async Task<Cuenta?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public override async Task<IEnumerable<Cuenta>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.Cliente)
            .Include(c => c.Movimientos)
            .ToListAsync();
    }
}
