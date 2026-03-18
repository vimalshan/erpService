namespace ObjectiveService.Domain.ValueObjects;

/// <summary>
/// Value object representing a period (from/to date range).
/// </summary>
public sealed class Period : IEquatable<Period>
{
    public DateTime From { get; }
    public DateTime To { get; }

    public Period(DateTime from, DateTime to)
    {
        if (to < from)
            throw new ArgumentException("Period 'To' date must be greater than or equal to 'From' date.");

        From = from;
        To = to;
    }

    public bool Contains(DateTime date) => date >= From && date <= To;
    public TimeSpan Duration => To - From;

    public bool Overlaps(Period other) => From < other.To && To > other.From;

    public bool Equals(Period? other) =>
        other is not null && From == other.From && To == other.To;

    public override bool Equals(object? obj) => Equals(obj as Period);
    public override int GetHashCode() => HashCode.Combine(From, To);
    public override string ToString() => $"{From:yyyy-MM-dd} → {To:yyyy-MM-dd}";

    public static bool operator ==(Period? left, Period? right) =>
        left?.Equals(right) ?? right is null;
    public static bool operator !=(Period? left, Period? right) => !(left == right);
}

/// <summary>
/// Value object for an employee identifier (userId + pin combination).
/// </summary>
public sealed class EmployeeId : IEquatable<EmployeeId>
{
    public string UserId { get; }
    public decimal PinNumber { get; }

    public EmployeeId(string userId, decimal pinNumber)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        if (pinNumber <= 0)
            throw new ArgumentException("PinNumber must be positive.", nameof(pinNumber));

        UserId = userId;
        PinNumber = pinNumber;
    }

    public bool Equals(EmployeeId? other) =>
        other is not null && UserId == other.UserId && PinNumber == other.PinNumber;

    public override bool Equals(object? obj) => Equals(obj as EmployeeId);
    public override int GetHashCode() => HashCode.Combine(UserId, PinNumber);
    public override string ToString() => $"{UserId}/{PinNumber}";

    public static bool operator ==(EmployeeId? left, EmployeeId? right) =>
        left?.Equals(right) ?? right is null;
    public static bool operator !=(EmployeeId? left, EmployeeId? right) => !(left == right);
}

/// <summary>
/// Value object for a control-point measurement unit range.
/// </summary>
public sealed class MeasurementRange : IEquatable<MeasurementRange>
{
    public string Unit { get; }
    public string From { get; }
    public string To { get; }

    public MeasurementRange(string unit, string from, string to)
    {
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.", nameof(unit));

        Unit = unit;
        From = from ?? string.Empty;
        To = to ?? string.Empty;
    }

    public bool Equals(MeasurementRange? other) =>
        other is not null && Unit == other.Unit && From == other.From && To == other.To;

    public override bool Equals(object? obj) => Equals(obj as MeasurementRange);
    public override int GetHashCode() => HashCode.Combine(Unit, From, To);
    public override string ToString() => $"{From} – {To} {Unit}";

    public static bool operator ==(MeasurementRange? left, MeasurementRange? right) =>
        left?.Equals(right) ?? right is null;
    public static bool operator !=(MeasurementRange? left, MeasurementRange? right) => !(left == right);
}
