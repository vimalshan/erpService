using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class VehicleInvoice : BaseEntity
{
    public long TrackingNumber { get; set; }
    public long ReferenceNumber { get; set; }
    public long InvoiceSerial { get; set; }
    public long? OriginalInvoice { get; set; }
    public long ChainInvoice { get; set; }
    public string? CustomerCode { get; set; }
    public char? CancelFlag { get; set; }
    public long ModifiedNumber { get; set; }
    public string ModifiedUser { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
}
