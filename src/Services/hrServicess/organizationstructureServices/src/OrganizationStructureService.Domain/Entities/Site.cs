using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Site : Entity
{
    public decimal SiteId { get; private set; }
    public string? SiteName { get; private set; }
    public string? SiteShortName { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string? AddressPin { get; private set; }
    public decimal SiteCityCode { get; private set; }
    public decimal SiteCategoryCode { get; private set; }
    public string? Phone1 { get; private set; }
    public string? Phone2 { get; private set; }
    public string? FaxNo { get; private set; }
    public string? Landmark1 { get; private set; }
    public string? Landmark2 { get; private set; }
    public string? ImagePath { get; private set; }
    public string? VisitorPolicyPath { get; private set; }
    public string? NearestAirport { get; private set; }
    public string? DistanceAirport { get; private set; }
    public string? NearestRail { get; private set; }
    public string? DistanceRail { get; private set; }
    public string? Remarks { get; private set; }
    public decimal? LocationCode { get; private set; }
    public string? AttachedEmployee { get; private set; }
    public string? ContactName1 { get; private set; }
    public string? ContactPhone1 { get; private set; }
    public string? ContactName2 { get; private set; }
    public string? ContactPhone2 { get; private set; }
    public LiveFlag? LiveFlag { get; private set; }
    public long? TravelLocationId { get; private set; }

    private Site() { }

    public static Site Create(decimal siteId, string siteName, string shortName, decimal cityCode, decimal categoryCode)
    {
        return new Site
        {
            SiteId = siteId,
            SiteName = siteName,
            SiteShortName = shortName,
            SiteCityCode = cityCode,
            SiteCategoryCode = categoryCode,
            AttachedEmployee = "N",
            LiveFlag = ValueObjects.LiveFlag.Active
        };
    }

    public void UpdateContactInfo(string? contactName1, string? contactPhone1, string? contactName2, string? contactPhone2)
    {
        ContactName1 = contactName1;
        ContactPhone1 = contactPhone1;
        ContactName2 = contactName2;
        ContactPhone2 = contactPhone2;
    }

    public void UpdateAddress(string? line1, string? line2, string? line3, string? line4, string? pin)
    {
        AddressLine1 = line1;
        AddressLine2 = line2;
        AddressLine3 = line3;
        AddressLine4 = line4;
        AddressPin = pin;
    }
}
