namespace CSA.Service.Domain.ValueObjects;

public record ControlIdentifier(long ControlId);
public record SurveyIdentifier(long SurveyId);
public record ProcessIdentifier(long ProcessId);
public record UnitIdentifier(long UnitId);
public record EmployeeIdentifier(long EmployeeSysId);

public record DateRange(DateTime StartDate, DateTime EndDate)
{
    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;
}

public record UnitCode
{
    public string Value { get; }

    public UnitCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Unit code must be 1-3 characters.", nameof(value));
        Value = value.ToUpperInvariant();
    }
}
