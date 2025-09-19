namespace BankingSystem.Domain.Exceptions;

public class ClienteNoEncontradoException : Exception
{
    public ClienteNoEncontradoException() : base("Cliente no encontrado")
    {
    }

    public ClienteNoEncontradoException(string message) : base(message)
    {
    }

    public ClienteNoEncontradoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
