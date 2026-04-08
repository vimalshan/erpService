using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class AutoMailStatusEntity : AggregateRoot<int>
{
    public string MailType { get; private set; } = string.Empty;
    public DateTime MailDate { get; private set; }
    public string MailStatus { get; private set; } = string.Empty;
    public string? MailRemarks { get; private set; }

    private AutoMailStatusEntity() { }

    public static AutoMailStatusEntity Create(
        string mailType, DateTime mailDate, string mailStatus, string? remarks)
    {
        return new AutoMailStatusEntity
        {
            MailType = mailType,
            MailDate = mailDate,
            MailStatus = mailStatus,
            MailRemarks = remarks
        };
    }

    public void UpdateStatus(string newStatus, string? remarks)
    {
        MailStatus = newStatus;
        MailRemarks = remarks ?? MailRemarks;
    }
}
