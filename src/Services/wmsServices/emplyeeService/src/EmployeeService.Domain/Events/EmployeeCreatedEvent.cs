using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.Events;

public sealed class EmployeeCreatedEvent : IDomainEvent
{
    public int EmployeeId { get; }
    public string EmployeeCode { get; }
    public string FullName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public EmployeeCreatedEvent(int employeeId, string employeeCode, string fullName)
    {
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        FullName = fullName;
    }
}
