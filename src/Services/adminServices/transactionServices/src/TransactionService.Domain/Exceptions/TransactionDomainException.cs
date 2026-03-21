namespace TransactionService.Domain.Exceptions;

public sealed class TransactionDomainException : Exception
{
    public TransactionDomainException(string message) : base(message) { }
    public TransactionDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
