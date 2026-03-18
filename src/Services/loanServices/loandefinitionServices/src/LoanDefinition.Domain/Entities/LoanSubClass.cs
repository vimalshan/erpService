using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanSubClass : BaseEntity<long>
{
    public long LoanId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? ItClassification { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public long? PrincipalRecoveryEdId { get; private set; }
    public long? InterestRecoveryEdId { get; private set; }

    public LoanMaster? Loan { get; private set; }

    private LoanSubClass() { }

    public static LoanSubClass Create(long id, long loanId, string description, string? itClassification, long modifiedBy)
    {
        return new LoanSubClass
        {
            Id = id,
            LoanId = loanId,
            Description = description,
            ItClassification = itClassification,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string description, string? itClassification, long modifiedBy)
    {
        Description = description;
        ItClassification = itClassification;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
