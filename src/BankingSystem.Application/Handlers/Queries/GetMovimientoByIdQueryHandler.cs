using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Movimientos;
using BankingSystem.Domain.Interfaces;
using MediatR;

namespace BankingSystem.Application.Handlers.Queries;

public class GetMovimientoByIdQueryHandler : IRequestHandler<GetMovimientoByIdQuery, MovimientoDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMovimientoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MovimientoDto?> Handle(GetMovimientoByIdQuery request, CancellationToken cancellationToken)
    {
        var movimiento = await _unitOfWork.Movimientos.GetByIdAsync(request.Id);
        return movimiento != null ? _mapper.Map<MovimientoDto>(movimiento) : null;
    }
}
