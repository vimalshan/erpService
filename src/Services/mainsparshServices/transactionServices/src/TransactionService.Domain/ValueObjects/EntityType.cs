namespace TransactionService.Domain.ValueObjects;

public sealed class EntityType
{
    public static readonly EntityType Booking = new("BOOKING");
    public static readonly EntityType Reimbursement = new("REIMBURSEMENT");
    public static readonly EntityType Stipend = new("STIPEND");
    public static readonly EntityType Timesheet = new("TIMESHEET");
    public static readonly EntityType Meeting = new("MEETING");
    public static readonly EntityType Proxy = new("PROXY");

    public string Code { get; }

    private EntityType(string code) => Code = code;

    public static EntityType FromCode(string code) => code switch
    {
        "BOOKING" => Booking,
        "REIMBURSEMENT" => Reimbursement,
        "STIPEND" => Stipend,
        "TIMESHEET" => Timesheet,
        "MEETING" => Meeting,
        "PROXY" => Proxy,
        _ => throw new ArgumentException($"Unknown entity type: {code}", nameof(code))
    };

    public override string ToString() => Code;
    public override bool Equals(object? obj) => obj is EntityType other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}
