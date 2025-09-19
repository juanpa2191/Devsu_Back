namespace BankingSystem.Domain.Exceptions;

public class CupoDiarioExcedidoException : Exception
{
    public CupoDiarioExcedidoException() : base("Cupo diario Excedido")
    {
    }

    public CupoDiarioExcedidoException(string message) : base(message)
    {
    }

    public CupoDiarioExcedidoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
