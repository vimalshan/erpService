using EmployeePrideManagement.Domain.Events;
using EmployeePrideManagement.Domain.ValueObjects;

namespace EmployeePrideManagement.Domain.Entities;

public class MomentPride : BaseEntity
{
    public decimal MomentPrideId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Body { get; private set; }
    public decimal EmployeeSysId { get; private set; }
    public string Footer { get; private set; } = string.Empty;
    public Location Location { get; private set; } = null!;
    public ImagePath Image { get; private set; } = null!;
    public long ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private MomentPride() { } // EF constructor

    public MomentPride(
        string title,
        string? body,
        decimal employeeSysId,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Body = body;
        EmployeeSysId = employeeSysId;
        Footer = footer ?? throw new ArgumentNullException(nameof(footer));
        Location = new Location(location);
        Image = new ImagePath(imagePath);
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new PrideMomentCreatedEvent(this));
    }

    public void Update(
        string title,
        string? body,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Body = body;
        Footer = footer ?? throw new ArgumentNullException(nameof(footer));
        Location = new Location(location);
        Image = new ImagePath(imagePath);
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new PrideMomentUpdatedEvent(this));
    }
}
