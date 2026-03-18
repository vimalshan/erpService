using System;

namespace EmployeeService.Domain.Common;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent
{
    protected DomainEvent()
    {
        DateOccurred = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset DateOccurred { get; } = DateTimeOffset.UtcNow;
    public bool IsPublished { get; set; } = false;
}
