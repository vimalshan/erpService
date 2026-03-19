namespace PurchaseSalesService.Domain.Exceptions;

public sealed class PurchaseNotFoundException : Exception
{
    public PurchaseNotFoundException(long serialNumber)
        : base($"Purchase with serial number {serialNumber} was not found.") { }
}

public sealed class SaleNotFoundException : Exception
{
    public SaleNotFoundException(long serialNumber)
        : base($"Sale with serial number {serialNumber} was not found.") { }
}

public sealed class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message) { }
}
