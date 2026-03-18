namespace CompetencyService.Domain.Exceptions;

public class CompetencyDomainException : Exception
{
    public CompetencyDomainException(string message) : base(message) { }
    public CompetencyDomainException(string message, Exception innerException) : base(message, innerException) { }
}
