using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TRAVEL_BATCHCONTRACT - Batch contract details
/// </summary>
public sealed class TravelBatchContract : BaseEntity
{
    private TravelBatchContract() { }

    public decimal ContractNum { get; private set; }
    public decimal BatchMainNum { get; private set; }
    public decimal? BookCnfNo { get; private set; }
    public string? TicketCost { get; private set; }
    public string? TicketCostAdj { get; private set; }
    public string? BasicTax { get; private set; }
    public string? TotalPayAmt { get; private set; }
    public string? ApprovedAmt { get; private set; }
    public string? ServiceTax { get; private set; }
    public string? CessTax { get; private set; }
    public string? AdditionalTax { get; private set; }
    public string? Remarks { get; private set; }
    public string? Remarks1 { get; private set; }
    public string? Remarks2 { get; private set; }

    public static TravelBatchContract Create(
        decimal contractNum, decimal batchMainNum,
        decimal? bookCnfNo = null, string? ticketCost = null,
        string? approvedAmt = null)
    {
        return new TravelBatchContract
        {
            ContractNum = contractNum,
            BatchMainNum = batchMainNum,
            BookCnfNo = bookCnfNo,
            TicketCost = ticketCost,
            ApprovedAmt = approvedAmt
        };
    }
}
