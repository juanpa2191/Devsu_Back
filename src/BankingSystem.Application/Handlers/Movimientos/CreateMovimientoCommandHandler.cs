using AutoMapper;
using BankingSystem.Application.Commands.Movimientos;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Services;
using BankingSystem.Domain.ValueObjects;
using MediatR;

namespace BankingSystem.Application.Handlers.Movimientos;

public class CreateMovimientoCommandHandler : IRequestHandler<CreateMovimientoCommand, MovimientoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransaccionService _transaccionService;
    private readonly IMapper _mapper;

    public CreateMovimientoCommandHandler(
        IUnitOfWork unitOfWork, 
        ITransaccionService transaccionService, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MovimientoDto> Handle(CreateMovimientoCommand request, CancellationToken cancellationToken)
    {
        // Validar que la cuenta existe
        var cuenta = await _unitOfWork.Cuentas.GetByIdAsync(request.CuentaId);
        if (cuenta == null)
            throw new CuentaNoEncontradaException($"Cuenta con ID {request.CuentaId} no encontrada");

        var monto = new Dinero(request.Valor);

        // Realizar la transacción usando el servicio de dominio
        var movimiento = await _transaccionService.RealizarMovimientoAsync(
            cuenta, 
            request.TipoMovimiento, 
            monto
        );

        // Guardar el movimiento
        var movimientoCreado = await _unitOfWork.Movimientos.AddAsync(movimiento);
        
        // Actualizar la cuenta
        await _unitOfWork.Cuentas.UpdateAsync(cuenta);
        
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MovimientoDto>(movimientoCreado);
    }
}
