using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Department : Entity
{
    public decimal DepartmentId { get; private set; }
    public string? DepartmentName { get; private set; }
    public LiveFlag? LiveFlag { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }
    public string? DepartmentCode { get; private set; }

    private Department() { }

    public static Department Create(decimal departmentId, string departmentName, decimal updatedBy)
    {
        return new Department
        {
            DepartmentId = departmentId,
            DepartmentName = departmentName,
            LiveFlag = ValueObjects.LiveFlag.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
    }

    public void Update(string departmentName, string? departmentCode, decimal updatedBy)
    {
        DepartmentName = departmentName;
        DepartmentCode = departmentCode;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
