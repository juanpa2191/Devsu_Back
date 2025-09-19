using AutoMapper;
using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Clientes;

public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, ClienteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ClienteDto> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.Id);
        if (cliente == null)
            throw new ClienteNoEncontradoException($"Cliente con ID {request.Id} no encontrado");

        cliente.ActualizarInformacion(
            request.Nombre,
            request.Genero,
            request.Edad,
            request.Direccion,
            request.Telefono
        );

        await _unitOfWork.Clientes.UpdateAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ClienteDto>(cliente);
    }
}
