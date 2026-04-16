CREATE OR ALTER PROCEDURE [dbo].[Sp_GetMasterSiteList]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SiteListJson NVARCHAR(MAX) = (
        SELECT
            s.SiteId                            AS id,
            COALESCE(s.SiteName, 'Unknown Site') AS siteName,
            COALESCE(s.SiteCode, '')             AS siteCode,
            COALESCE(s.CompanyId, 0)             AS companyId,
            COALESCE(c.CompanyName, 'Unknown Company') AS companyName,
            COALESCE(ci.CityName, '')            AS city,
            COALESCE(s.Location, '')             AS location
        FROM [dbo].[Sites] s
        LEFT JOIN [dbo].[Companies] c  ON s.CompanyId = c.CompanyId
        LEFT JOIN [dbo].[Cities]    ci ON s.CityId    = ci.CityId
        WHERE s.IsActive = 1
        ORDER BY c.CompanyName, s.SiteName
        FOR JSON PATH
    );

    IF @SiteListJson IS NULL
        SET @SiteListJson = '[]';

    SELECT (
        SELECT
            JSON_QUERY(@SiteListJson) AS data,
            CAST(1 AS BIT)           AS isSuccess,
            ''                       AS message,
            ''                       AS errorCode
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonResponse;
END


