using MediatR;

namespace EmailNotification.Application.Queries;

/// <summary>
/// Query to get recipients for an email type by organization and business unit
/// </summary>
public class GetRecipientsByOrgAndBusinessQuery : IRequest<IEnumerable<Dtos.MailAccessDto>>
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long EmailTypeId { get; set; }

    /// <summary>
    /// Organization ID
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// Business unit ID (optional)
    /// </summary>
    public long? BusinessId { get; set; }

    /// <summary>
    /// Creates a new instance of GetRecipientsByOrgAndBusinessQuery
    /// </summary>
    /// <param name="emailTypeId">Email type ID</param>
    /// <param name="orgId">Organization ID</param>
    /// <param name="businessId">Business unit ID (optional)</param>
    public GetRecipientsByOrgAndBusinessQuery(long emailTypeId, long orgId, long? businessId = null)
    {
        EmailTypeId = emailTypeId;
        OrgId = orgId;
        BusinessId = businessId;
    }
}
