namespace FinyearAPI.Domain.ValueObjects
{
    /// <summary>
    /// Date Range value object
    /// Represents an immutable period of time
    /// </summary>
    public class DateRange : ValueObject
    {
        /// <summary>
        /// Start date of the range
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// End date of the range
        /// </summary>
        public DateTime EndDate { get; private set; }

        private DateRange() { }

        /// <summary>
        /// Factory method to create a date range
        /// </summary>
        public static DateRange Create(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date");

            return new DateRange { StartDate = startDate, EndDate = endDate };
        }

        /// <summary>
        /// Get duration in days
        /// </summary>
        public int LengthInDays => (int)(EndDate - StartDate).TotalDays;

        /// <summary>
        /// Check if the date range contains the given date
        /// </summary>
        public bool Contains(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }

        /// <summary>
        /// Check if the date range overlaps with another range
        /// </summary>
        public bool OverlapsWith(DateRange other)
        {
            return StartDate <= other.EndDate && EndDate >= other.StartDate;
        }

        /// <summary>
        /// Get equality components for value comparison
        /// </summary>
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}";
        }
    }
}
