namespace BankingSystem.Domain.Exceptions;

public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException() : base("Saldo no disponible")
    {
    }

    public SaldoInsuficienteException(string message) : base(message)
    {
    }

    public SaldoInsuficienteException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
