using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Division : Entity
{
    public decimal DivisionId { get; private set; }
    public string? DivisionCode { get; private set; }
    public string? DivisionName { get; private set; }
    public LiveFlag? LiveFlag { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }

    private Division() { }

    public static Division Create(decimal divisionId, string divisionName, string divisionCode, decimal updatedBy)
    {
        return new Division
        {
            DivisionId = divisionId,
            DivisionName = divisionName,
            DivisionCode = divisionCode,
            LiveFlag = ValueObjects.LiveFlag.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
    }

    public void Update(string divisionName, string divisionCode, decimal updatedBy)
    {
        DivisionName = divisionName;
        DivisionCode = divisionCode;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
