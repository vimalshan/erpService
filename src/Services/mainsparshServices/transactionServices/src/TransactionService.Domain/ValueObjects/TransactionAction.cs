namespace TransactionService.Domain.ValueObjects;

public sealed class TransactionAction
{
    public static readonly TransactionAction Create = new("CREATE");
    public static readonly TransactionAction Update = new("UPDATE");
    public static readonly TransactionAction Delete = new("DELETE");
    public static readonly TransactionAction Submit = new("SUBMIT");
    public static readonly TransactionAction Approve = new("APPROVE");
    public static readonly TransactionAction Reject = new("REJECT");
    public static readonly TransactionAction Process = new("PROCESS");
    public static readonly TransactionAction Cancel = new("CANCEL");

    public string Code { get; }

    private TransactionAction(string code) => Code = code;

    public static TransactionAction FromCode(string code) => code switch
    {
        "CREATE" => Create,
        "UPDATE" => Update,
        "DELETE" => Delete,
        "SUBMIT" => Submit,
        "APPROVE" => Approve,
        "REJECT" => Reject,
        "PROCESS" => Process,
        "CANCEL" => Cancel,
        _ => throw new ArgumentException($"Unknown transaction action: {code}", nameof(code))
    };

    public override string ToString() => Code;
    public override bool Equals(object? obj) => obj is TransactionAction other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}
