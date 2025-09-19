using BankingSystem.Application.Commands.Cuentas;
using BankingSystem.Domain.Enums;
using FluentValidation;

namespace BankingSystem.Application.Validators;

public class CreateCuentaCommandValidator : AbstractValidator<CreateCuentaCommand>
{
    public CreateCuentaCommandValidator()
    {
        RuleFor(x => x.NumeroCuenta)
            .NotEmpty().WithMessage("El número de cuenta es requerido")
            .Length(6, 20).WithMessage("El número de cuenta debe tener entre 6 y 20 caracteres")
            .Matches(@"^[0-9]+$").WithMessage("El número de cuenta solo puede contener números");

        RuleFor(x => x.TipoCuenta)
            .IsInEnum().WithMessage("El tipo de cuenta debe ser válido");

        RuleFor(x => x.SaldoInicial)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo");

        RuleFor(x => x.ClienteId)
            .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0");
    }
}

public class UpdateCuentaCommandValidator : AbstractValidator<UpdateCuentaCommand>
{
    public UpdateCuentaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");
    }
}
