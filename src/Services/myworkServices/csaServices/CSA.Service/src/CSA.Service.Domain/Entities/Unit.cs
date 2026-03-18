using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class Unit : AuditableEntity
{
    public long UnitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long BusinessId { get; set; }
    public char LiveFlag { get; set; }
    public long OrgId { get; set; }

    // Navigation
    public ICollection<UnitMapDetail> UnitMappings { get; set; } = [];
    public ICollection<SurveyQuestion> SurveyQuestions { get; set; } = [];
}
