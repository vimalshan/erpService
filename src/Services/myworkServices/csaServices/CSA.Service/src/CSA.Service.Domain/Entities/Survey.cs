using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class Survey : AuditableEntity
{
    public long SurveyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime CloseDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long? Alert1 { get; set; }
    public long? Alert2 { get; set; }

    // Navigation
    public ICollection<SurveyQuestion> Questions { get; set; } = [];
}
