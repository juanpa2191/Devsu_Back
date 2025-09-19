using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Domain.Interfaces;

public interface ICuentaRepository : IRepository<Cuenta>
{
    Task<Cuenta?> GetByNumeroCuentaAsync(string numeroCuenta);
    Task<IEnumerable<Cuenta>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<Cuenta>> GetByTipoAsync(TipoCuenta tipoCuenta);
    Task<bool> ExistsByNumeroCuentaAsync(string numeroCuenta);
    Task<IEnumerable<Cuenta>> GetActiveAccountsAsync();
}
