using BankingSystem.Domain.Interfaces;
using BankingSystem.Infrastructure.Data;
using BankingSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankingDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(BankingDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Clientes = new ClienteRepository(_context);
        Cuentas = new CuentaRepository(_context);
        Movimientos = new MovimientoRepository(_context);
    }

    public IClienteRepository Clientes { get; }
    public ICuentaRepository Cuentas { get; }
    public IMovimientoRepository Movimientos { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
