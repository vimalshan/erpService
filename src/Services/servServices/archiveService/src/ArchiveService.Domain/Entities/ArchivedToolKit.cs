using ArchiveService.Domain.Common;
using ArchiveService.Domain.Events;

namespace ArchiveService.Domain.Entities;

public class ArchivedToolKit : AggregateRoot<long>
{
    public string? KitCode { get; private set; }
    public string? AppPassword { get; private set; }
    public string? InstPassword { get; private set; }
    public string? ImeiNo { get; private set; }
    public string? EngineerId { get; private set; }
    public string? Flag { get; private set; }

    public ICollection<ArchivedToolKitTransaction> Transactions { get; private set; } = new List<ArchivedToolKitTransaction>();

    private ArchivedToolKit() { }

    public static ArchivedToolKit Create(
        string? kitCode, string? appPassword, string? instPassword,
        string? imeiNo, string? engineerId, string? flag, string? enteredBy)
    {
        var toolkit = new ArchivedToolKit
        {
            KitCode = kitCode,
            AppPassword = appPassword,
            InstPassword = instPassword,
            ImeiNo = imeiNo,
            EngineerId = engineerId,
            Flag = flag,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy
        };

        toolkit.AddDomainEvent(new ToolKitArchivedEvent(kitCode, engineerId));
        return toolkit;
    }

    public void UpdateFlag(string? flag, string? changedBy)
    {
        Flag = flag;
        ChangedOn = DateTime.UtcNow;
        ChangedBy = changedBy;
    }
}
