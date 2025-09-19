namespace BankingSystem.Domain.Exceptions;

public class CuentaNoEncontradaException : Exception
{
    public CuentaNoEncontradaException() : base("Cuenta no encontrada")
    {
    }

    public CuentaNoEncontradaException(string message) : base(message)
    {
    }

    public CuentaNoEncontradaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
