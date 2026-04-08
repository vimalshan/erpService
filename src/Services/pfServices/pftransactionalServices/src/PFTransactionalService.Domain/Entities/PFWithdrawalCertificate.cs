using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Domain.Entities;

/// <summary>
/// PF Withdrawal Certificate generated after settlement.
/// </summary>
public class PFWithdrawalCertificate : BaseEntity
{
    public long CertificateId { get; private set; }
    public long PfSettlementId { get; private set; }
    public long EmpSysId { get; private set; }
    public decimal CertificateAmount { get; private set; }
    public DateTime CertificateDate { get; private set; }
    public CertificateStatus CertificateStatus { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private PFWithdrawalCertificate() { }

    public PFWithdrawalCertificate(long settlementId, long empSysId, decimal amount, long createdBy)
    {
        PfSettlementId = settlementId;
        EmpSysId = empSysId;
        CertificateAmount = amount;
        CertificateDate = DateTime.UtcNow;
        CertificateStatus = CertificateStatus.Generated;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public void Issue()
    {
        if (CertificateStatus != CertificateStatus.Generated)
            throw new InvalidOperationException("Only generated certificates can be issued.");
        CertificateStatus = CertificateStatus.Issued;
    }

    public void Cancel()
    {
        if (CertificateStatus == CertificateStatus.Cancelled)
            throw new InvalidOperationException("Certificate is already cancelled.");
        CertificateStatus = CertificateStatus.Cancelled;
    }
}
