using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class AccountMaster : BaseEntity
{
    public string? CompanyCode { get; private set; }
    public string? EdCode { get; private set; }
    public string? AccountCode { get; private set; }
    public string? GradeType { get; private set; }
    public string? DebitCreditFlag { get; private set; }
    public string? SubCode { get; private set; }
    public string? AccountDescription { get; private set; }

    private AccountMaster() { }

    public static AccountMaster Create(
        string? companyCode,
        string? edCode,
        string? accountCode,
        string? gradeType,
        string? dcFlag,
        string? subCode,
        string? description)
    {
        return new AccountMaster
        {
            CompanyCode = companyCode,
            EdCode = edCode,
            AccountCode = accountCode,
            GradeType = gradeType,
            DebitCreditFlag = dcFlag,
            SubCode = subCode,
            AccountDescription = description
        };
    }
}
