namespace ContributionService.Domain.Entities;

public class ContributionProcessLog
{
    public long LogId { get; set; }
    public string LogType { get; set; } = null!;
    public string LogMessage { get; set; } = null!;
    public DateTime ProcessDate { get; set; }
    public long UserId { get; set; }

    public static ContributionProcessLog Create(string logType, string message, long userId)
    {
        return new ContributionProcessLog
        {
            LogType = logType,
            LogMessage = message,
            ProcessDate = DateTime.UtcNow,
            UserId = userId
        };
    }
}
