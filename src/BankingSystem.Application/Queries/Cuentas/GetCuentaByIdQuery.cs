using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Cuentas;

public class GetCuentaByIdQuery : IRequest<CuentaDto?>
{
    public int Id { get; set; }
}
