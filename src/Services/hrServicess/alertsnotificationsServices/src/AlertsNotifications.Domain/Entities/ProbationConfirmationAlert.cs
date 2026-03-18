using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class ProbationConfirmationAlert : BaseEntity
{
    public long ProbationId { get; set; }
    public long ProbationEmpSysId { get; set; }
    public long ProbationGrade { get; set; }
    public DateTime ProbationDate { get; set; }
    public char? SelfAppraisal { get; set; }
    public DateTime? AlertSentOn { get; set; }
}
