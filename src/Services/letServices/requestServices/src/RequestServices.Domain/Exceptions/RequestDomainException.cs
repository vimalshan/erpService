namespace RequestServices.Domain.Exceptions;

public sealed class RequestDomainException : Exception
{
    public RequestDomainException(string message) : base(message) { }
    public RequestDomainException(string message, Exception inner) : base(message, inner) { }
}

public sealed class RequestNotFoundException : Exception
{
    public RequestNotFoundException(long requestId)
        : base($"Training request with ID '{requestId}' was not found.") { }
}
