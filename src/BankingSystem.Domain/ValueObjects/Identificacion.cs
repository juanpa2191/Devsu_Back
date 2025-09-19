using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BankingSystem.Domain.ValueObjects;

public record Identificacion
{
    public string Numero { get; init; }
    public string Tipo { get; init; }

    public Identificacion(string numero, string tipo = "Cedula")
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("El número de identificación no puede estar vacío", nameof(numero));
            
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("El tipo de identificación no puede estar vacío", nameof(tipo));

        // Validar formato básico
        if (!EsFormatoValido(numero))
            throw new ArgumentException("El formato de identificación no es válido", nameof(numero));

        Numero = numero.Trim();
        Tipo = tipo.Trim();
    }

    private static bool EsFormatoValido(string numero)
    {
        // Permitir números y guiones, longitud entre 7 y 13 caracteres
        var regex = new Regex(@"^[0-9-]{7,13}$");
        return regex.IsMatch(numero);
    }

    public override string ToString() => $"{Tipo}: {Numero}";
}
