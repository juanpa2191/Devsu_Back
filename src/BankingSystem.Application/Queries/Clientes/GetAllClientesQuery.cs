using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Clientes;

public class GetAllClientesQuery : IRequest<IEnumerable<ClienteDto>>
{
}
