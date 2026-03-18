namespace RecruitmentService.Domain.ValueObjects;

public enum ApplicationStatus
{
    Received = 1,    // 01
    InProgress = 2,  // 02
    NotSuitable = 3, // 03
    Shortlisted = 4, // 04
    Selected = 5,    // 05
    Rejected = 6     // 06
}

public static class ApplicationStatusExtensions
{
    public static string ToCode(this ApplicationStatus status) =>
        status switch
        {
            ApplicationStatus.Received => "01",
            ApplicationStatus.InProgress => "02",
            ApplicationStatus.NotSuitable => "03",
            ApplicationStatus.Shortlisted => "04",
            ApplicationStatus.Selected => "05",
            ApplicationStatus.Rejected => "06",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

    public static ApplicationStatus FromCode(string code) =>
        code switch
        {
            "01" => ApplicationStatus.Received,
            "02" => ApplicationStatus.InProgress,
            "03" => ApplicationStatus.NotSuitable,
            "04" => ApplicationStatus.Shortlisted,
            "05" => ApplicationStatus.Selected,
            "06" => ApplicationStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(code), $"Invalid status code: {code}")
        };
}
