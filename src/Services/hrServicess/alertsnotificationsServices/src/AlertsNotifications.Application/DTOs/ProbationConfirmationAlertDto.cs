namespace AlertsNotifications.Application.DTOs;

public class ProbationConfirmationAlertDto
{
    public long ProbationId { get; set; }
    public long ProbationEmpSysId { get; set; }
    public long ProbationGrade { get; set; }
    public DateTime ProbationDate { get; set; }
    public char? SelfAppraisal { get; set; }
    public DateTime? AlertSentOn { get; set; }
}
