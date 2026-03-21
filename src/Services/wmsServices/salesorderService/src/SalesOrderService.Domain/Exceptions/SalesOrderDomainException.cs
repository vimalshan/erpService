namespace SalesOrderService.Domain.Exceptions;

public sealed class SalesOrderDomainException : Exception
{
    public SalesOrderDomainException(string message) : base(message) { }
    public SalesOrderDomainException(string message, Exception inner) : base(message, inner) { }
}
