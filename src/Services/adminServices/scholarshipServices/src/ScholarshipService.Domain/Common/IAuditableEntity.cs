namespace ScholarshipService.Domain.Common;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; }
    long CreatedBy { get; }
    DateTime? UpdatedOn { get; }
    long? UpdatedBy { get; }
}
