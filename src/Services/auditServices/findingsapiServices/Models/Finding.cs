// Models/Finding.cs
namespace FindingsAPI.Gateway
{
    public class Finding
    {
        public int FindingId { get; set; }
        public string FindingNumber { get; set; }
        public int AuditId { get; set; }
        public int? SiteId { get; set; }
        public int CompanyId { get; set; }  // from JOIN, not a DB column
        public string Title { get; set; }
        public string Description { get; set; }
        public string FindingType { get; set; }
        public string Severity { get; set; }
        public int FindingStatusId { get; set; }
        public int? FindingCategoryId { get; set; }
        public string Status { get; set; }  // from JOIN to FindingStatuses
        public string Category { get; set; }  // from JOIN to FindingCategories
        public DateTime IdentifiedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IdentifiedBy { get; set; }
        public int? AssignedTo { get; set; }
        public string Response { get; set; }
        public string ClosureNotes { get; set; }
        public string ClosedBy { get; set; }
        // Not DB columns — for display/compatibility only
        public DateTime? OpenDate { get; set; }
        public List<int> Services { get; set; }
        public Company Company { get; set; }
        public Site Site { get; set; }
    }
}