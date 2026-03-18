using RecruitmentService.Domain.Common;
using RecruitmentService.Domain.Events;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Domain.Entities;

public class Prospect : AggregateRoot
{
    public decimal WebUserId { get; private set; }
    public string? Password { get; private set; }
    public string? FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string? LastName { get; private set; }
    public string? EmailId { get; private set; }
    public ProspectStatus Status { get; private set; } = ProspectStatus.Live;
    public DateTime? DateOfBirth { get; private set; }
    public DateTime? CreatedOn { get; private set; }
    public string? ProspectType { get; private set; }

    // Navigation
    public ICollection<ProspectAddress> Addresses { get; private set; } = new List<ProspectAddress>();
    public ICollection<ProspectQualification> Qualifications { get; private set; } = new List<ProspectQualification>();
    public ICollection<ProspectReference> References { get; private set; } = new List<ProspectReference>();
    public ICollection<ProspectTraining> Trainings { get; private set; } = new List<ProspectTraining>();

    private Prospect() { }

    public static Prospect Register(
        decimal userId, string firstName, string? middleName, string lastName,
        string emailId, DateTime? dateOfBirth, string? prospectType)
    {
        var prospect = new Prospect
        {
            WebUserId = userId,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            EmailId = emailId,
            Status = ProspectStatus.Live,
            DateOfBirth = dateOfBirth,
            CreatedOn = DateTime.UtcNow,
            ProspectType = prospectType
        };

        prospect.AddDomainEvent(new ProspectRegisteredEvent(userId, emailId));
        return prospect;
    }

    public void Deactivate()
    {
        Status = ProspectStatus.Closed;
        AddDomainEvent(new ProspectDeactivatedEvent(WebUserId));
    }

    public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
}
