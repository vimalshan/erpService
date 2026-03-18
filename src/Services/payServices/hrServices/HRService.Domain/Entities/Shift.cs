namespace HRService.Domain.Entities;

public class Shift : Common.AggregateRoot
{
    public string ShiftCode { get; private set; } = null!;
    public string ShiftName { get; private set; } = null!;
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    private Shift() { }

    public static Shift Create(string shiftCode, string shiftName, TimeSpan startTime, TimeSpan endTime)
    {
        if (string.IsNullOrWhiteSpace(shiftCode))
            throw new ArgumentException("Shift code cannot be empty", nameof(shiftCode));

        if (string.IsNullOrWhiteSpace(shiftName))
            throw new ArgumentException("Shift name cannot be empty", nameof(shiftName));

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time");

        return new Shift
        {
            Id = Guid.NewGuid(),
            ShiftCode = shiftCode,
            ShiftName = shiftName,
            StartTime = startTime,
            EndTime = endTime,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
