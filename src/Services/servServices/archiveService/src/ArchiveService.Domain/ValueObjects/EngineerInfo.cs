using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.ValueObjects;

public class EngineerInfo : ValueObject
{
    public string? EngineerId { get; private set; }
    public string? EngineerName { get; private set; }
    public string? MobileNo { get; private set; }

    private EngineerInfo() { }

    public EngineerInfo(string? engineerId, string? engineerName, string? mobileNo)
    {
        EngineerId = engineerId;
        EngineerName = engineerName;
        MobileNo = mobileNo;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EngineerId;
        yield return EngineerName;
        yield return MobileNo;
    }
}
