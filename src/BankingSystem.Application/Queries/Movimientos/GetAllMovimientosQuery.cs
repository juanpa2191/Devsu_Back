using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Movimientos;

public class GetAllMovimientosQuery : IRequest<IEnumerable<MovimientoDto>>
{
}
