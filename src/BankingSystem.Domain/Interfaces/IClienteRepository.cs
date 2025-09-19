using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByClienteIdAsync(string clienteId);
    Task<Cliente?> GetByIdentificacionAsync(string numeroIdentificacion);
    Task<bool> ExistsByClienteIdAsync(string clienteId);
    Task<bool> ExistsByIdentificacionAsync(string numeroIdentificacion);
    Task<IEnumerable<Cliente>> GetActiveClientsAsync();
}
