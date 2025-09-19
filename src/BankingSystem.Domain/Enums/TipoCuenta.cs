namespace BankingSystem.Domain.Enums;

public enum TipoCuenta
{
    Ahorro = 1,
    Corriente = 2
}

public static class TipoCuentaExtensions
{
    public static string ToString(this TipoCuenta tipo)
    {
        return tipo switch
        {
            TipoCuenta.Ahorro => "Ahorro",
            TipoCuenta.Corriente => "Corriente",
            _ => throw new ArgumentException($"Tipo de cuenta no válido: {tipo}")
        };
    }
}
