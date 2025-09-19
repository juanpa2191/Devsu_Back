using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Movimientos;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Queries;

public class GetAllMovimientosQueryHandler : IRequestHandler<GetAllMovimientosQuery, IEnumerable<MovimientoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMovimientosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<MovimientoDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.Movimientos.GetAllAsync();
        return _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);
    }
}
