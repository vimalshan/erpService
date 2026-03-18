using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

/// <summary>Maps to WEBSITE_CON_MAILID table in SRFSPARSHDB</summary>
public class WebsiteContactEmail : BaseEntity
{
    public long ContactId { get; private set; }
    public long UserSysId { get; private set; }
    public string PrimaryEmail { get; private set; } = string.Empty;
    public string? SecondaryEmail { get; private set; }
    public string? Phone { get; private set; }
    public string? Mobile { get; private set; }
    public string? Website { get; private set; }
    public string? SocialMedia { get; private set; }
    public char NewsletterOptIn { get; private set; } = 'Y';
    public char ContactStatus { get; private set; } = 'A';
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private WebsiteContactEmail() { } // EF constructor

    public static WebsiteContactEmail Create(
        long userSysId,
        string primaryEmail,
        long createdBy,
        string? secondaryEmail = null,
        string? phone = null,
        string? mobile = null,
        string? website = null,
        string? socialMedia = null,
        bool newsletterOptIn = true)
    {
        if (userSysId <= 0)
            throw new DomainException("UserSysId must be a positive value.");

        var contact = new WebsiteContactEmail
        {
            UserSysId = userSysId,
            PrimaryEmail = ValueObjects.Email.Create(primaryEmail).Value,
            SecondaryEmail = string.IsNullOrWhiteSpace(secondaryEmail) ? null : ValueObjects.Email.Create(secondaryEmail).Value,
            Phone = phone,
            Mobile = mobile,
            Website = website,
            SocialMedia = socialMedia,
            NewsletterOptIn = newsletterOptIn ? 'Y' : 'N',
            ContactStatus = 'A',
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        contact.AddDomainEvent(new Events.UserContactCreatedEvent(contact));
        return contact;
    }

    public void Update(
        string? secondaryEmail,
        string? phone,
        string? mobile,
        string? website,
        string? socialMedia,
        bool newsletterOptIn,
        long updatedBy)
    {
        SecondaryEmail = string.IsNullOrWhiteSpace(secondaryEmail) ? null : ValueObjects.Email.Create(secondaryEmail).Value;
        Phone = phone;
        Mobile = mobile;
        Website = website;
        SocialMedia = socialMedia;
        NewsletterOptIn = newsletterOptIn ? 'Y' : 'N';
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new Events.UserContactUpdatedEvent(this));
    }

    public void Deactivate(long updatedBy)
    {
        ContactStatus = 'I';
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public bool IsActive => ContactStatus == 'A';
    public bool IsNewsletterSubscribed => NewsletterOptIn == 'Y';
}
