using Recruitment.Domain.Common;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities;

/// <summary>
/// ApplicationStatusHistory entity
/// </summary>
public class ApplicationStatusHistory : Entity
{
    public decimal ApplicationNumber { get; private set; }
    public decimal SerialNo { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public string Remarks { get; private set; }
    public string UpdatedBy { get; private set; }

    public ApplicationStatusHistory(
        decimal applicationNumber,
        ApplicationStatus status,
        string remarks,
        string updatedBy)
    {
        ApplicationNumber = applicationNumber;
        Status = status;
        Remarks = remarks;
        UpdatedBy = updatedBy;
        CreatedDate = DateTime.UtcNow;
    }
}
