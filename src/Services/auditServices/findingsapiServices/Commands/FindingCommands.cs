// Commands/FindingCommands.cs
namespace FindingsAPI.Gateway
{
    public class CreateFindingCommand
    {
        public string FindingNumber { get; set; }
        public int AuditId { get; set; }
        public int? SiteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FindingType { get; set; }
        public string Severity { get; set; }
        public int FindingStatusId { get; set; }
        public int? FindingCategoryId { get; set; }
        public DateTime IdentifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IdentifiedBy { get; set; }
        public List<int> Services { get; set; } = new();
    }

    public class UpdateFindingCommand
    {
        public int FindingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FindingType { get; set; }
        public string Severity { get; set; }
        public int FindingStatusId { get; set; }
        public int? FindingCategoryId { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class CloseFindingCommand
    {
        public int FindingId { get; set; }
        public int? ClosedBy { get; set; }
    }

    public class BulkUpdateStatusCommand
    {
        public List<int> FindingIds { get; set; }
        public int FindingStatusId { get; set; }
        public string Reason { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class BulkUpdateResult
    {
        public int UpdatedCount { get; set; }
        public int FailedCount { get; set; }
        public List<int> FailedIds { get; set; }
    }
}