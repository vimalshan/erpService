using TravelService.Domain.Common;
using TravelService.Domain.Events;

namespace TravelService.Domain.Entities.Batch;

public class BatchMain : AggregateRoot<string>
{
    public string AdminId { get; private set; } = string.Empty;
    public string PayrollUnitId { get; private set; } = string.Empty;
    public DateTime BatchDate { get; private set; }
    public string? InvoiceNo { get; private set; }
    public DateTime? InvoiceDate { get; private set; }
    public decimal InvoiceAmount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string? AdminRemarks { get; private set; }
    public string? FinanceRemarks { get; private set; }
    public string? VendorId { get; private set; }
    public decimal ApprovedAmount { get; private set; }
    public decimal BilledAmount { get; private set; }
    public decimal ServiceTax { get; private set; }
    public decimal Cess { get; private set; }
    public decimal AdditionalTax { get; private set; }
    public decimal TotalPayable { get; private set; }
    public string? JvId { get; private set; }
    public string? PaymentTerms { get; private set; }
    public DateTime? BillDate { get; private set; }
    public string? BatchType { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string? FinanceApprovedBy { get; private set; }
    public DateTime? FinanceApprovedOn { get; private set; }
    public string? CabType { get; private set; }
    public string? DocumentRefNo { get; private set; }
    public string? SourceUid { get; private set; }

    private readonly List<BatchSub> _batchSubs = new();
    public IReadOnlyCollection<BatchSub> BatchSubs => _batchSubs.AsReadOnly();

    protected BatchMain() { }

    public static BatchMain Create(string id, string adminId, string payrollUnitId,
        string createdBy, string? vendorId = null, string? batchType = null)
        => new()
        {
            Id = id,
            AdminId = adminId,
            PayrollUnitId = payrollUnitId,
            BatchDate = DateTime.UtcNow,
            Status = "C",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            VendorId = vendorId,
            BatchType = batchType,
            InvoiceAmount = 0,
            ApprovedAmount = 0,
            BilledAmount = 0,
            ServiceTax = 0,
            Cess = 0,
            AdditionalTax = 0,
            TotalPayable = 0
        };

    public void AdminApprove(string approvedBy, string? remarks = null)
    {
        Status = "A";
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        AdminRemarks = remarks;
        RaiseDomainEvent(new BatchApprovedEvent(Id, approvedBy));
    }

    public void FinanceApprove(string approvedBy, string? remarks = null)
    {
        Status = "F";
        FinanceApprovedBy = approvedBy;
        FinanceApprovedOn = DateTime.UtcNow;
        FinanceRemarks = remarks;
    }

    public void AddBatchSub(BatchSub sub) => _batchSubs.Add(sub);
}
