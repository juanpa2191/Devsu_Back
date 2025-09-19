using BankingSystem.Domain.Interfaces;

namespace BankingSystem.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClienteRepository Clientes { get; }
    ICuentaRepository Cuentas { get; }
    IMovimientoRepository Movimientos { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
