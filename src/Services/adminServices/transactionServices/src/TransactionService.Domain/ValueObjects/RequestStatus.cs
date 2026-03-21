namespace TransactionService.Domain.ValueObjects;

using TransactionService.Domain.Common;

public sealed class RequestStatus : ValueObject
{
    public static readonly RequestStatus Pending = new("P");
    public static readonly RequestStatus Approved = new("A");
    public static readonly RequestStatus Rejected = new("R");
    public static readonly RequestStatus Completed = new("C");
    public static readonly RequestStatus Indented = new("I");

    public string Value { get; }

    public RequestStatus(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 1)
            throw new ArgumentException("Request status must be a single character.");
        Value = value.Trim().ToUpperInvariant();
    }

    public bool IsPending => Value == "P";
    public bool IsApproved => Value == "A";
    public bool IsRejected => Value == "R";
    public bool IsCompleted => Value == "C";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(RequestStatus status) => status.Value;
    public static implicit operator RequestStatus(string value) => new(value);
}
