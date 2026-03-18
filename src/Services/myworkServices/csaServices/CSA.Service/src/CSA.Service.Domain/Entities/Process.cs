using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class Process : AuditableEntity
{
    public long ProcessId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<SubProcess> SubProcesses { get; set; } = [];
    public ICollection<Control> Controls { get; set; } = [];
}
