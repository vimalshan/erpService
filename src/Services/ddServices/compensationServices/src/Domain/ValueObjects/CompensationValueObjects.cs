namespace CompensationService.Domain.ValueObjects;

/// <summary>
/// Represents monetary value.
/// </summary>
public record MoneyAmount
{
    /// <summary>Gets the amount value.</summary>
    public decimal Amount { get; init; }

    private MoneyAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        Amount = amount;
    }

    /// <summary>Creates a new money amount.</summary>
    public static MoneyAmount Create(decimal amount) => new(amount);

    /// <summary>Adds two money amounts.</summary>
    public static MoneyAmount operator +(MoneyAmount left, MoneyAmount right) =>
        Create(left.Amount + right.Amount);

    /// <summary>Subtracts two money amounts.</summary>
    public static MoneyAmount operator -(MoneyAmount left, MoneyAmount right) =>
        Create(left.Amount - right.Amount);
}

/// <summary>
/// Represents a compensation level range.
/// </summary>
public record LevelRange
{
    /// <summary>Gets the minimum amount.</summary>
    public decimal MinAmount { get; init; }

    /// <summary>Gets the maximum amount.</summary>
    public decimal MaxAmount { get; init; }

    private LevelRange(decimal minAmount, decimal maxAmount)
    {
        if (minAmount < 0 || maxAmount < 0)
            throw new ArgumentException("Amounts cannot be negative.");

        if (minAmount > maxAmount)
            throw new ArgumentException("Minimum amount cannot be greater than maximum amount.");

        MinAmount = minAmount;
        MaxAmount = maxAmount;
    }

    /// <summary>Creates a new level range.</summary>
    public static LevelRange Create(decimal minAmount, decimal maxAmount) =>
        new(minAmount, maxAmount);

    /// <summary>Determines if an amount is within the range.</summary>
    public bool IsWithinRange(decimal amount) =>
        amount >= MinAmount && amount <= MaxAmount;
}

/// <summary>
/// Represents a recommendation status.
/// </summary>
public record RecommendationStatus
{
    public const int PendingCode = 1;
    public const int AppraisalSubmittedCode = 2;
    public const int ReviewerSubmittedCode = 3;
    public const int BhrSubmittedCode = 4;
    public const int ChrSubmittedCode = 5;
    public const int RejectedCode = 6;
    public const int ApprovedCode = 7;

    /// <summary>Gets the status code.</summary>
    public int StatusCode { get; private set; }

    /// <summary>Gets the status description.</summary>
    public string Description { get; private set; }

    private RecommendationStatus(int statusCode, string description)
    {
        StatusCode = statusCode;
        Description = description;
    }

    /// <summary>Creates a pending recommendation status.</summary>
    public static RecommendationStatus Pending() =>
        new(1, "Pending");

    /// <summary>Creates an appraisal submitted status.</summary>
    public static RecommendationStatus AppraisalSubmitted() =>
        new(2, "Appraisal Submitted");

    /// <summary>Creates a reviewer submitted status.</summary>
    public static RecommendationStatus ReviewerSubmitted() =>
        new(3, "Reviewer Submitted");

    /// <summary>Creates a BHR submitted status.</summary>
    public static RecommendationStatus BhrSubmitted() =>
        new(4, "BHR Submitted");

    /// <summary>Creates a CHR submitted status.</summary>
    public static RecommendationStatus ChrSubmitted() =>
        new(5, "CHR Submitted");

    /// <summary>Creates a rejected status.</summary>
    public static RecommendationStatus Rejected() =>
        new(6, "Rejected");

    /// <summary>Creates an approved status.</summary>
    public static RecommendationStatus Approved() =>
        new(7, "Approved");

    /// <summary>Creates a status from code.</summary>
    public static RecommendationStatus FromCode(int code) =>
        code switch
        {
            1 => Pending(),
            2 => AppraisalSubmitted(),
            3 => ReviewerSubmitted(),
            4 => BhrSubmitted(),
            5 => ChrSubmitted(),
            6 => Rejected(),
            7 => Approved(),
            _ => throw new ArgumentException($"Invalid status code: {code}")
        };
}

/// <summary>
/// Represents a period status.
/// </summary>
public record PeriodStatus
{
    public const string OpenCode = "O";
    public const string CircularGeneratedCode = "C";
    public const string ConfirmedToPayrollCode = "P";

    /// <summary>Gets the status code.</summary>
    public string StatusCode { get; private set; }

    /// <summary>Gets the status description.</summary>
    public string Description { get; private set; }

    private PeriodStatus(string statusCode, string description)
    {
        StatusCode = statusCode;
        Description = description;
    }

    /// <summary>Creates an open period status.</summary>
    public static PeriodStatus Open() =>
        new("O", "Open");

    /// <summary>Creates a circular generated status.</summary>
    public static PeriodStatus CircularGenerated() =>
        new("C", "Circular Generated");

    /// <summary>Creates a confirmed to payroll status.</summary>
    public static PeriodStatus ConfirmedToPayroll() =>
        new("P", "Confirmed to Payroll");

    /// <summary>Creates a status from code.</summary>
    public static PeriodStatus FromCode(string code) =>
        code switch
        {
            "O" => Open(),
            "C" => CircularGenerated(),
            "P" => ConfirmedToPayroll(),
            _ => throw new ArgumentException($"Invalid status code: {code}")
        };
}
