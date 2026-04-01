namespace LetTransactionService.Domain.Exceptions;

public class LetDomainException : Exception
{
    public LetDomainException(string message) : base(message) { }
    public LetDomainException(string message, Exception inner) : base(message, inner) { }
}
