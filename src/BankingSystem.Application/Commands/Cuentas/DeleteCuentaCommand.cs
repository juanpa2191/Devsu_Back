using MediatR;

namespace BankingSystem.Application.Commands.Cuentas;

public class DeleteCuentaCommand : IRequest<bool>
{
    public int Id { get; set; }
}
