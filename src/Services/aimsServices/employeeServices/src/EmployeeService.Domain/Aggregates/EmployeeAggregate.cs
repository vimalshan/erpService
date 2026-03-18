using EmployeeService.Domain.Common;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Aggregates;

/// <summary>
/// EmployeeAggregate — root aggregate for an employee.
/// Owns time-info, approvers, approval-mail mappings, calendars, patterns and shifts.
/// </summary>
public sealed class EmployeeAggregate : BaseEntity
{
    public EmployeeId EmpSysId { get; private set; } = null!;

    // Collections owned by this aggregate
    private readonly List<EmployeeTimeInfo> _timeInfos = new();
    private readonly List<EmployeeApprover> _approvers = new();
    private readonly List<EmployeeApprovalMail> _approvalMails = new();
    private readonly List<EmployeeCalendar> _calendars = new();
    private readonly List<EmployeePattern> _patterns = new();
    private readonly List<EmployeeShift> _shifts = new();
    private readonly List<EmployeeShiftPattern> _shiftPatterns = new();

    public IReadOnlyList<EmployeeTimeInfo> TimeInfos => _timeInfos.AsReadOnly();
    public IReadOnlyList<EmployeeApprover> Approvers => _approvers.AsReadOnly();
    public IReadOnlyList<EmployeeApprovalMail> ApprovalMails => _approvalMails.AsReadOnly();
    public IReadOnlyList<EmployeeCalendar> Calendars => _calendars.AsReadOnly();
    public IReadOnlyList<EmployeePattern> Patterns => _patterns.AsReadOnly();
    public IReadOnlyList<EmployeeShift> Shifts => _shifts.AsReadOnly();
    public IReadOnlyList<EmployeeShiftPattern> ShiftPatterns => _shiftPatterns.AsReadOnly();

    private EmployeeAggregate() { }

    public static EmployeeAggregate Create(long empSysId) =>
        new() { EmpSysId = EmployeeId.Of(empSysId) };

    public void AddApprover(EmployeeApprover approver)
    {
        if (_approvers.Any(a => a.Level.Value == approver.Level.Value))
            throw new InvalidOperationException($"An approver at level {approver.Level.Value} already exists.");
        _approvers.Add(approver);
    }

    public void MapCalendar(EmployeeCalendar calendar) => _calendars.Add(calendar);

    public void RecordTimeInfo(EmployeeTimeInfo info) => _timeInfos.Add(info);

    public void AssignPattern(EmployeePattern pattern) => _patterns.Add(pattern);

    public void AddShift(EmployeeShift shift) => _shifts.Add(shift);

    public void AddShiftPattern(EmployeeShiftPattern shiftPattern) => _shiftPatterns.Add(shiftPattern);
}
