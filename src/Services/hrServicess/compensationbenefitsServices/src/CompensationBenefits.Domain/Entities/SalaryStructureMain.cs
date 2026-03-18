using CompensationBenefits.Domain.Common;
using CompensationBenefits.Domain.Events;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to SALSTRUCTURE_MAIN table</summary>
public class SalaryStructureMain : BaseEntity
{
    public long StructureId { get; private set; }
    public long StructureUnitId { get; private set; }
    public string StructureName { get; private set; } = default!;
    public string StructureGradeCategory { get; private set; } = default!;
    public long StructureApplyToAll { get; private set; }
    public long StructureGradeId { get; private set; }
    public string StructureType { get; private set; } = default!; // C=CTC, F=Fixed
    public decimal StructureCtcMin { get; private set; }
    public decimal StructureCtcMax { get; private set; }
    public long StructureFooterId { get; private set; }
    public DateTime? StructureClsDate { get; private set; }
    public long StructureCreatedBy { get; private set; }
    public DateTime StructureCreatedOn { get; private set; }
    public long StructureLastModifiedBy { get; private set; }
    public DateTime StructureLastModifiedOn { get; private set; }
    public long? StructureApplyToAllUnit { get; private set; }
    public long? StructureOfferFooterId { get; private set; }

    public ICollection<SalaryStructureDetail> Details { get; private set; } = [];

    private SalaryStructureMain() { }

    public static SalaryStructureMain Create(long id, long unitId, string name, string gradeCategory,
        long gradeId, string type, decimal ctcMin, decimal ctcMax, long footerId, long createdBy)
    {
        var s = new SalaryStructureMain
        {
            StructureId = id,
            StructureUnitId = unitId,
            StructureName = name,
            StructureGradeCategory = gradeCategory,
            StructureApplyToAll = 0,
            StructureGradeId = gradeId,
            StructureType = type,
            StructureCtcMin = ctcMin,
            StructureCtcMax = ctcMax,
            StructureFooterId = footerId,
            StructureCreatedBy = createdBy,
            StructureCreatedOn = DateTime.UtcNow,
            StructureLastModifiedBy = createdBy,
            StructureLastModifiedOn = DateTime.UtcNow
        };
        s.AddDomainEvent(new SalaryStructureCreatedDomainEvent(id, name));
        return s;
    }

    public void Update(string name, decimal ctcMin, decimal ctcMax, long modifiedBy)
    {
        StructureName = name;
        StructureCtcMin = ctcMin;
        StructureCtcMax = ctcMax;
        StructureLastModifiedBy = modifiedBy;
        StructureLastModifiedOn = DateTime.UtcNow;
    }

    public void Close(long modifiedBy)
    {
        StructureClsDate = DateTime.UtcNow;
        StructureLastModifiedBy = modifiedBy;
        StructureLastModifiedOn = DateTime.UtcNow;
    }
}
