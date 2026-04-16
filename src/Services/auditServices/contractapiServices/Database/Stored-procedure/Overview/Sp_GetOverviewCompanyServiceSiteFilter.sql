CREATE PROCEDURE [dbo].[Sp_GetOverviewCompanyServiceSiteFilter]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT DISTINCT
               cs.CompanyId  AS companyId,
               css.ServiceId AS serviceId,
               cst.SiteId    AS siteId
        FROM   Certificates cs
        INNER JOIN CertificateServices  css ON cs.CertificateId = css.CertificateId
        INNER JOIN CertificateSites     cst ON cs.CertificateId = cst.CertificateId
        WHERE  cs.CompanyId IS NOT NULL
        ORDER  BY cs.CompanyId, css.ServiceId, cst.SiteId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS companyId, 0 AS serviceId, 0 AS siteId;
    END CATCH
END


