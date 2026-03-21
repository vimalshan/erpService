namespace ExpenseService.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date.");

        StartDate = startDate;
        EndDate = endDate;
    }

    public int TotalDays => (EndDate - StartDate).Days + 1;
    public int TotalHours => TotalDays * 24;
}
