namespace LoanManagement.Domain.Exceptions;

public class LoanDomainException : Exception
{
    public LoanDomainException(string message) : base(message) { }

    public LoanDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
