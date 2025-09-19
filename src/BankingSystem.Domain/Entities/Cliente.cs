using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities;

public class Cliente : Persona
{
    [Required]
    [StringLength(50)]
    public string ClienteId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Contrasena { get; set; } = string.Empty;

    [Required]
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();

    // Constructor para Entity Framework
    protected Cliente() { }

    public Cliente(string nombre, Genero genero, int edad, Identificacion identificacion,
                   string direccion, string telefono, string clienteId, string contrasena)
        : base(nombre, genero, edad, identificacion, direccion, telefono)
    {
        ClienteId = clienteId ?? throw new ArgumentNullException(nameof(clienteId));
        Contrasena = contrasena ?? throw new ArgumentNullException(nameof(contrasena));
    }

    public void CambiarContrasena(string nuevaContrasena)
    {
        if (string.IsNullOrWhiteSpace(nuevaContrasena))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(nuevaContrasena));

        Contrasena = nuevaContrasena;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool PuedeRealizarTransacciones()
    {
        return Estado && !IsDeleted;
    }
}
