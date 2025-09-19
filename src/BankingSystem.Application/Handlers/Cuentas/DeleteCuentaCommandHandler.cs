using BankingSystem.Application.Commands.Cuentas;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Cuentas;

public class DeleteCuentaCommandHandler : IRequestHandler<DeleteCuentaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCuentaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteCuentaCommand request, CancellationToken cancellationToken)
    {
        var cuenta = await _unitOfWork.Cuentas.GetByIdAsync(request.Id);
        if (cuenta == null)
            throw new CuentaNoEncontradaException($"Cuenta con ID {request.Id} no encontrada");

        // Verificar si tiene movimientos
        var movimientos = await _unitOfWork.Movimientos.GetByCuentaIdAsync(request.Id);
        if (movimientos.Any())
            throw new InvalidOperationException("No se puede eliminar una cuenta que tiene movimientos");

        await _unitOfWork.Cuentas.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
