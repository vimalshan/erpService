using CompetencyService.Domain.Common;
using CompetencyService.Domain.Exceptions;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to COMPETENCY_RATING_SCALE.</summary>
public class CompetencyRatingScale : AuditableEntity
{
    public decimal CompetencyId { get; private set; }
    public string R1Desc { get; private set; } = default!;
    public string? R2Desc { get; private set; }
    public string R3Desc { get; private set; } = default!;
    public string? R4Desc { get; private set; }
    public string R5Desc { get; private set; } = default!;

    private CompetencyRatingScale() { }

    public static CompetencyRatingScale Create(
        decimal competencyId, string r1, string r3, string r5,
        string? r2 = null, string? r4 = null)
    {
        if (string.IsNullOrWhiteSpace(r1)) throw new CompetencyDomainException("R1 description is required.");
        if (string.IsNullOrWhiteSpace(r3)) throw new CompetencyDomainException("R3 description is required.");
        if (string.IsNullOrWhiteSpace(r5)) throw new CompetencyDomainException("R5 description is required.");

        return new CompetencyRatingScale
        {
            CompetencyId = competencyId,
            R1Desc = r1, R2Desc = r2, R3Desc = r3, R4Desc = r4, R5Desc = r5
        };
    }

    public void Update(string r1, string r3, string r5, string? r2, string? r4, decimal? modifiedBy)
    {
        R1Desc = r1; R2Desc = r2; R3Desc = r3; R4Desc = r4; R5Desc = r5;
        SetAudit(modifiedBy);
    }
}
