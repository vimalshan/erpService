using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class AutoMailIdEntity : Entity<int>
{
    public string? IdType { get; private set; }
    public string? MailId { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? MailType { get; private set; }

    private AutoMailIdEntity() { }

    public static AutoMailIdEntity Create(
        string? idType, string? mailId, DateTime? startDate, DateTime? endDate, string? mailType)
    {
        return new AutoMailIdEntity
        {
            IdType = idType,
            MailId = mailId,
            StartDate = startDate,
            EndDate = endDate,
            MailType = mailType
        };
    }
}
