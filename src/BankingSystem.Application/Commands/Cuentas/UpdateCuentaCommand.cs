using BankingSystem.Application.DTOs;
using MediatR;

namespace BankingSystem.Application.Commands.Cuentas;

public class UpdateCuentaCommand : IRequest<CuentaDto>
{
    public int Id { get; set; }
    public bool Estado { get; set; }
}
