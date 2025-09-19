using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Cuentas;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Queries;

public class GetCuentaByIdQueryHandler : IRequestHandler<GetCuentaByIdQuery, CuentaDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCuentaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CuentaDto?> Handle(GetCuentaByIdQuery request, CancellationToken cancellationToken)
    {
        var cuenta = await _unitOfWork.Cuentas.GetByIdAsync(request.Id);
        return cuenta != null ? _mapper.Map<CuentaDto>(cuenta) : null;
    }
}
