using BankingSystem.Application.Handlers.Clientes;
using BankingSystem.Application.Handlers.Cuentas;
using BankingSystem.Application.Handlers.Movimientos;
using BankingSystem.Application.Handlers.Queries;
using BankingSystem.Application.Mappings;
using BankingSystem.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Command Handlers
        services.AddScoped<CreateClienteCommandHandler>();
        services.AddScoped<UpdateClienteCommandHandler>();
        services.AddScoped<DeleteClienteCommandHandler>();
        services.AddScoped<CreateCuentaCommandHandler>();
        services.AddScoped<UpdateCuentaCommandHandler>();
        services.AddScoped<DeleteCuentaCommandHandler>();
        services.AddScoped<CreateMovimientoCommandHandler>();
        services.AddScoped<DeleteMovimientoCommandHandler>();
        
        // Query Handlers
        services.AddScoped<GetClienteByIdQueryHandler>();
        services.AddScoped<GetAllClientesQueryHandler>();
        services.AddScoped<GetCuentaByIdQueryHandler>();
        services.AddScoped<GetAllCuentasQueryHandler>();
        services.AddScoped<GetMovimientoByIdQueryHandler>();
        services.AddScoped<GetAllMovimientosQueryHandler>();
        services.AddScoped<GetMovimientosByFechaQueryHandler>();

        return services;
    }
}
