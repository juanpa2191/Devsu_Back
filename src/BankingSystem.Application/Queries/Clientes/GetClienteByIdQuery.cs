using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Queries.Clientes;

public class GetClienteByIdQuery : IRequest<ClienteDto?>
{
    public int Id { get; set; }
}
