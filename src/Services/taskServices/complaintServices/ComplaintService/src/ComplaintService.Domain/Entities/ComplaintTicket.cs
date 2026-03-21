using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_DET — complaint/NCR ticket details.</summary>
public class ComplaintTicket : BaseEntity
{
    public decimal TicketNum { get; private set; }       // CD_TICKET_NUM (PK)
    public decimal GroupId { get; private set; }         // CD_GROUPID
    public decimal Type { get; private set; }            // CD_TYPE
    public decimal Location { get; private set; }        // CD_LOCATION
    public decimal Department { get; private set; }      // CD_DEPARTMENT
    public decimal Process { get; private set; }         // CD_PROCESS
    public string? Subject { get; private set; }         // CD_SUBJECT
    public string? Description { get; private set; }     // CD_DESCRIPTION
    public char? IsNCR { get; private set; }             // CD_NCR
    public string? PicturePath { get; private set; }     // CD_PICTUREPATH
    public string? FilePath { get; private set; }        // CD_FILEPATH
    public string TargetDate { get; private set; } = default!; // CD_TARGET_DATE
    public DateTime? ClosureDate { get; private set; }   // CD_CLOSURE_DATE

    // Navigation
    public ComplaintAction? Action { get; private set; }
    public ICollection<ComplaintEscalation> Escalations { get; private set; } = [];
    public ICollection<ComplaintTask> Tasks { get; private set; } = [];

    protected ComplaintTicket() { }

    public static ComplaintTicket Create(
        decimal ticketNum, decimal groupId, decimal type,
        decimal location, decimal department, decimal process,
        string? subject, string? description, bool isNCR,
        int targetResolutionHours)
    {
        return new ComplaintTicket
        {
            TicketNum = ticketNum,
            GroupId = groupId,
            Type = type,
            Location = location,
            Department = department,
            Process = process,
            Subject = subject,
            Description = description,
            IsNCR = isNCR ? 'Y' : 'N',
            TargetDate = DateTime.UtcNow.AddHours(targetResolutionHours).ToString("o")
        };
    }

    public void Close()
    {
        ClosureDate = DateTime.UtcNow;
    }

    public void SetAttachments(string? picturePath, string? filePath)
    {
        PicturePath = picturePath;
        FilePath = filePath;
    }

    public bool IsClosed => ClosureDate.HasValue;
}
