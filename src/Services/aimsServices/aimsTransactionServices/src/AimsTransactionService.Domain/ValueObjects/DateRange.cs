namespace AimsTransactionService.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateRange(DateTime start, DateTime end)
    {
        if (start > end) throw new ArgumentException("Start date cannot be after end date.");
        Start = start;
        End = end;
    }

    public int TotalDays => (End.Date - Start.Date).Days + 1;
}
