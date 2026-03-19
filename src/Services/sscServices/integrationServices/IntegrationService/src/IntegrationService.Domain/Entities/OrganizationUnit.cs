using IntegrationService.Domain.Common;
using IntegrationService.Domain.Events;

namespace IntegrationService.Domain.Entities;

public class OrganizationUnit : BaseEntity<string>, IAggregateRoot
{
    public string OuName { get; private set; } = string.Empty;
    public string BuId { get; private set; } = string.Empty;

    private OrganizationUnit() { }

    public static OrganizationUnit Create(string ouId, string ouName, string buId)
    {
        if (string.IsNullOrWhiteSpace(ouId))
            throw new ArgumentException("OU ID is required.", nameof(ouId));
        if (string.IsNullOrWhiteSpace(ouName))
            throw new ArgumentException("OU Name is required.", nameof(ouName));

        var ou = new OrganizationUnit
        {
            Id = ouId,
            OuName = ouName,
            BuId = buId
        };

        ou.AddDomainEvent(new OrganizationUnitCreatedEvent(ou.Id, ou.OuName));
        return ou;
    }

    public void UpdateDetails(string ouName, string buId)
    {
        OuName = ouName;
        BuId = buId;
    }
}
