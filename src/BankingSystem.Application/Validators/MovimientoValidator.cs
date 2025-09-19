using BankingSystem.Application.Commands.Movimientos;
using BankingSystem.Domain.Enums;
using FluentValidation;

namespace BankingSystem.Application.Validators;

public class CreateMovimientoCommandValidator : AbstractValidator<CreateMovimientoCommand>
{
    public CreateMovimientoCommandValidator()
    {
        RuleFor(x => x.TipoMovimiento)
            .IsInEnum().WithMessage("El tipo de movimiento debe ser válido");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("El valor debe ser mayor a 0")
            .LessThanOrEqualTo(10000).WithMessage("El valor no puede ser mayor a $10,000");

        RuleFor(x => x.CuentaId)
            .GreaterThan(0).WithMessage("El ID de la cuenta debe ser mayor a 0");
    }
}
