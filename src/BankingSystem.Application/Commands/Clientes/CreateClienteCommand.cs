using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using MediatR;

namespace BankingSystem.Application.Commands.Clientes;

public class CreateClienteCommand : IRequest<ClienteDto>
{
    public string ClienteId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public Genero Genero { get; set; }
    public int Edad { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string TipoIdentificacion { get; set; } = "Cedula";
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}
