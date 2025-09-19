using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Movimientos;

public class GetMovimientoByIdQuery : IRequest<MovimientoDto?>
{
    public int Id { get; set; }
}
