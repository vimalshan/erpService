using LookupService.Domain.Common;
using LookupService.Domain.Events;

namespace LookupService.Domain.Entities;

public class ProcessMaster : AggregateRoot
{
    public decimal ProcessId { get; private set; }
    public string? ProcessName { get; private set; }
    public string? ProcessLivFlag { get; private set; }

    // Navigation
    public ICollection<UnitProcessMap> UnitProcessMaps { get; private set; } = [];
    public ICollection<UnitLovAccessMaster> AccessMasters { get; private set; } = [];

    private ProcessMaster() { }

    public static ProcessMaster Create(decimal processId, string processName, string liveFlag = "Y")
    {
        var process = new ProcessMaster
        {
            ProcessId = processId,
            ProcessName = processName,
            ProcessLivFlag = liveFlag
        };

        process.AddDomainEvent(new ProcessCreatedEvent(processId, processName));
        return process;
    }

    public void Update(string processName, string liveFlag)
    {
        ProcessName = processName;
        ProcessLivFlag = liveFlag;
        AddDomainEvent(new ProcessUpdatedEvent(ProcessId, processName));
    }

    public void Deactivate()
    {
        ProcessLivFlag = "N";
    }
}
