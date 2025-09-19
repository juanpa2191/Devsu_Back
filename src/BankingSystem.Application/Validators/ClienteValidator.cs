using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Domain.Enums;
using FluentValidation;

namespace BankingSystem.Application.Validators;

public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
{
    public CreateClienteCommandValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty().WithMessage("El ID del cliente es requerido")
            .Length(3, 20).WithMessage("El ID del cliente debe tener entre 3 y 20 caracteres");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

        RuleFor(x => x.Genero)
            .IsInEnum().WithMessage("El género debe ser válido");

        RuleFor(x => x.Edad)
            .GreaterThan(0).WithMessage("La edad debe ser mayor a 0")
            .LessThanOrEqualTo(120).WithMessage("La edad debe ser menor o igual a 120");

        RuleFor(x => x.NumeroIdentificacion)
            .NotEmpty().WithMessage("El número de identificación es requerido")
            .Length(7, 13).WithMessage("El número de identificación debe tener entre 7 y 13 caracteres")
            .Matches(@"^[0-9-]+$").WithMessage("El número de identificación solo puede contener números y guiones");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida")
            .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .Length(7, 20).WithMessage("El teléfono debe tener entre 7 y 20 caracteres")
            .Matches(@"^[0-9+\-\s()]+$").WithMessage("El teléfono solo puede contener números, espacios, guiones, paréntesis y el signo +");

        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .Length(4, 50).WithMessage("La contraseña debe tener entre 4 y 50 caracteres");
    }
}

public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
{
    public UpdateClienteCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

        RuleFor(x => x.Genero)
            .IsInEnum().WithMessage("El género debe ser válido");

        RuleFor(x => x.Edad)
            .GreaterThan(0).WithMessage("La edad debe ser mayor a 0")
            .LessThanOrEqualTo(120).WithMessage("La edad debe ser menor o igual a 120");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida")
            .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .Length(7, 20).WithMessage("El teléfono debe tener entre 7 y 20 caracteres")
            .Matches(@"^[0-9+\-\s()]+$").WithMessage("El teléfono solo puede contener números, espacios, guiones, paréntesis y el signo +");
    }
}
