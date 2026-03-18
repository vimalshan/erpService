using FinyearAPI.Domain.Events;
using FinyearAPI.Domain.ValueObjects;

namespace FinyearAPI.Domain.Entities
{
    /// <summary>
    /// Aggregate Root for Financial Year
    /// Implements DDD aggregate pattern
    /// Encapsulates business logic and domain rules
    /// </summary>
    public class FinancialYearAggregate : Entity
    {
        // Required for EF Core
        private FinancialYearAggregate() { }

        /// <summary>
        /// Financial year name (2024-2025)
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Date range value object
        /// </summary>
        public DateRange Period { get; private set; } = null!;

        /// <summary>
        /// Status of the financial year
        /// </summary>
        public FinancialYearStatus Status { get; private set; }

        /// <summary>
        /// User who last updated this aggregate
        /// </summary>
        public long UpdatedBy { get; private set; }

        /// <summary>
        /// When it was last updated
        /// </summary>
        public DateTime UpdatedOn { get; private set; }

        /// <summary>
        /// Factory method to create a new financial year
        /// </summary>
        public static FinancialYearAggregate Create(
            long id,
            string name,
            DateTime startDate,
            DateTime endDate,
            long updatedBy)
        {
            // Business rules validation
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Financial year name cannot be empty");

            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date");

            var aggregate = new FinancialYearAggregate
            {
                Id = id,
                Name = name,
                Period = DateRange.Create(startDate, endDate),
                Status = FinancialYearStatus.Open,
                UpdatedBy = updatedBy,
                UpdatedOn = DateTime.UtcNow
            };

            // Raise domain event
            aggregate.AddDomainEvent(new FinancialYearCreatedEvent(
                aggregate.Id,
                aggregate.Name,
                aggregate.Period.StartDate,
                aggregate.Period.EndDate,
                updatedBy));

            return aggregate;
        }

        /// <summary>
        /// Update the financial year details
        /// </summary>
        public void Update(string name, DateTime startDate, DateTime endDate, long updatedBy)
        {
            if (Status == FinancialYearStatus.Closed)
                throw new ApplicationException("Cannot update a closed financial year");

            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date");

            Name = name;
            Period = DateRange.Create(startDate, endDate);
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new FinancialYearUpdatedEvent(
                Id, Name, startDate, endDate, updatedBy));
        }

        /// <summary>
        /// Close the financial year
        /// </summary>
        public void Close(long closedBy)
        {
            if (Status == FinancialYearStatus.Closed)
                throw new ApplicationException("Financial year is already closed");

            Status = FinancialYearStatus.Closed;
            UpdatedBy = closedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new FinancialYearClosedEvent(Id, closedBy));
        }

        /// <summary>
        /// Check if the financial year is currently active
        /// </summary>
        public bool IsActive
        {
            get
            {
                var now = DateTime.UtcNow;
                return Status == FinancialYearStatus.Open &&
                       Period.StartDate <= now &&
                       Period.EndDate >= now;
            }
        }

        /// <summary>
        /// Get duration of financial year in days
        /// </summary>
        public int DurationInDays => (int)(Period.EndDate - Period.StartDate).TotalDays;
    }

    /// <summary>
    /// Financial Year Status enumeration
    /// </summary>
    public enum FinancialYearStatus
    {
        Open = 1,
        Closed = 2,
        Suspended = 3
    }
}
