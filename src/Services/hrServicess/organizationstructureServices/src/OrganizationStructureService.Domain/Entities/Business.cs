using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.Events;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Business : AggregateRoot
{
    public decimal BusinessId { get; private set; }
    public string BusinessName { get; private set; } = string.Empty;
    public string BusinessShortName { get; private set; } = string.Empty;
    public string BusinessCode { get; private set; } = string.Empty;
    public decimal BusinessCompanyId { get; private set; }
    public string BusinessCompanyCode { get; private set; } = string.Empty;
    public LiveFlag LiveFlag { get; private set; } = LiveFlag.Active;
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }

    private Business() { }

    public static Business Create(
        decimal businessId,
        string businessName,
        string businessShortName,
        string businessCode,
        decimal companyId,
        string companyCode,
        decimal updatedBy)
    {
        var business = new Business
        {
            BusinessId = businessId,
            BusinessName = businessName,
            BusinessShortName = businessShortName,
            BusinessCode = businessCode,
            BusinessCompanyId = companyId,
            BusinessCompanyCode = companyCode,
            LiveFlag = LiveFlag.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
        business.RaiseDomainEvent(new BusinessCreatedEvent(businessId, businessName));
        business.IncrementVersion();
        return business;
    }

    public void Update(string businessName, string businessShortName, decimal updatedBy)
    {
        BusinessName = businessName;
        BusinessShortName = businessShortName;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        RaiseDomainEvent(new BusinessUpdatedEvent(BusinessId, businessName));
        IncrementVersion();
    }

    public void Deactivate(decimal updatedBy)
    {
        LiveFlag = LiveFlag.Inactive;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        IncrementVersion();
    }
}
