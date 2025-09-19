using BankingSystem.Application.Commands.Movimientos;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Movimientos;

public class DeleteMovimientoCommandHandler : IRequestHandler<DeleteMovimientoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMovimientoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteMovimientoCommand request, CancellationToken cancellationToken)
    {
        var movimiento = await _unitOfWork.Movimientos.GetByIdAsync(request.Id);
        if (movimiento == null)
            throw new InvalidOperationException($"Movimiento con ID {request.Id} no encontrado");

        await _unitOfWork.Movimientos.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
