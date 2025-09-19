using MediatR;

namespace BankingSystem.Application.Commands.Clientes;

public class DeleteClienteCommand : IRequest<bool>
{
    public int Id { get; set; }
}
