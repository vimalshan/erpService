namespace EmailNotification.Domain.Aggregates;

/// <summary>
/// Email Type aggregate root - represents an email alert type and manages its distribution list
/// </summary>
public class EmailTypeAggregate : Common.Entity
{
    /// <summary>
    /// Name of the email alert
    /// </summary>
    public string EmailName { get; private set; }

    /// <summary>
    /// Email type (Daily or Event-based)
    /// </summary>
    public ValueObjects.EmailTypeEnum EmailType { get; private set; }

    /// <summary>
    /// Procedure name that generates this email
    /// </summary>
    public string EmailProcName { get; private set; }

    /// <summary>
    /// Collection of recipients for this email type
    /// </summary>
    private readonly List<Entities.MailAccess> _mailAccessList = new();

    /// <summary>
    /// Gets the read-only collection of mail access records
    /// </summary>
    public IReadOnlyCollection<Entities.MailAccess> MailAccessList => _mailAccessList.AsReadOnly();

    /// <summary>
    /// Private parameterless constructor for Entity Framework Core only
    /// </summary>
    private EmailTypeAggregate() { }

    /// <summary>
    /// Initializes a new instance of the EmailTypeAggregate class
    /// </summary>
    /// <param name="emailName">Name of the email alert</param>
    /// <param name="emailType">Type of email (Daily or Event)</param>
    /// <param name="emailProcName">Procedure name that generates the email</param>
    /// <param name="createdBy">User ID who created this record</param>
    public EmailTypeAggregate(
        string emailName,
        ValueObjects.EmailTypeEnum emailType,
        string emailProcName,
        long createdBy)
    {
        if (string.IsNullOrWhiteSpace(emailName))
            throw new ArgumentException("Email name cannot be empty", nameof(emailName));

        if (string.IsNullOrWhiteSpace(emailProcName))
            throw new ArgumentException("Procedure name cannot be empty", nameof(emailProcName));

        EmailName = emailName;
        EmailType = emailType;
        EmailProcName = emailProcName;
        CreatedBy = createdBy;
        ModifiedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the email type information
    /// </summary>
    /// <param name="emailName">New email name</param>
    /// <param name="emailProcName">New procedure name</param>
    /// <param name="modifiedBy">User ID who is modifying this record</param>
    public void Update(string emailName, string emailProcName, long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(emailName))
            throw new ArgumentException("Email name cannot be empty", nameof(emailName));

        if (string.IsNullOrWhiteSpace(emailProcName))
            throw new ArgumentException("Procedure name cannot be empty", nameof(emailProcName));

        EmailName = emailName;
        EmailProcName = emailProcName;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a recipient to the email type
    /// </summary>
    /// <param name="mailAccess">The recipient to add</param>
    public void AddRecipient(Entities.MailAccess mailAccess)
    {
        if (mailAccess == null)
            throw new ArgumentNullException(nameof(mailAccess));

        if (_mailAccessList.Any(m => m.MailEmail == mailAccess.MailEmail))
            throw new InvalidOperationException($"Recipient {mailAccess.MailEmail} already exists in this email type");

        _mailAccessList.Add(mailAccess);
    }

    /// <summary>
    /// Removes a recipient from the email type
    /// </summary>
    /// <param name="mailAccessId">The ID of the recipient to remove</param>
    public void RemoveRecipient(long mailAccessId)
    {
        var mailAccess = _mailAccessList.FirstOrDefault(m => m.Id == mailAccessId);
        if (mailAccess != null)
        {
            _mailAccessList.Remove(mailAccess);
        }
    }

    /// <summary>
    /// Gets recipients filtered by organization and business unit
    /// </summary>
    /// <param name="orgId">The organization ID</param>
    /// <param name="businessId">The business unit ID (optional)</param>
    /// <returns>Enumerable of recipients matching the criteria</returns>
    public IEnumerable<Entities.MailAccess> GetRecipientsByOrgAndBusiness(long orgId, long? businessId = null)
    {
        return _mailAccessList.Where(m =>
            (m.MailOrgId == null || m.MailOrgId == 0 || m.MailOrgId == orgId) &&
            (businessId == null || m.MailBusinessId == null || m.MailBusinessId == 0 || m.MailBusinessId == businessId)
        );
    }

    /// <summary>
    /// Gets all recipients
    /// </summary>
    /// <returns>Enumerable of all recipients</returns>
    public IEnumerable<Entities.MailAccess> GetAllRecipients()
    {
        return _mailAccessList;
    }
}
