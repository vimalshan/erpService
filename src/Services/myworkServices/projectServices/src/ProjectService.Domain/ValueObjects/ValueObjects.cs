namespace ProjectService.Domain.ValueObjects;

public record DateRange
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

    public int DurationInDays => (EndDate - StartDate).Days;
}

public record CharterNumber
{
    public decimal Value { get; init; }

    public CharterNumber(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Charter number must be non-negative.");
        Value = value;
    }
}

public record FileReference
{
    public string FilePath { get; init; }

    public FileReference(string filePath)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }
}
