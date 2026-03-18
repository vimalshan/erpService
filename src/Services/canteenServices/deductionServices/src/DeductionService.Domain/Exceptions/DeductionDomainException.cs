namespace DeductionService.Domain.Exceptions;

public class DeductionDomainException : Exception
{
    public DeductionDomainException(string message) : base(message) { }
    public DeductionDomainException(string message, Exception inner) : base(message, inner) { }
}
