using MediatR;

namespace BankingSystem.Application.Commands.Movimientos;

public class DeleteMovimientoCommand : IRequest<bool>
{
    public int Id { get; set; }
}
