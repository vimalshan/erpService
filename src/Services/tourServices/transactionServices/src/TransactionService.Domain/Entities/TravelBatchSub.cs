using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TRAVEL_BATCHSUB - Travel Batch line items
/// </summary>
public sealed class TravelBatchSub : BaseEntity
{
    private TravelBatchSub() { }

    public string BatchSubId { get; private set; } = default!;
    public string BatchId { get; private set; } = default!;
    public string? BookCnfId { get; private set; }
    public string? BookNo { get; private set; }
    public string? BasAmt { get; private set; }
    public string? AdjAmt { get; private set; }
    public string? TotAmt { get; private set; }
    public string? AppAmt { get; private set; }
    public string? SerTax { get; private set; }
    public string? CesTax { get; private set; }
    public string? AdlTax { get; private set; }
    public string? TotPay { get; private set; }
    public string? RefDet { get; private set; }
    public string? VenRemarks { get; private set; }
    public string CreditType { get; private set; } = default!;
    public string? AdmRemarks { get; private set; }
    public string? TktReference { get; private set; }
    public string? TpId { get; private set; }
    public string? ForReqId { get; private set; }
    public string? HigCes { get; private set; }
    public string? RndOff { get; private set; }
    public string? SurTax { get; private set; }
    public string? ChrTax { get; private set; }
    public string? InvNum { get; private set; }
    public DateTime? InvDate { get; private set; }
    public string? R12LocId { get; private set; }
    public string? CgstBas { get; private set; }
    public string? SgstBas { get; private set; }
    public string? TravelClass { get; private set; }
    public string? IgstBas { get; private set; }
    public string? CgstMgt { get; private set; }
    public string? SgstMgt { get; private set; }
    public string? IgstMgt { get; private set; }
    public string? R12Bu { get; private set; }
    public string? TaxBasic { get; private set; }
    public string? VendorId { get; private set; }

    public static TravelBatchSub Create(
        string batchSubId, string batchId, string creditType,
        string? bookCnfId = null, string? bookNo = null,
        string? basAmt = null, string? totAmt = null, string? appAmt = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(batchSubId);
        ArgumentException.ThrowIfNullOrWhiteSpace(batchId);

        return new TravelBatchSub
        {
            BatchSubId = batchSubId,
            BatchId = batchId,
            CreditType = creditType,
            BookCnfId = bookCnfId,
            BookNo = bookNo,
            BasAmt = basAmt,
            TotAmt = totAmt,
            AppAmt = appAmt
        };
    }
}
