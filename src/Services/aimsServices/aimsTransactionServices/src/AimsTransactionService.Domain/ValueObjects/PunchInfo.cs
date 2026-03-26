using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Domain.ValueObjects;

public sealed record PunchInfo
{
    public int GateNo { get; }
    public PunchStatus PunchStatus { get; }
    public int? MachineNo { get; }
    public string? ReferenceNo { get; }

    public PunchInfo(int gateNo, PunchStatus punchStatus, int? machineNo = null, string? referenceNo = null)
    {
        GateNo = gateNo;
        PunchStatus = punchStatus;
        MachineNo = machineNo;
        ReferenceNo = referenceNo;
    }
}
