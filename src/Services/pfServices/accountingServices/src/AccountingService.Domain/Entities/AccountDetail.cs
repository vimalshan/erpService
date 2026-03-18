using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to ACC_DET table – individual account ledger entries.</summary>
public class AccountDetail : BaseEntity
{
    public long AcSysId { get; private set; }
    public string AcTrustCode { get; private set; } = default!;
    public string AcTranCode { get; private set; } = default!;
    public long AcTranNo { get; private set; }
    public long AcDocNo { get; private set; }
    public long AcFinYer { get; private set; }
    public DateTime AcDocDat { get; private set; }
    public string AcMainCode { get; private set; } = default!;
    public string AcSubCode { get; private set; } = default!;
    public string AcDcType { get; private set; } = default!;   // D=Debit, C=Credit
    public decimal AcTranAmt { get; private set; }
    public string AcRefTranCode { get; private set; } = default!;
    public long AcRefTranNo { get; private set; }
    public string? AcRemarks { get; private set; }

    private AccountDetail() { }

    public static AccountDetail Create(
        long acSysId, string trustCode, string tranCode,
        long tranNo, long docNo, long finYer,
        DateTime docDat, string mainCode, string subCode,
        string dcType, decimal tranAmt, string refTranCode,
        long refTranNo, string? remarks = null)
    {
        if (dcType != "D" && dcType != "C")
            throw new ArgumentException("DC Type must be 'D' (Debit) or 'C' (Credit).", nameof(dcType));

        var entity = new AccountDetail
        {
            AcSysId = acSysId,
            AcTrustCode = trustCode,
            AcTranCode = tranCode,
            AcTranNo = tranNo,
            AcDocNo = docNo,
            AcFinYer = finYer,
            AcDocDat = docDat,
            AcMainCode = mainCode,
            AcSubCode = subCode,
            AcDcType = dcType,
            AcTranAmt = tranAmt,
            AcRefTranCode = refTranCode,
            AcRefTranNo = refTranNo,
            AcRemarks = remarks
        };

        entity.AddDomainEvent(new Events.AccountDetailCreatedEvent(entity));
        return entity;
    }
}
