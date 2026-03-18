using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class DisciplinaryEmp : BaseEntity
{
    // Composite primary key: MainId + EmpSysId
    public long MainId { get; private set; }
    public long EmpSysId { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    protected DisciplinaryEmp() { }

    public DisciplinaryEmp(long mainId, long empSysId)
    {
        MainId = mainId;
        EmpSysId = empSysId;
    }
}
