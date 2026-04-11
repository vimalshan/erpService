using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TRAVEL_EMPPAYDET - Employee travel payment details
/// </summary>
public sealed class EmployeeTravelPay : BaseEntity
{
    private EmployeeTravelPay() { }

    public string? EmpPayId { get; private set; }
    public string? EmpPayEmpSysId { get; private set; }
    public string? EmpPayType { get; private set; }
    public string? EmpPayMode { get; private set; }
    public DateTime? EmpPayTrnDate { get; private set; }
    public string? EmpPayAmount { get; private set; }
    public string? EmpPaySource { get; private set; }
    public string? EmpPayTrnType { get; private set; }
    public DateTime? EmpPayDate { get; private set; }
    public string? EmpPayRefId { get; private set; }
    public string? EmpPayAccType { get; private set; }
    public string? EmpPayAccRefNo { get; private set; }
    public string? EmpPayTpId { get; private set; }

    public static EmployeeTravelPay Create(
        string empPayId, string empSysId, string payType, string mode,
        string amount, string source, string trnType, string? tpId = null)
    {
        return new EmployeeTravelPay
        {
            EmpPayId = empPayId,
            EmpPayEmpSysId = empSysId,
            EmpPayType = payType,
            EmpPayMode = mode,
            EmpPayTrnDate = DateTime.UtcNow,
            EmpPayAmount = amount,
            EmpPaySource = source,
            EmpPayTrnType = trnType,
            EmpPayTpId = tpId
        };
    }
}
