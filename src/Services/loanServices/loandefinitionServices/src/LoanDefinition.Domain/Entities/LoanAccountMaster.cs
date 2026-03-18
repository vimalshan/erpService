using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanAccountMaster : BaseEntity<long>
{
    public long LoanType { get; private set; }
    public string GradeType { get; private set; } = string.Empty;
    public string AccountCode { get; private set; } = string.Empty;
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private LoanAccountMaster() { }

    public static LoanAccountMaster Create(long id, long loanType, string gradeType, string accountCode, long updatedBy)
    {
        return new LoanAccountMaster
        {
            Id = id,
            LoanType = loanType,
            GradeType = gradeType,
            AccountCode = accountCode,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void Update(string gradeType, string accountCode, long updatedBy)
    {
        GradeType = gradeType;
        AccountCode = accountCode;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
