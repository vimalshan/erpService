namespace TdsService.Domain.Exceptions;

public class TdsVendorNotFoundException : DomainException
{
    public TdsVendorNotFoundException(long vendorId)
        : base($"TDS Vendor with ID '{vendorId}' was not found.") { }

    public TdsVendorNotFoundException(string pan)
        : base($"TDS Vendor with PAN '{pan}' was not found.") { }
}
