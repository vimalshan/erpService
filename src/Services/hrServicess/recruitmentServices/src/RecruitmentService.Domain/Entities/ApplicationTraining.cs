namespace RecruitmentService.Domain.Entities;

public class ApplicationTraining
{
    public decimal AppId { get; private set; }
    public decimal TrainingId { get; private set; }
    public string? Title { get; private set; }
    public string? Duration { get; private set; }
    public string? Institute { get; private set; }
    public string? Location { get; private set; }

    private ApplicationTraining() { }

    public static ApplicationTraining Create(
        decimal appId, decimal trainingId, string? title,
        string? duration, string? institute, string? location) =>
        new()
        {
            AppId = appId,
            TrainingId = trainingId,
            Title = title,
            Duration = duration,
            Institute = institute,
            Location = location
        };
}
