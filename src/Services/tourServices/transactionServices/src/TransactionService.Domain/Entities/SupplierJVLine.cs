using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to JVSUP_SUB - Supplier JV line items (Debit/Credit entries)
/// </summary>
public sealed class SupplierJVLine : BaseEntity
{
    private SupplierJVLine() { }

    public long JvSubId { get; private set; }
    public long JvId { get; private set; }
    public string JvBu { get; private set; } = default!;
    public string JvAcCode { get; private set; } = default!;
    public string JvSubAcc { get; private set; } = default!;
    public string JvCcCode { get; private set; } = default!;
    public string JvProduct { get; private set; } = default!;
    public string JvDcFlag { get; private set; } = default!;
    public decimal JvTrnAmt { get; private set; }
    public string JvLoc { get; private set; } = default!;
    public string JvRemarks { get; private set; } = default!;
    public string JvLineFlag { get; private set; } = default!;
    public string JvCombinationId { get; private set; } = default!;
    public string JvSubType { get; private set; } = default!;
    public string? JvCombinationCode { get; private set; }
    public string JvIutaBu { get; private set; } = default!;
    public long JvTpId { get; private set; }
    public long JvBatchSubId { get; private set; }
    public string? JvGstBu { get; private set; }
    public string? JvGstAcCode { get; private set; }
    public string? JvGstSubAcc { get; private set; }
    public string? JvGstCcCode { get; private set; }
    public string? JvGstProduct { get; private set; }
    public string? JvGstLoc { get; private set; }
    public string? JvGstCombinationId { get; private set; }
    public string? JvGstCombinationCode { get; private set; }
    public string? JvInvNo { get; private set; }
    public DateTime? JvInvDate { get; private set; }
    public string? JvPayType { get; private set; }
    public string? JvTpCat { get; private set; }
    public int? JvClass { get; private set; }
    public decimal? JvBasRateAmt { get; private set; }

    public static SupplierJVLine Create(
        long jvSubId, long jvId, string bu, string acCode, string subAcc,
        string ccCode, string product, string dcFlag, decimal trnAmt,
        string loc, string remarks, string lineFlag, string combinationId,
        string subType, string iutaBu, long tpId, long batchSubId,
        string? combinationCode = null)
    {
        return new SupplierJVLine
        {
            JvSubId = jvSubId,
            JvId = jvId,
            JvBu = bu,
            JvAcCode = acCode,
            JvSubAcc = subAcc,
            JvCcCode = ccCode,
            JvProduct = product,
            JvDcFlag = dcFlag,
            JvTrnAmt = trnAmt,
            JvLoc = loc,
            JvRemarks = remarks,
            JvLineFlag = lineFlag,
            JvCombinationId = combinationId,
            JvSubType = subType,
            JvIutaBu = iutaBu,
            JvTpId = tpId,
            JvBatchSubId = batchSubId,
            JvCombinationCode = combinationCode
        };
    }
}
