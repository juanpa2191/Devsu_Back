using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Movimientos;

public class GetMovimientosByFechaQuery : IRequest<IEnumerable<ReporteMovimientoDto>>
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int? ClienteId { get; set; }
}
