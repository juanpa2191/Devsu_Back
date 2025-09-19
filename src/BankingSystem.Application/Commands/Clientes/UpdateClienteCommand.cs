using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Enums;
using MediatR;

namespace BankingSystem.Application.Commands.Clientes;

public class UpdateClienteCommand : IRequest<ClienteDto>
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public Genero Genero { get; set; }
    public int Edad { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}
