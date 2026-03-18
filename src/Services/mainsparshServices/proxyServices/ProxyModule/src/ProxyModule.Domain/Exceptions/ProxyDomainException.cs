namespace ProxyModule.Domain.Exceptions;

public class ProxyDomainException : Exception
{
    public ProxyDomainException(string message) : base(message) { }
    public ProxyDomainException(string message, Exception innerException) : base(message, innerException) { }
}
