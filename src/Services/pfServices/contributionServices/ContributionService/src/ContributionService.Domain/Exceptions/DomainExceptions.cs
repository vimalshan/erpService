namespace ContributionService.Domain.Exceptions;

public class ContributionDomainException : Exception
{
    public ContributionDomainException(string message) : base(message) { }
    public ContributionDomainException(string message, Exception inner) : base(message, inner) { }
}

public class ContributionNotFoundException : ContributionDomainException
{
    public ContributionNotFoundException(string entity, object id)
        : base($"{entity} with id '{id}' was not found.") { }
}
