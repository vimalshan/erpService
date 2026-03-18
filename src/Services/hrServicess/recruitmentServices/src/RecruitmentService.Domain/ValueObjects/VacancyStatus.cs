namespace RecruitmentService.Domain.ValueObjects;

public enum VacancyStatus
{
    Open,   // Y
    Closed  // N
}

public static class VacancyStatusExtensions
{
    public static string ToCode(this VacancyStatus status) =>
        status == VacancyStatus.Open ? "Y" : "N";

    public static VacancyStatus FromCode(string? code) =>
        code == "Y" ? VacancyStatus.Open : VacancyStatus.Closed;
}
