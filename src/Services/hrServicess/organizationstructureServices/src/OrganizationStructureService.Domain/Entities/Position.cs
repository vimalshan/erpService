using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.Events;

namespace OrganizationStructureService.Domain.Entities;

public class Position : AggregateRoot
{
    public decimal PositionId { get; private set; }
    public string PosUnitCode { get; private set; } = string.Empty;
    public decimal PosGradeId { get; private set; }
    public string? PositionName { get; private set; }
    public string PositionDesignation { get; private set; } = string.Empty;
    public DateTime PosEffectiveDate { get; private set; }
    public DateTime? PosClosedDate { get; private set; }
    public string ReferenceCode { get; private set; } = string.Empty;
    public string? DeletedFlag { get; private set; }
    public decimal? PositionJdId { get; private set; }
    public DateTime EnteredDate { get; private set; }
    public decimal EnteredPinNo { get; private set; }
    public decimal? Ctc { get; private set; }
    public decimal? ProcessId { get; private set; }
    public decimal? ReasonId { get; private set; }
    public decimal? ReplacePositionId { get; private set; }
    public decimal? PosModifiedBy { get; private set; }
    public DateTime? PosModifiedOn { get; private set; }
    public decimal? PosUnitId { get; private set; }
    public long? PosRefNo { get; private set; }
    public long PosEvaluatedGradeId { get; private set; }
    public string PositionEvaluatedDesignation { get; private set; } = string.Empty;

    private Position() { }

    public static Position Create(
        decimal positionId,
        string unitCode,
        decimal gradeId,
        string designation,
        DateTime effectiveDate,
        string referenceCode,
        decimal enteredBy)
    {
        var position = new Position
        {
            PositionId = positionId,
            PosUnitCode = unitCode,
            PosGradeId = gradeId,
            PositionDesignation = designation,
            PosEffectiveDate = effectiveDate,
            ReferenceCode = referenceCode,
            DeletedFlag = "N",
            EnteredDate = DateTime.UtcNow,
            EnteredPinNo = enteredBy,
            PosEvaluatedGradeId = (long)gradeId,
            PositionEvaluatedDesignation = designation
        };
        position.RaiseDomainEvent(new PositionCreatedEvent(positionId, designation, gradeId));
        position.IncrementVersion();
        return position;
    }

    public void Close(DateTime closeDate, decimal modifiedBy)
    {
        PosClosedDate = closeDate;
        PosModifiedBy = modifiedBy;
        PosModifiedOn = DateTime.UtcNow;
        IncrementVersion();
    }

    public void MarkDeleted(decimal modifiedBy)
    {
        DeletedFlag = "Y";
        PosModifiedBy = modifiedBy;
        PosModifiedOn = DateTime.UtcNow;
        IncrementVersion();
    }
}
