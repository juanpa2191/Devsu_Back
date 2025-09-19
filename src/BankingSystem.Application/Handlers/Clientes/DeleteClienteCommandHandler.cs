using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Clientes;

public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClienteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.Id);
        if (cliente == null)
            throw new ClienteNoEncontradoException($"Cliente con ID {request.Id} no encontrado");

        // Verificar si tiene cuentas activas
        var cuentas = await _unitOfWork.Cuentas.GetByClienteIdAsync(request.Id);
        if (cuentas.Any(c => c.Estado))
            throw new InvalidOperationException("No se puede eliminar un cliente que tiene cuentas activas");

        await _unitOfWork.Clientes.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
