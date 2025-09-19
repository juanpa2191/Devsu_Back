namespace BankingSystem.Domain.Enums;

public enum Genero
{
    Masculino = 1,
    Femenino = 2,
    Otro = 3
}

public static class GeneroExtensions
{
    public static string ToString(this Genero genero)
    {
        return genero switch
        {
            Genero.Masculino => "Masculino",
            Genero.Femenino => "Femenino",
            Genero.Otro => "Otro",
            _ => throw new ArgumentException($"Género no válido: {genero}")
        };
    }
}
