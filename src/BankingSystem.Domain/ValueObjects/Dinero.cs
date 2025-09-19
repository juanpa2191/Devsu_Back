using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.ValueObjects;

public record Dinero
{
    public decimal Monto { get; init; }
    public string Moneda { get; init; } = "USD";

    public Dinero(decimal monto, string moneda = "USD")
    {
        if (monto < 0)
            throw new ArgumentException("El monto no puede ser negativo", nameof(monto));
            
        Monto = monto;
        Moneda = moneda ?? throw new ArgumentNullException(nameof(moneda));
    }

    public static Dinero operator +(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden sumar montos de diferentes monedas");
            
        return new Dinero(a.Monto + b.Monto, a.Moneda);
    }

    public static Dinero operator -(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden restar montos de diferentes monedas");
            
        return new Dinero(a.Monto - b.Monto, a.Moneda);
    }

    public static bool operator >(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");
            
        return a.Monto > b.Monto;
    }

    public static bool operator <(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");
            
        return a.Monto < b.Monto;
    }

    public static bool operator >=(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");
            
        return a.Monto >= b.Monto;
    }

    public static bool operator <=(Dinero a, Dinero b)
    {
        if (a.Moneda != b.Moneda)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");
            
        return a.Monto <= b.Monto;
    }

    public static Dinero Zero => new(0);
    public static Dinero MaximoRetiroDiario => new(1000);

    public override string ToString() => $"{Monto:C} {Moneda}";
}
