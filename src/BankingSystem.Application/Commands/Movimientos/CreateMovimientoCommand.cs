using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Enums;
using MediatR;

namespace BankingSystem.Application.Commands.Movimientos;

public class CreateMovimientoCommand : IRequest<MovimientoDto>
{
    public TipoMovimiento TipoMovimiento { get; set; }
    public decimal Valor { get; set; }
    public int CuentaId { get; set; }
}
