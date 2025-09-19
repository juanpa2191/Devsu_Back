using AutoMapper;
using BankingSystem.Application.Commands.Cuentas;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.ValueObjects;
using MediatR;

namespace BankingSystem.Application.Handlers.Cuentas;

public class CreateCuentaCommandHandler : IRequestHandler<CreateCuentaCommand, CuentaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCuentaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CuentaDto> Handle(CreateCuentaCommand request, CancellationToken cancellationToken)
    {
        // Validar que el cliente existe
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.ClienteId);
        if (cliente == null)
            throw new ClienteNoEncontradoException($"Cliente con ID {request.ClienteId} no encontrado");

        if (!cliente.PuedeRealizarTransacciones())
            throw new InvalidOperationException("El cliente no está activo");

        // Validar que el número de cuenta no exista
        if (await _unitOfWork.Cuentas.ExistsByNumeroCuentaAsync(request.NumeroCuenta))
            throw new InvalidOperationException($"Ya existe una cuenta con el número: {request.NumeroCuenta}");

        var saldoInicial = new Dinero(request.SaldoInicial);
        
        var cuenta = new Cuenta(
            request.NumeroCuenta,
            request.TipoCuenta,
            saldoInicial,
            request.ClienteId
        );

        var cuentaCreada = await _unitOfWork.Cuentas.AddAsync(cuenta);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CuentaDto>(cuentaCreada);
    }
}
