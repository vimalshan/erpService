using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class PatternMaster : BaseEntity
{
    public int PatternId { get; private set; }
    public string PatternName { get; private set; } = string.Empty;
    public string? PatternDescription { get; private set; }
    public int PatternCycleId { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public ICollection<PatternDetail> Details { get; private set; } = [];

    private PatternMaster() { }

    public static PatternMaster Create(int id, string name, int cycleId, long modifiedBy, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new PatternMaster
        {
            PatternId = id,
            PatternName = name,
            PatternDescription = description,
            PatternCycleId = cycleId,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        PatternName = name;
        PatternDescription = description;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
