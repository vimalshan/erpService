using System.Collections.ObjectModel;
using Ardalis.GuardClauses;
using LoanAccount.Domain.Common;

namespace LoanAccount.Domain.ValueObjects;

/// <summary>
/// Money value object for representing currency amounts
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        Guard.Against.Negative(amount, nameof(amount));
        Amount = amount;
    }

    protected override bool EqualsCore(ValueObject other) =>
        other is Money money && Amount == money.Amount;

    public override int GetHashCode() => Amount.GetHashCode();
}

/// <summary>
/// Interest rate value object
/// </summary>
public sealed class InterestRate : ValueObject
{
    public decimal Rate { get; }

    public InterestRate(decimal rate)
    {
        Guard.Against.OutOfRange(rate, nameof(rate), 0, 100);
        Rate = rate;
    }

    protected override bool EqualsCore(ValueObject other) =>
        other is InterestRate ir && Rate == ir.Rate;

    public override int GetHashCode() => Rate.GetHashCode();
}

/// <summary>
/// Loan status value object
/// </summary>
public sealed class LoanStatus : ValueObject
{
    public string Status { get; }

    private LoanStatus(string status) => Status = status;

    public static readonly LoanStatus Active = new("Active");
    public static readonly LoanStatus Closed = new("Closed");
    public static readonly LoanStatus Discontinued = new("Discontinued");
    public static readonly LoanStatus WrittenOff = new("WrittenOff");
    public static readonly LoanStatus Adjusted = new("Adjusted");

    public static LoanStatus Create(string status) =>
        status.ToLower() switch
        {
            "active" => Active,
            "closed" => Closed,
            "discontinued" => Discontinued,
            "written_off" => WrittenOff,
            "adjusted" => Adjusted,
            _ => throw new ArgumentException($"Invalid loan status: {status}")
        };

    protected override bool EqualsCore(ValueObject other) =>
        other is LoanStatus ls && Status == ls.Status;

    public override int GetHashCode() => Status.GetHashCode();
}

/// <summary>
/// Disbursement type value object
/// </summary>
public sealed class DisbursementType : ValueObject
{
    public string Type { get; }

    private DisbursementType(string type) => Type = type;

    public static readonly DisbursementType New = new("NEW");
    public static readonly DisbursementType Adjusted = new("ADJ");

    public static DisbursementType Create(string type) =>
        type.ToUpper() switch
        {
            "NEW" => New,
            "ADJ" => Adjusted,
            _ => throw new ArgumentException($"Invalid disbursement type: {type}")
        };

    protected override bool EqualsCore(ValueObject other) =>
        other is DisbursementType dt && Type == dt.Type;

    public override int GetHashCode() => Type.GetHashCode();
}

/// <summary>
/// Recovery method value object
/// </summary>
public sealed class RecoveryMethod : ValueObject
{
    public string Method { get; }

    private RecoveryMethod(string method) => Method = method;

    public static readonly RecoveryMethod RBM = new("RBM");
    public static readonly RecoveryMethod EM1 = new("EM1");
    public static readonly RecoveryMethod EMA = new("EMA");
    public static readonly RecoveryMethod FPI = new("FPI");

    public static RecoveryMethod Create(string method) =>
        method.ToUpper() switch
        {
            "RBM" => RBM,
            "EM1" => EM1,
            "EMA" => EMA,
            "FPI" => FPI,
            _ => throw new ArgumentException($"Invalid recovery method: {method}")
        };

    protected override bool EqualsCore(ValueObject other) =>
        other is RecoveryMethod rm && Method == rm.Method;

    public override int GetHashCode() => Method.GetHashCode();
}

/// <summary>
/// Settlement type value object
/// </summary>
public sealed class SettlementType : ValueObject
{
    public string Type { get; }

    private SettlementType(string type) => Type = type;

    public static readonly SettlementType Settlement = new("SET");
    public static readonly SettlementType Installment = new("INS");

    public static SettlementType Create(string type) =>
        type.ToUpper() switch
        {
            "SET" => Settlement,
            "INS" => Installment,
            _ => throw new ArgumentException($"Invalid settlement type: {type}")
        };

    protected override bool EqualsCore(ValueObject other) =>
        other is SettlementType st && Type == st.Type;

    public override int GetHashCode() => Type.GetHashCode();
}
