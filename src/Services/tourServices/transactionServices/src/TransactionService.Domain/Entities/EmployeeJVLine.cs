using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to JVEMP_SUB - Employee JV line items (Debit/Credit entries)
/// </summary>
public sealed class EmployeeJVLine : BaseEntity
{
    private EmployeeJVLine() { }

    public long JvSubId { get; private set; }
    public long JvBatchId { get; private set; }
    public string JvBu { get; private set; } = default!;
    public string JvAcCode { get; private set; } = default!;
    public string JvSubAcc { get; private set; } = default!;
    public string JvCcCode { get; private set; } = default!;
    public string JvProduct { get; private set; } = default!;
    public string JvDcFlag { get; private set; } = default!;
    public string JvTrnAmt { get; private set; } = default!;
    public string JvIutaBu { get; private set; } = default!;
    public string JvLoc { get; private set; } = default!;
    public string JvRemarks { get; private set; } = default!;
    public string JvLineFlag { get; private set; } = default!;
    public string? JvCombinationId { get; private set; }
    public string JvSubType { get; private set; } = default!;
    public string? JvCombinationCode { get; private set; }

    public static EmployeeJVLine Create(
        long jvSubId, long jvBatchId, string bu, string acCode, string subAcc,
        string ccCode, string product, string dcFlag, string trnAmt,
        string iutaBu, string loc, string remarks, string lineFlag,
        string subType, string? combinationId = null, string? combinationCode = null)
    {
        return new EmployeeJVLine
        {
            JvSubId = jvSubId,
            JvBatchId = jvBatchId,
            JvBu = bu,
            JvAcCode = acCode,
            JvSubAcc = subAcc,
            JvCcCode = ccCode,
            JvProduct = product,
            JvDcFlag = dcFlag,
            JvTrnAmt = trnAmt,
            JvIutaBu = iutaBu,
            JvLoc = loc,
            JvRemarks = remarks,
            JvLineFlag = lineFlag,
            JvSubType = subType,
            JvCombinationId = combinationId,
            JvCombinationCode = combinationCode
        };
    }
}
