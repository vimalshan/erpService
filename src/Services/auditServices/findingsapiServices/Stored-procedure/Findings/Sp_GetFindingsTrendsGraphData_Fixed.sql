-- =============================================
-- Description: Get findings trend graph data by category over years
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetFindingsTrendsGraphData]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetFindingsTrendsGraphData];
GO
CREATE PROCEDURE [dbo].[Sp_GetFindingsTrendsGraphData]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId    INT = NULL;
    DECLARE @CompanyId INT = NULL;
    DECLARE @SiteId    INT = NULL;
    DECLARE @ErrorCode NVARCHAR(50) = '';
    DECLARE @Message   NVARCHAR(500) = '';

    BEGIN TRY
        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId    = JSON_VALUE(@Parameters, '$.userId'),
                @CompanyId = JSON_VALUE(@Parameters, '$.companyId'),
                @SiteId    = JSON_VALUE(@Parameters, '$.siteId');
        END
        ELSE
        BEGIN
            SET @ErrorCode = 'INVALID_JSON';
            SET @Message   = 'Invalid JSON format.';
            GOTO ErrorResponse;
        END

        IF @UserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId AND IsActive = 1)
        BEGIN
            SET @ErrorCode = 'INVALID_USER';
            SET @Message   = 'User not found or inactive.';
            GOTO ErrorResponse;
        END

        -- Trend data: count by category and year
        SELECT
            ISNULL(fc.CategoryName, 'Uncategorised') AS CategoryName,
            ISNULL(fc.CategoryCode, 'NONE')          AS CategoryCode,
            YEAR(f.IdentifiedDate)                   AS FindingYear,
            COUNT(*)                                 AS FindingCount,
            SUM(CASE WHEN fs.IsClosedStatus = 1 THEN 1 ELSE 0 END) AS ClosedCount,
            SUM(CASE WHEN fs.IsClosedStatus = 0 THEN 1 ELSE 0 END) AS OpenCount
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        LEFT  JOIN FindingCategories fc ON f.FindingCategoryId = fc.FindingCategoryId
        WHERE f.IsActive = 1
          AND f.IdentifiedDate IS NOT NULL
          AND (@CompanyId IS NULL OR a.companyId = @CompanyId)
          AND (@SiteId    IS NULL OR f.SiteId    = @SiteId)
          AND (@UserId    IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        GROUP BY fc.CategoryName, fc.CategoryCode, YEAR(f.IdentifiedDate)
        ORDER BY FindingYear DESC, FindingCount DESC;

        -- Severity breakdown
        SELECT
            f.Severity,
            COUNT(*) AS Count
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        WHERE f.IsActive = 1
          AND (@CompanyId IS NULL OR a.companyId = @CompanyId)
          AND (@SiteId    IS NULL OR f.SiteId    = @SiteId)
          AND (@UserId    IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        GROUP BY f.Severity
        ORDER BY Count DESC;

        RETURN;

        ErrorResponse:
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;

    END TRY
    BEGIN CATCH
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
