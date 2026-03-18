using CompetencyService.Domain.Common;
using CompetencyService.Domain.Exceptions;
using CompetencyService.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyService.Domain.Entities;

/// <summary>
/// Maps to DD_COMPENDMAST — the master competency record.
/// </summary>
public class CompetencyMaster : AuditableEntity
{
    public decimal Id { get; private set; }           // CM_CPD_NUM
    public string Name { get; private set; } = default!;    // CM_CPD_NAM
    public DateTime EffectiveDate { get; private set; }     // CM_EFF_DAT
    public DateTime? ClosureDate { get; private set; }      // CM_CLS_DAT
    public string? Remarks { get; private set; }            // CM_CPD_REM
    public decimal? JobCode { get; private set; }           // CM_JOB_COD
    public string? PositiveIndicator { get; private set; }  // CM_POS_IND
    public string? NegativeIndicator { get; private set; }  // CM_NEG_IND
    public string? SelfDescription { get; private set; }    // CM_CPD_SLF
    public string? CompetencyType { get; private set; }     // CM_CPD_TYPE
    public decimal? ParentId { get; private set; }          // CM_PARENTID

    // Navigation (not a DB FK — loaded separately via Dapper or EF queries)
    [NotMapped]
    private readonly List<CompetencyIndicator> _indicators = new();
    [NotMapped]
    public IReadOnlyCollection<CompetencyIndicator> Indicators => _indicators.AsReadOnly();

    private CompetencyMaster() { }

    public static CompetencyMaster Create(
        decimal id, string name, DateTime effectiveDate,
        string? type = null, decimal? parentId = null,
        string? remarks = null, decimal? jobCode = null,
        string? positiveInd = null, string? negativeInd = null,
        string? selfDesc = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CompetencyDomainException("Competency name cannot be empty.");

        var entity = new CompetencyMaster
        {
            Id = id,
            Name = name,
            EffectiveDate = effectiveDate,
            CompetencyType = type,
            ParentId = parentId,
            Remarks = remarks,
            JobCode = jobCode,
            PositiveIndicator = positiveInd,
            NegativeIndicator = negativeInd,
            SelfDescription = selfDesc
        };

        entity.AddDomainEvent(new CompetencyCreatedEvent(entity.Id, entity.Name));
        return entity;
    }

    public void Update(string name, DateTime effectiveDate, DateTime? closureDate,
        string? remarks, string? type, decimal? modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CompetencyDomainException("Competency name cannot be empty.");

        Name = name;
        EffectiveDate = effectiveDate;
        ClosureDate = closureDate;
        Remarks = remarks;
        CompetencyType = type;
        SetAudit(modifiedBy);

        AddDomainEvent(new CompetencyUpdatedEvent(Id, Name));
    }

    public void Close(DateTime closureDate, decimal? modifiedBy)
    {
        if (closureDate < EffectiveDate)
            throw new CompetencyDomainException("Closure date cannot be before effective date.");
        ClosureDate = closureDate;
        SetAudit(modifiedBy);
        AddDomainEvent(new CompetencyClosedEvent(Id, closureDate));
    }
}
