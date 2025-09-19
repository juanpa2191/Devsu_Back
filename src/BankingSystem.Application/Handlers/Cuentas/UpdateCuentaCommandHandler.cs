using AutoMapper;
using BankingSystem.Application.Commands.Cuentas;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Cuentas;

public class UpdateCuentaCommandHandler : IRequestHandler<UpdateCuentaCommand, CuentaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCuentaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CuentaDto> Handle(UpdateCuentaCommand request, CancellationToken cancellationToken)
    {
        var cuenta = await _unitOfWork.Cuentas.GetByIdAsync(request.Id);
        if (cuenta == null)
            throw new CuentaNoEncontradaException($"Cuenta con ID {request.Id} no encontrada");

        if (request.Estado)
            cuenta.Activar();
        else
            cuenta.Desactivar();

        await _unitOfWork.Cuentas.UpdateAsync(cuenta);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CuentaDto>(cuenta);
    }
}
