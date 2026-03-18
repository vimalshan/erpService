using System;

namespace AppraisalService.Domain.Entities;

/// <summary>
/// Employee Goal entity
/// </summary>
public class EmployeeGoalEntity : Entity
{
    public long RequestNumber { get; set; }
    public long SerialNumber { get; set; }
    public long? PinNumber { get; set; }
    public string? UserId { get; set; }
    public string? PersonDesignation { get; set; }
    public string? UnitFrom { get; set; }
    public string? UnitTo { get; set; }
    public decimal? Weightage { get; set; }
    public string? AppraiseeRemark { get; set; }
    public string? Remark { get; set; }
    public DateTime? FinancialStartDate { get; set; }
    public DateTime? FinancialEndDate { get; set; }
    public string? Category { get; set; }
    public string? UnitOfMeasure { get; set; }
    public string? Status { get; set; }
    public string? Achievements { get; set; }
    public string? Difficulties { get; set; }
    public long? ModifiedSerialNumber { get; set; }
    public string? ExperienceCode { get; set; }
    public string? GoalFlag { get; set; }
    public long? AccountabilityId { get; set; }

    private EmployeeGoalEntity() { }

    public EmployeeGoalEntity(long requestNumber, long serialNumber, string? userId, long? pin)
    {
        RequestNumber = requestNumber;
        SerialNumber = serialNumber;
        UserId = userId;
        PinNumber = pin;
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddAchievements(string achievements, string difficulties)
    {
        Achievements = achievements;
        Difficulties = difficulties;
        ModifiedOn = DateTime.UtcNow;
    }
}
