using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class Control : AuditableEntity
{
    public long ControlId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public char? ControlType { get; set; }
    public char? ControlMethod { get; set; }
    public string? Risk { get; set; }
    public char? Priority { get; set; }
    public long? ProcessId { get; set; }
    public long? SubProcessId { get; set; }
    public char? Periodicity { get; set; }
    public char? EvidenceFlag { get; set; }
    public char? ApproverFlag { get; set; }

    // Navigation properties
    public Process? Process { get; set; }
    public SubProcess? SubProcess { get; set; }
    public ICollection<Evidence> Evidences { get; set; } = [];
    public ICollection<UnitMapDetail> UnitMappings { get; set; } = [];
    public ICollection<SurveyQuestion> SurveyQuestions { get; set; } = [];
}
