namespace BankingSystem.Domain.Enums;

public enum TipoMovimiento
{
    Credito = 1,
    Debito = 2
}

public static class TipoMovimientoExtensions
{
    public static string ToString(this TipoMovimiento tipo)
    {
        return tipo switch
        {
            TipoMovimiento.Credito => "Crédito",
            TipoMovimiento.Debito => "Débito",
            _ => throw new ArgumentException($"Tipo de movimiento no válido: {tipo}")
        };
    }

    public static bool EsCredito(this TipoMovimiento tipo) => tipo == TipoMovimiento.Credito;
    public static bool EsDebito(this TipoMovimiento tipo) => tipo == TipoMovimiento.Debito;
}
