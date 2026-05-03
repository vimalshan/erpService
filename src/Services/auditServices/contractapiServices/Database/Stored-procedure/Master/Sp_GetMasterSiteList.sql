CREATE PROCEDURE [dbo].[Sp_GetMasterSiteList]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT s.SiteId         AS id,
               COALESCE(s.SiteName, 'Unknown Site') AS siteName,
               COALESCE(s.CompanyId, 0)             AS companyId,
               COALESCE(c.CompanyName, '')           AS companyName,
               ''                                   AS city,
               0                                    AS countryId,
               ''                                   AS countryName,
               COALESCE(s.Address, '')              AS formattedAddress,
               ''                                   AS siteState,
               ''                                   AS siteZip
        FROM   Sites s
        LEFT JOIN Companies c ON s.CompanyId = c.CompanyId
        WHERE  s.IsActive = 1
        ORDER  BY c.CompanyName, s.SiteName;
    END TRY
    BEGIN CATCH
        SELECT 0 AS id, ERROR_MESSAGE() AS siteName, 0 AS companyId, '' AS companyName,
               '' AS city, 0 AS countryId, '' AS countryName, '' AS formattedAddress,
               '' AS siteState, '' AS siteZip;
    END CATCH
END


