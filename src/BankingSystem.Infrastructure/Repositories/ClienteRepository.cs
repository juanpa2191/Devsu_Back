using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(BankingDbContext context) : base(context)
    {
    }

    public async Task<Cliente?> GetByClienteIdAsync(string clienteId)
    {
        return await _dbSet
            .Include(c => c.Cuentas)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<Cliente?> GetByIdentificacionAsync(string numeroIdentificacion)
    {
        return await _dbSet
            .Include(c => c.Cuentas)
            .Where(c => c.Identificacion.Numero == numeroIdentificacion)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByClienteIdAsync(string clienteId)
    {
        return await _dbSet.AnyAsync(c => c.ClienteId == clienteId);
    }

    public async Task<bool> ExistsByIdentificacionAsync(string numeroIdentificacion)
    {
        return await _dbSet
            .Where(c => c.Identificacion.Numero == numeroIdentificacion)
            .AnyAsync();
    }

    public async Task<IEnumerable<Cliente>> GetActiveClientsAsync()
    {
        return await _dbSet
            .Include(c => c.Cuentas)
            .Where(c => c.Estado && !c.IsDeleted)
            .ToListAsync();
    }

    public override async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Cuentas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public override async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.Cuentas)
            .ToListAsync();
    }
}
