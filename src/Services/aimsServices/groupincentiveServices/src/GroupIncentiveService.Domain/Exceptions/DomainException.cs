namespace GroupIncentiveService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.") { }
}

public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string rule) : base(rule) { }
}
