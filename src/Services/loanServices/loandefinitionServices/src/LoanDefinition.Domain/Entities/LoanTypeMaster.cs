using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanTypeMaster : AggregateRoot<long>
{
    public string LoanName { get; private set; } = string.Empty;
    public string LoanCategory { get; private set; } = string.Empty;

    private readonly List<LoanMaster> _loans = [];
    public IReadOnlyCollection<LoanMaster> Loans => _loans.AsReadOnly();

    private LoanTypeMaster() { }

    public static LoanTypeMaster Create(long loanType, string loanName, string loanCategory, long createdBy)
    {
        var entity = new LoanTypeMaster
        {
            Id = loanType,
            LoanName = loanName,
            LoanCategory = loanCategory,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.LoanTypeCreatedEvent(entity.Id, loanName));
        return entity;
    }

    public void Update(string loanName, string loanCategory, long modifiedBy)
    {
        LoanName = loanName;
        LoanCategory = loanCategory;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new Events.LoanTypeUpdatedEvent(Id, loanName));
    }
}
