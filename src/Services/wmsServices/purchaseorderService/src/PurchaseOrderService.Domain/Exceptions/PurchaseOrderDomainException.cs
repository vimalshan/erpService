namespace PurchaseOrderService.Domain.Exceptions;

public class PurchaseOrderDomainException : Exception
{
    public PurchaseOrderDomainException(string message) : base(message) { }
    public PurchaseOrderDomainException(string message, Exception innerException) : base(message, innerException) { }
}
