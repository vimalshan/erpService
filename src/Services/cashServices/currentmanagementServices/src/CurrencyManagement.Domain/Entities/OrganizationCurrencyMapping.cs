using CurrencyManagement.Domain.Common;
using CurrencyManagement.Domain.Events;

namespace CurrencyManagement.Domain.Entities;

/// <summary>
/// Organization to Currency mapping entity
/// Maps to DEAL_ORGCURRMAP table
/// </summary>
public class OrganizationCurrencyMapping : BaseEntity
{
    public long OrganizationId { get; private set; }
    public long CurrencyId { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    #region Constructors
    private OrganizationCurrencyMapping() { }

    public OrganizationCurrencyMapping(long organizationId, long currencyId, long modifiedBy)
    {
        if (organizationId <= 0)
            throw new ArgumentException("Organization ID must be positive", nameof(organizationId));

        if (currencyId <= 0)
            throw new ArgumentException("Currency ID must be positive", nameof(currencyId));

        OrganizationId = organizationId;
        CurrencyId = currencyId;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new OrganizationCurrencyMappedDomainEvent(organizationId, currencyId));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Updates the organization currency mapping
    /// </summary>
    public void Update(long currencyId, long modifiedBy)
    {
        if (currencyId <= 0)
            throw new ArgumentException("Currency ID must be positive", nameof(currencyId));

        CurrencyId = currencyId;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
    #endregion
}
