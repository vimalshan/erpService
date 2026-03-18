using FinyearAPI.Domain.Entities;

namespace FinyearAPI.Domain.Events
{
    /// <summary>
    /// Domain event raised when a financial year is created
    /// </summary>
    public class FinancialYearCreatedEvent : DomainEvent
    {
        public long FinancialYearId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedBy { get; set; }

        public FinancialYearCreatedEvent(long id, string name, DateTime start, DateTime end, long createdBy)
        {
            FinancialYearId = id;
            Name = name;
            StartDate = start;
            EndDate = end;
            CreatedBy = createdBy;
        }
    }

    /// <summary>
    /// Domain event raised when a financial year is updated
    /// </summary>
    public class FinancialYearUpdatedEvent : DomainEvent
    {
        public long FinancialYearId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UpdatedBy { get; set; }

        public FinancialYearUpdatedEvent(long id, string name, DateTime start, DateTime end, long updatedBy)
        {
            FinancialYearId = id;
            Name = name;
            StartDate = start;
            EndDate = end;
            UpdatedBy = updatedBy;
        }
    }

    /// <summary>
    /// Domain event raised when a financial year is closed
    /// </summary>
    public class FinancialYearClosedEvent : DomainEvent
    {
        public long FinancialYearId { get; set; }
        public long ClosedBy { get; set; }

        public FinancialYearClosedEvent(long id, long closedBy)
        {
            FinancialYearId = id;
            ClosedBy = closedBy;
        }
    }
}
