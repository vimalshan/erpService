using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.Events;

public sealed class EmployeeDeactivatedEvent : IDomainEvent
{
    public int EmployeeId { get; }
    public string EmployeeCode { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public EmployeeDeactivatedEvent(int employeeId, string employeeCode)
    {
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
    }
}
