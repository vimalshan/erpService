using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Events;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeePromotion : BaseEntity
{
    public long PromotionNo { get; private set; }
    public string Source { get; private set; } = string.Empty;  // ADD/MYR/DIR/PRB
    public long RequestNo { get; private set; }
    public DateTime RecommendationDate { get; private set; }
    public long EmployeeId { get; private set; }
    public long OldGradeId { get; private set; }
    public long NewGradeId { get; private set; }
    public char Status { get; private set; }
    public long OldPositionId { get; private set; }
    public long NewPositionId { get; private set; }
    public long ReasonId { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime? ConfirmationDate { get; private set; }
    public char RevisionStatus { get; private set; }
    public long IncrementNo { get; private set; }
    public string? Designation { get; private set; }
    public char? PromotionType { get; private set; }  // P = Promotion, G = Grade change
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }

    private EmployeePromotion() { }

    public static EmployeePromotion Create(long promotionNo, string source, long requestNo,
        long employeeId, long oldGradeId, long newGradeId, long oldPositionId, long newPositionId,
        long reasonId, string? remarks, long incrementNo, string? designation, char? type, long createdBy)
    {
        return new EmployeePromotion
        {
            PromotionNo = promotionNo, Source = source, RequestNo = requestNo,
            RecommendationDate = DateTime.UtcNow, EmployeeId = employeeId,
            OldGradeId = oldGradeId, NewGradeId = newGradeId, Status = 'P',
            OldPositionId = oldPositionId, NewPositionId = newPositionId,
            ReasonId = reasonId, Remarks = remarks, RevisionStatus = 'N',
            IncrementNo = incrementNo, Designation = designation, PromotionType = type,
            CreatedBy = createdBy, CreatedOn = DateTime.UtcNow
        };
    }
}
