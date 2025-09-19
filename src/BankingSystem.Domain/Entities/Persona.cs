using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities;

public class Persona : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public Genero Genero { get; set; }

    [Required]
    [Range(1, 120)]
    public int Edad { get; set; }

    [Required]
    public Identificacion Identificacion { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    // Constructor para Entity Framework
    protected Persona() { }

    public Persona(string nombre, Genero genero, int edad, Identificacion identificacion, 
                   string direccion, string telefono)
    {
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        Genero = genero;
        Edad = edad;
        Identificacion = identificacion ?? throw new ArgumentNullException(nameof(identificacion));
        Direccion = direccion ?? throw new ArgumentNullException(nameof(direccion));
        Telefono = telefono ?? throw new ArgumentNullException(nameof(telefono));
    }

    public void ActualizarInformacion(string nombre, Genero genero, int edad, 
                                     string direccion, string telefono)
    {
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        Genero = genero;
        Edad = edad;
        Direccion = direccion ?? throw new ArgumentNullException(nameof(direccion));
        Telefono = telefono ?? throw new ArgumentNullException(nameof(telefono));
        UpdatedAt = DateTime.UtcNow;
    }
}
