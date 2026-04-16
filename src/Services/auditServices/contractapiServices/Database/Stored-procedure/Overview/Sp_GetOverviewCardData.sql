SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE [dbo].[Sp_GetOverviewCardData]
    @filterCompanies NVARCHAR(MAX) = NULL,
    @filterSites     NVARCHAR(MAX) = NULL,
    @filterServices  NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @CompanyList  TABLE (CompanyId  INT);
        DECLARE @SiteList     TABLE (SiteId     INT);
        DECLARE @ServiceList  TABLE (ServiceId  INT);

        IF @filterCompanies IS NOT NULL AND @filterCompanies NOT IN ('[]','')
            INSERT INTO @CompanyList  SELECT CAST(value AS INT) FROM OPENJSON(@filterCompanies);
        IF @filterSites IS NOT NULL AND @filterSites NOT IN ('[]','')
            INSERT INTO @SiteList     SELECT CAST(value AS INT) FROM OPENJSON(@filterSites);
        IF @filterServices IS NOT NULL AND @filterServices NOT IN ('[]','')
            INSERT INTO @ServiceList  SELECT CAST(value AS INT) FROM OPENJSON(@filterServices);

        SELECT CAST(svc.ServiceId AS NVARCHAR(50))   AS serviceId,
               COALESCE(svc.ServiceName, '')        AS serviceName,
               YEAR(c.ExpiryDate)                    AS year,
               COUNT(*)                              AS count,
               ROW_NUMBER() OVER (ORDER BY c.Status) AS seq,
               COALESCE(c.Status, 'Active')          AS statusValue,
               COUNT(*) OVER (PARTITION BY svc.ServiceId, YEAR(c.ExpiryDate)) AS totalCount
        FROM   Certificates c
        INNER JOIN CertificateServices cs  ON c.CertificateId = cs.CertificateId
        INNER JOIN Services svc            ON cs.ServiceId    = svc.ServiceId
        WHERE  c.ExpiryDate IS NOT NULL
          AND  (NOT EXISTS (SELECT 1 FROM @CompanyList) OR c.CompanyId IN (SELECT CompanyId FROM @CompanyList))
          AND  (NOT EXISTS (SELECT 1 FROM @SiteList)    OR EXISTS (
                    SELECT 1 FROM CertificateSites cst
                    WHERE cst.CertificateId = c.CertificateId
                      AND cst.SiteId IN (SELECT SiteId FROM @SiteList)))
          AND  (NOT EXISTS (SELECT 1 FROM @ServiceList) OR cs.ServiceId IN (SELECT ServiceId FROM @ServiceList))
        GROUP  BY svc.ServiceId, svc.ServiceName, YEAR(c.ExpiryDate), c.Status
        ORDER  BY svc.ServiceId, YEAR(c.ExpiryDate);
    END TRY
    BEGIN CATCH
        SELECT '' AS serviceId, '' AS serviceName, 0 AS year,
               0 AS count, 0 AS seq, '' AS statusValue, 0 AS totalCount;
    END CATCH
END