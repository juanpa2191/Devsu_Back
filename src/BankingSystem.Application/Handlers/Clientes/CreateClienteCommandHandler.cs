using AutoMapper;
using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.ValueObjects;
using MediatR;

namespace BankingSystem.Application.Handlers.Clientes;

public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, ClienteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ClienteDto> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        // Validar que el ClienteId no exista
        if (await _unitOfWork.Clientes.ExistsByClienteIdAsync(request.ClienteId))
            throw new InvalidOperationException($"Ya existe un cliente con el ID: {request.ClienteId}");

        // Validar que la identificación no exista
        if (await _unitOfWork.Clientes.ExistsByIdentificacionAsync(request.NumeroIdentificacion))
            throw new InvalidOperationException($"Ya existe un cliente con la identificación: {request.NumeroIdentificacion}");

        var identificacion = new Identificacion(request.NumeroIdentificacion, request.TipoIdentificacion);
        
        var cliente = new Cliente(
            request.Nombre,
            request.Genero,
            request.Edad,
            identificacion,
            request.Direccion,
            request.Telefono,
            request.ClienteId,
            request.Contrasena
        );

        var clienteCreado = await _unitOfWork.Clientes.AddAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ClienteDto>(clienteCreado);
    }
}
