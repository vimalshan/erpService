namespace EmailNotification.Domain.Entities;

/// <summary>
/// Mail access entity representing a recipient for an email type
/// </summary>
public class MailAccess : Common.Entity
{
    /// <summary>
    /// Email type ID (foreign key)
    /// </summary>
    public long MailTypeId { get; private set; }

    /// <summary>
    /// Organization ID (0 for all organizations, NULL for global)
    /// </summary>
    public long? MailOrgId { get; private set; }

    /// <summary>
    /// Business unit ID (0 for all business units, NULL for global)
    /// </summary>
    public long? MailBusinessId { get; private set; }

    /// <summary>
    /// Employee system ID (NULL for non-employees)
    /// </summary>
    public long? MailEmpSysId { get; private set; }

    /// <summary>
    /// Email address of the recipient
    /// </summary>
    public ValueObjects.EmailAddress MailEmail { get; private set; }

    /// <summary>
    /// Name of non-employee recipient (if applicable)
    /// </summary>
    public string? MailName { get; private set; }

    /// <summary>
    /// Private parameterless constructor for Entity Framework Core only
    /// </summary>
    private MailAccess() { }

    /// <summary>
    /// Initializes a new instance of the MailAccess class
    /// </summary>
    /// <param name="mailTypeId">The email type ID</param>
    /// <param name="mailEmail">The recipient email address</param>
    /// <param name="createdBy">User ID who created this record</param>
    /// <param name="mailOrgId">Optional organization ID</param>
    /// <param name="mailBusinessId">Optional business unit ID</param>
    /// <param name="mailEmpSysId">Optional employee system ID</param>
    /// <param name="mailName">Optional non-employee name</param>
    public MailAccess(
        long mailTypeId,
        ValueObjects.EmailAddress mailEmail,
        long createdBy,
        long? mailOrgId = null,
        long? mailBusinessId = null,
        long? mailEmpSysId = null,
        string? mailName = null)
    {
        MailTypeId = mailTypeId;
        MailEmail = mailEmail;
        CreatedBy = createdBy;
        ModifiedBy = createdBy;
        MailOrgId = mailOrgId;
        MailBusinessId = mailBusinessId;
        MailEmpSysId = mailEmpSysId;
        MailName = mailName;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the mail access record
    /// </summary>
    /// <param name="mailEmail">The new email address</param>
    /// <param name="modifiedBy">User ID who is modifying this record</param>
    /// <param name="mailName">Optional new non-employee name</param>
    public void Update(
        ValueObjects.EmailAddress mailEmail,
        long modifiedBy,
        string? mailName = null)
    {
        MailEmail = mailEmail;
        MailName = mailName;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
