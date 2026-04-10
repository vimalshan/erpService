using TaskTransactional.Domain.Common;
using TaskTransactional.Domain.Events;

namespace TaskTransactional.Domain.Entities;

public class ComplaintDetail : AggregateRoot
{
    public decimal CdTicketNum { get; private set; }
    public decimal CdGroupId { get; private set; }
    public decimal CdType { get; private set; }
    public decimal CdLocation { get; private set; }
    public decimal CdDepartment { get; private set; }
    public decimal CdProcess { get; private set; }
    public string? CdSubject { get; private set; }
    public string? CdDescription { get; private set; }
    public string? CdNcr { get; private set; }
    public string? CdPicturePath { get; private set; }
    public string? CdFilePath { get; private set; }
    public string CdTargetDate { get; private set; } = null!;
    public DateTime? CdClosureDate { get; private set; }

    // Navigation
    public ICollection<ComplaintTask> Tasks { get; private set; } = [];

    private ComplaintDetail() { }

    public static ComplaintDetail Create(
        decimal ticketNum, decimal groupId, decimal type, decimal location,
        decimal department, decimal process, string targetDate,
        string? subject = null, string? description = null, string? ncr = null)
    {
        var entity = new ComplaintDetail
        {
            CdTicketNum = ticketNum,
            CdGroupId = groupId,
            CdType = type,
            CdLocation = location,
            CdDepartment = department,
            CdProcess = process,
            CdTargetDate = targetDate,
            CdSubject = subject,
            CdDescription = description,
            CdNcr = ncr ?? "N"
        };

        entity.AddDomainEvent(new TicketCreatedEvent(ticketNum, subject ?? string.Empty));
        return entity;
    }

    public void Close()
    {
        CdClosureDate = DateTime.UtcNow;
        AddDomainEvent(new TicketClosedEvent(CdTicketNum));
    }

    public void SetFilePaths(string? picturePath, string? filePath)
    {
        CdPicturePath = picturePath;
        CdFilePath = filePath;
    }
}
