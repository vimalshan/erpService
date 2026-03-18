using TimeAttendance.Domain.Common;

namespace TimeAttendance.Domain.Entities;

/// <summary>
/// Aggregate root for absenteeism MIS records.
/// Maps to ABSMIS table.
/// </summary>
public class AbsenteeismMis : AuditableEntity
{
    public long Id { get; private set; }
    public int? UnitId { get; private set; }
    public int? CompanyId { get; private set; }
    public long? DepartmentId { get; private set; }
    public long? SystemId { get; private set; }
    public string? Grade { get; private set; }
    public decimal? PlannedLeave { get; private set; }
    public decimal? PaidDays { get; private set; }
    public decimal? WeeklyOff { get; private set; }
    public decimal? LeaveWithoutPay { get; private set; }
    public decimal? NumberOfPresentHours { get; private set; }
    public decimal? CompensatoryOff { get; private set; }
    public decimal? BankLeave { get; private set; }
    public decimal? AnnualPaidLeave { get; private set; }
    public decimal? PenaltyLeave { get; private set; }
    public decimal? ShiftSwap { get; private set; }
    public decimal? OnDuty { get; private set; }
    public string? Month { get; private set; }
    public decimal? LogSystemId { get; private set; }
    public decimal? LeaveWithoutPayPercentage { get; private set; }

    private AbsenteeismMis() { } // EF constructor

    public static AbsenteeismMis Create(
        int? unitId, int? companyId, long? departmentId,
        long? systemId, string? grade, string? month)
    {
        var entity = new AbsenteeismMis
        {
            UnitId = unitId,
            CompanyId = companyId,
            DepartmentId = departmentId,
            SystemId = systemId,
            Grade = grade,
            Month = month
        };

        entity.AddDomainEvent(new Events.AbsenteeismMisCreatedEvent(entity.Id, unitId, month));
        return entity;
    }

    public void UpdateLeaveData(
        decimal? plannedLeave, decimal? paidDays, decimal? weeklyOff,
        decimal? leaveWithoutPay, decimal? numberOfPresentHours,
        decimal? compensatoryOff, decimal? bankLeave, decimal? annualPaidLeave,
        decimal? penaltyLeave, decimal? shiftSwap, decimal? onDuty,
        decimal? leaveWithoutPayPercentage)
    {
        PlannedLeave = plannedLeave;
        PaidDays = paidDays;
        WeeklyOff = weeklyOff;
        LeaveWithoutPay = leaveWithoutPay;
        NumberOfPresentHours = numberOfPresentHours;
        CompensatoryOff = compensatoryOff;
        BankLeave = bankLeave;
        AnnualPaidLeave = annualPaidLeave;
        PenaltyLeave = penaltyLeave;
        ShiftSwap = shiftSwap;
        OnDuty = onDuty;
        LeaveWithoutPayPercentage = leaveWithoutPayPercentage;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new Events.AbsenteeismMisUpdatedEvent(Id, UnitId, Month));
    }
}
