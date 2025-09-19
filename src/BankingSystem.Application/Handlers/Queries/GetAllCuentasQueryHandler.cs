using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Cuentas;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Queries;

public class GetAllCuentasQueryHandler : IRequestHandler<GetAllCuentasQuery, IEnumerable<CuentaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCuentasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<CuentaDto>> Handle(GetAllCuentasQuery request, CancellationToken cancellationToken)
    {
        var cuentas = await _unitOfWork.Cuentas.GetAllAsync();
        return _mapper.Map<IEnumerable<CuentaDto>>(cuentas);
    }
}
