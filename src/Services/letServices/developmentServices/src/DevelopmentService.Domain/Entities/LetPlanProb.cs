namespace DevelopmentService.Domain.Entities;

public class LetPlanProb
{
    public long? ReqNum { get; set; }
    public long? Sno { get; set; }
    public string? UserId { get; set; }
    public long? PinNum { get; set; }
    public string? DevSource { get; set; }
    public string? DevNeed { get; set; }
    public string? DevIndicator { get; set; }
    public long? DevMode { get; set; }
    public string? RecProg { get; set; }
    public string? TrainingProgram { get; set; }
    public long? InternalTraining { get; set; }
    public string? RevDate { get; set; }
    public long? Priority { get; set; }
    public DateTime? EntDate { get; set; }
    public char? AppStatus { get; set; }
    public char? BhrStatus { get; set; }
    public DateTime? StrDate { get; set; }
    public DateTime? EnDate { get; set; }
}
