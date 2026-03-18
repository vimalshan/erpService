namespace HRService.Domain.Entities;

public class Position : Common.AggregateRoot
{
    public string PositionCode { get; private set; } = null!;
    public string PositionTitle { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid? ReportsToPositionId { get; private set; }

    private Position() { }

    public static Position Create(string positionCode, string positionTitle, Guid departmentId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(positionCode))
            throw new ArgumentException("Position code cannot be empty", nameof(positionCode));

        if (string.IsNullOrWhiteSpace(positionTitle))
            throw new ArgumentException("Position title cannot be empty", nameof(positionTitle));

        if (departmentId == Guid.Empty)
            throw new ArgumentException("Department id cannot be empty", nameof(departmentId));

        return new Position
        {
            Id = Guid.NewGuid(),
            PositionCode = positionCode,
            PositionTitle = positionTitle,
            Description = description,
            DepartmentId = departmentId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void SetReportsTo(Guid? positionId)
    {
        ReportsToPositionId = positionId;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
