using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.Batch;

public class BatchSub : Entity<string>
{
    public string BatchId { get; private set; } = string.Empty;
    public string? BookingConfirmId { get; private set; }
    public string? BookingNo { get; private set; }
    public decimal BaseAmount { get; private set; }
    public decimal AdjustedAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal ApprovedAmount { get; private set; }
    public decimal ServiceTax { get; private set; }
    public decimal Cess { get; private set; }
    public decimal AdditionalTax { get; private set; }
    public decimal NetPayable { get; private set; }
    public string? Details { get; private set; }
    public string? VendorRemarks { get; private set; }
    public string CreditType { get; private set; } = string.Empty;
    public string? AdminRemarks { get; private set; }
    public string? TicketReference { get; private set; }
    public string? TourPlanId { get; private set; }
    public string? ForexRequestId { get; private set; }
    public string? InvoiceNo { get; private set; }
    public DateTime? InvoiceDate { get; private set; }
    public string? VendorId { get; private set; }

    protected BatchSub() { }

    public static BatchSub Create(
        string id, string batchId, string creditType,
        decimal baseAmount, decimal totalAmount, decimal netPayable,
        string? tourPlanId = null, string? ticketReference = null)
        => new()
        {
            Id = id,
            BatchId = batchId,
            CreditType = creditType,
            BaseAmount = baseAmount,
            AdjustedAmount = 0,
            TotalAmount = totalAmount,
            ApprovedAmount = 0,
            ServiceTax = 0,
            Cess = 0,
            AdditionalTax = 0,
            NetPayable = netPayable,
            TourPlanId = tourPlanId,
            TicketReference = ticketReference
        };
}
