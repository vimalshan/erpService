namespace CSA.Service.Domain.Entities;

public class CsaMainUploadErr
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ControlType { get; set; }
    public string? ControlMethod { get; set; }
    public string? Risk { get; set; }
    public string? Priority { get; set; }
    public long? ProcessId { get; set; }
    public long? SubProcessId { get; set; }
    public string? Periodicity { get; set; }
    public string? EvidenceFlag { get; set; }
    public string? Evidence { get; set; }
    public string? ApproverFlag { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SessionId { get; set; }
}
