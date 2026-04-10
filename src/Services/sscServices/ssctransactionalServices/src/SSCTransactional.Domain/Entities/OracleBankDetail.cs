using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_ORACLEBNKDET — Oracle bank details</summary>
public class OracleBankDetail : Entity<long>
{
    public long DocId { get; private set; }
    public string? Type { get; private set; }
    public string CheckId { get; private set; } = default!;
    public string? Business { get; private set; }
    public string? OrgId { get; private set; }
    public string? VendorSiteId { get; private set; }
    public string? FileName { get; private set; }
    public string? VendorCode { get; private set; }
    public string? Amount { get; private set; }
    public string? Currency { get; private set; }
    public string? PaymentNumber { get; private set; }
    public string? CheckNumber { get; private set; }
    public string? PaymentDate { get; private set; }
    public string? BeneIfsc { get; private set; }
    public string? BeneAccountType { get; private set; }
    public string? BeneBankName { get; private set; }
    public string? BeneBankAc { get; private set; }
    public string? BeneBankBranch { get; private set; }
    public string? BeneMailId { get; private set; }
    public string? UtrNo { get; private set; }
    public string? StatusLookupCode { get; private set; }

    private OracleBankDetail() { }

    public static OracleBankDetail Create(long id, long docId, string checkId)
    {
        return new OracleBankDetail
        {
            Id = id,
            DocId = docId,
            CheckId = checkId
        };
    }
}
