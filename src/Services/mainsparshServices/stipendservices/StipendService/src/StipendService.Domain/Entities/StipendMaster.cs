using StipendService.Domain.Common;
using StipendService.Domain.Events;
using StipendService.Domain.Exceptions;
using StipendService.Domain.ValueObjects;

namespace StipendService.Domain.Entities;

/// <summary>
/// Aggregate Root: SRF Stipend Master - holds the monthly stipend rate for a given research category and rank.
/// </summary>
public sealed class StipendMaster : AuditableEntity
{
    public long ResearchCategoryId { get; private set; }
    public long SrfRankId { get; private set; }
    public decimal SrfMonthlyStipend { get; private set; }
    public decimal? AdditionalAllowance { get; private set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public string Status { get; private set; } = "A";

    private readonly List<StipendDisbursement> _disbursements = new();
    public IReadOnlyCollection<StipendDisbursement> Disbursements => _disbursements.AsReadOnly();

    private StipendMaster() { }

    public static StipendMaster Create(
        long researchCategoryId,
        long srfRankId,
        decimal monthlyStipend,
        decimal? additionalAllowance,
        DateTime effectiveFrom,
        DateTime? effectiveTo,
        long createdBy)
    {
        if (researchCategoryId <= 0) throw new DomainException("ResearchCategoryId must be positive.");
        if (srfRankId <= 0) throw new DomainException("SrfRankId must be positive.");
        if (monthlyStipend < 0) throw new DomainException("Monthly stipend cannot be negative.");
        if (effectiveTo.HasValue && effectiveTo <= effectiveFrom)
            throw new DomainException("EffectiveTo must be after EffectiveFrom.");

        var master = new StipendMaster
        {
            ResearchCategoryId = researchCategoryId,
            SrfRankId = srfRankId,
            SrfMonthlyStipend = monthlyStipend,
            AdditionalAllowance = additionalAllowance,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            Status = "A",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        master.AddDomainEvent(new StipendMasterCreatedEvent(master));
        return master;
    }

    public void Update(
        decimal monthlyStipend,
        decimal? additionalAllowance,
        DateTime effectiveFrom,
        DateTime? effectiveTo,
        long updatedBy)
    {
        if (monthlyStipend < 0) throw new DomainException("Monthly stipend cannot be negative.");
        if (effectiveTo.HasValue && effectiveTo <= effectiveFrom)
            throw new DomainException("EffectiveTo must be after EffectiveFrom.");

        SrfMonthlyStipend = monthlyStipend;
        AdditionalAllowance = additionalAllowance;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new StipendMasterUpdatedEvent(this));
    }

    public void Deactivate(long updatedBy)
    {
        if (Status == "I") throw new DomainException("Stipend master is already inactive.");
        Status = "I";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new StipendMasterDeactivatedEvent(this));
    }

    public bool IsActiveOn(DateTime date) =>
        Status == "A" && EffectiveFrom <= date && (EffectiveTo == null || EffectiveTo >= date);

    public decimal TotalStipend() => SrfMonthlyStipend + (AdditionalAllowance ?? 0m);
}
