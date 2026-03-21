using FinanceService.Domain.Common;
using FinanceService.Domain.Events;

namespace FinanceService.Domain.Entities;

public class TravelBatchMain : AggregateRoot
{
    public string UnitCode { get; set; } = string.Empty;
    public decimal BatchNumber { get; set; }
    public DateTime? BatchDate { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string? BatchStatus { get; set; }
    public string? AdminRemarks { get; set; }
    public string? FinanceRemarks { get; set; }
    public decimal? AgencyCode { get; set; }
    public decimal? TotalApprovedAmount { get; set; }
    public decimal? Total { get; set; }
    public long? JvNo { get; set; }
    public decimal? CgstAmount { get; set; }
    public decimal? SgstAmount { get; set; }
    public decimal? IgstAmount { get; set; }

    public virtual ICollection<TravelBatchSub> BatchLines { get; set; } = new List<TravelBatchSub>();

    public void Approve(string? remarks)
    {
        BatchStatus = "Y";
        FinanceRemarks = remarks;
        AddDomainEvent(new BatchApprovedEvent(UnitCode, BatchNumber));
    }

    public void MarkPaymentInProgress()
    {
        BatchStatus = "P";
    }

    public static TravelBatchMain Create(string unitCode, decimal batchNumber, decimal? agencyCode,
        string? invoiceNumber, string? adminRemarks, decimal? totalAmount)
    {
        var batch = new TravelBatchMain
        {
            UnitCode = unitCode,
            BatchNumber = batchNumber,
            BatchDate = DateTime.UtcNow,
            InvoiceNumber = invoiceNumber,
            BatchStatus = "N",
            AdminRemarks = adminRemarks,
            AgencyCode = agencyCode,
            Total = totalAmount
        };
        batch.AddDomainEvent(new BatchCreatedEvent(unitCode, batchNumber));
        return batch;
    }
}
