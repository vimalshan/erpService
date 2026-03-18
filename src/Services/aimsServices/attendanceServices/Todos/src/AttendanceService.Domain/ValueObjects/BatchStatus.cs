using AttendanceService.Domain.Exceptions;

namespace AttendanceService.Domain.ValueObjects;

/// <summary>P = Pending, Y = Closed</summary>
public sealed record BatchStatus
{
    public static readonly BatchStatus Pending = new("P");
    public static readonly BatchStatus Closed = new("Y");

    public string Value { get; }

    private BatchStatus(string value) => Value = value;

    public static BatchStatus From(string value) => value switch
    {
        "P" => Pending,
        "Y" => Closed,
        _ => throw new DomainException($"Invalid batch status '{value}'.")
    };

    public override string ToString() => Value;
}
