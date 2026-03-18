using CompensationBenefits.Domain.Common;
using CompensationBenefits.Domain.Events;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to SALARY_MAIN table</summary>
public class SalaryMain : BaseEntity
{
    public long SalaryId { get; private set; }
    public string SalaryType { get; private set; } = default!;   // C = CTC Based, F = Fixed
    public decimal SalaryCTC { get; private set; }
    public long SalaryStructureId { get; private set; }
    public long SalaryFooterId { get; private set; }
    public long? SalaryCopyEmpSysId { get; private set; }
    public long SalaryCreatedBy { get; private set; }
    public DateTime SalaryCreatedOn { get; private set; }
    public long? SalaryCancelledBy { get; private set; }
    public DateTime? SalaryCancelledOn { get; private set; }

    // Navigation
    public ICollection<SalaryDetail> Details { get; private set; } = [];

    private SalaryMain() { }

    public static SalaryMain Create(long id, string type, decimal ctc, long structureId, long footerId, long createdBy)
    {
        var salary = new SalaryMain
        {
            SalaryId = id,
            SalaryType = type,
            SalaryCTC = ctc,
            SalaryStructureId = structureId,
            SalaryFooterId = footerId,
            SalaryCreatedBy = createdBy,
            SalaryCreatedOn = DateTime.UtcNow
        };
        salary.AddDomainEvent(new SalaryCreatedDomainEvent(id, ctc));
        return salary;
    }

    public void Cancel(long cancelledBy)
    {
        SalaryCancelledBy = cancelledBy;
        SalaryCancelledOn = DateTime.UtcNow;
        AddDomainEvent(new SalaryCancelledDomainEvent(SalaryId));
    }
}
