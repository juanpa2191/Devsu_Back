using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Cuentas;

public class GetAllCuentasQuery : IRequest<IEnumerable<CuentaDto>>
{
}
