using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>Leave Model (maps to LET_MODEL)</summary>
public sealed class LeaveModel : BaseEntity
{
    public long LtSklCod { get; private set; }
    public long LtLvlNum { get; private set; }
    public string LtFncCod { get; private set; } = default!;
    public long LtJobCod { get; private set; }

    private LeaveModel() { }

    public static LeaveModel Create(long sklCod, long lvlNum, string fncCod, long jobCod)
        => new() { LtSklCod = sklCod, LtLvlNum = lvlNum, LtFncCod = fncCod, LtJobCod = jobCod };
}

/// <summary>Leave Signature ID (maps to LET_SIGID)</summary>
public sealed class LeaveSignatureId : BaseEntity
{
    public string? LetSigid { get; private set; }
    public string? SigName { get; private set; }
    public string? SigDesg { get; private set; }

    private LeaveSignatureId() { }

    public static LeaveSignatureId Create(string? sigId, string? name, string? designation)
        => new() { LetSigid = sigId, SigName = name, SigDesg = designation };
}
