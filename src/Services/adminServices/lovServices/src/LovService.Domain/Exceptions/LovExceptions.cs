namespace LovService.Domain.Exceptions;

public class LovNotFoundException : Exception
{
    public LovNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.") { }

    public LovNotFoundException(string message) : base(message) { }
}

public class LovDomainException : Exception
{
    public LovDomainException(string message) : base(message) { }
    public LovDomainException(string message, Exception inner) : base(message, inner) { }
}
