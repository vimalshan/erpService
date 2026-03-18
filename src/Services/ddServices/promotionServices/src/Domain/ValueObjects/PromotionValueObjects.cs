namespace PromotionService.Domain.ValueObjects;

/// <summary>Rating grade value object (A/B/C/D)</summary>
public sealed class RatingGrade : IEquatable<RatingGrade>
{
    public static readonly RatingGrade A = new("A", "Exceptional");
    public static readonly RatingGrade B = new("B", "High Performer");
    public static readonly RatingGrade C = new("C", "Normal Performer");
    public static readonly RatingGrade D = new("D", "Below Expectations");

    public string Code { get; }
    public string Label { get; }

    private RatingGrade(string code, string label) { Code = code; Label = label; }

    public static RatingGrade FromScore(decimal score) => score switch
    {
        >= 4.5m => A,
        >= 3.5m => B,
        >= 2.5m => C,
        _ => D
    };

    public static RatingGrade FromCode(string code) => code.ToUpperInvariant() switch
    {
        "A" => A,
        "B" => B,
        "C" => C,
        _ => D
    };

    public bool Equals(RatingGrade? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is RatingGrade g && Equals(g);
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => Code;
    public static implicit operator string(RatingGrade g) => g.Code;
}

/// <summary>Money value object – amount + currency</summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "INR")
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.");
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency) throw new InvalidOperationException("Currency mismatch.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency) throw new InvalidOperationException("Currency mismatch.");
        return new Money(Amount - other.Amount, Currency);
    }

    public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Currency} {Amount:N2}";
}

/// <summary>Promotion period value object</summary>
public sealed class PromotionPeriodValue : IEquatable<PromotionPeriodValue>
{
    public DateTime From { get; }
    public DateTime To { get; }

    public PromotionPeriodValue(DateTime from, DateTime to)
    {
        if (to <= from) throw new ArgumentException("Period 'To' must be after 'From'.");
        From = from;
        To = to;
    }

    public bool Contains(DateTime date) => date >= From && date <= To;
    public int DaysInPeriod => (int)(To - From).TotalDays;

    public bool Equals(PromotionPeriodValue? other) => other is not null && From == other.From && To == other.To;
    public override bool Equals(object? obj) => obj is PromotionPeriodValue p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(From, To);
    public override string ToString() => $"{From:yyyy-MM-dd} to {To:yyyy-MM-dd}";
}

/// <summary>Grade + Band identifier value object</summary>
public sealed class GradeBand : IEquatable<GradeBand>
{
    public string GradeCode { get; }
    public decimal GradeId { get; }
    public decimal? BandId { get; }

    public GradeBand(string gradeCode, decimal gradeId, decimal? bandId = null)
    {
        if (string.IsNullOrWhiteSpace(gradeCode)) throw new ArgumentException("GradeCode required.");
        GradeCode = gradeCode;
        GradeId = gradeId;
        BandId = bandId;
    }

    public bool Equals(GradeBand? other) =>
        other is not null && GradeCode == other.GradeCode && GradeId == other.GradeId;
    public override bool Equals(object? obj) => obj is GradeBand g && Equals(g);
    public override int GetHashCode() => HashCode.Combine(GradeCode, GradeId);
    public override string ToString() => $"{GradeCode} ({GradeId})";
}
