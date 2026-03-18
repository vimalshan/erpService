using AttendanceService.Domain.Exceptions;

namespace AttendanceService.Domain.ValueObjects;

/// <summary>I = In, O = Out</summary>
public sealed record PunchStatus
{
    public static readonly PunchStatus In = new("I");
    public static readonly PunchStatus Out = new("O");

    public string Value { get; }

    private PunchStatus(string value) => Value = value;

    public static PunchStatus From(string value) => value switch
    {
        "I" => In,
        "O" => Out,
        _ => throw new DomainException($"Invalid punch status '{value}'. Must be 'I' or 'O'.")
    };

    public override string ToString() => Value;
}
