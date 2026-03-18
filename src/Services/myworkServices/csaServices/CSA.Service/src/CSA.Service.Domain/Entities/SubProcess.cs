using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class SubProcess : AuditableEntity
{
    public long SubProcessId { get; set; }
    public long ProcessId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public Process? Process { get; set; }
    public ICollection<Control> Controls { get; set; } = [];
}
