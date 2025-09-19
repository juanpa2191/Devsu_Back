using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Enums;
using MediatR;

namespace BankingSystem.Application.Commands.Cuentas;

public class CreateCuentaCommand : IRequest<CuentaDto>
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public TipoCuenta TipoCuenta { get; set; }
    public decimal SaldoInicial { get; set; }
    public int ClienteId { get; set; }
}
