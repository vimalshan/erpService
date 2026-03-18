namespace RecruitmentService.Domain.ValueObjects;

public enum ProspectStatus
{
    Live,   // L
    Closed  // C
}

public static class ProspectStatusExtensions
{
    public static string ToCode(this ProspectStatus status) =>
        status == ProspectStatus.Live ? "L" : "C";

    public static ProspectStatus FromCode(string? code) =>
        code == "L" ? ProspectStatus.Live : ProspectStatus.Closed;
}
