-- =============================================
-- Description: Get findings trends list with location hierarchy and year-over-year data
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetFindingTrendsList]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetFindingTrendsList];
GO
CREATE PROCEDURE [dbo].[Sp_GetFindingTrendsList]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId      INT = NULL;
    DECLARE @CompanyId   INT = NULL;
    DECLARE @CurrentYear INT = YEAR(GETDATE());
    DECLARE @ErrorCode   NVARCHAR(50) = '';
    DECLARE @Message     NVARCHAR(500) = '';

    BEGIN TRY
        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId    = JSON_VALUE(@Parameters, '$.userId'),
                @CompanyId = JSON_VALUE(@Parameters, '$.companyId');
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

        -- Year-over-year totals
        SELECT
            YEAR(f.IdentifiedDate)                                              AS FindingYear,
            COUNT(*)                                                            AS TotalFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 1 THEN 1 ELSE 0 END)             AS ClosedFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 0 THEN 1 ELSE 0 END)             AS OpenFindings,
            SUM(CASE WHEN f.DueDate < GETDATE() AND fs.IsClosedStatus = 0 THEN 1 ELSE 0 END) AS OverdueFindings
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        WHERE f.IsActive = 1
          AND f.IdentifiedDate IS NOT NULL
          AND (@CompanyId IS NULL OR a.companyId = @CompanyId)
          AND (@UserId    IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        GROUP BY YEAR(f.IdentifiedDate)
        ORDER BY FindingYear DESC;

        -- Per-site breakdown
        SELECT
            s.SiteId,
            s.SiteName,
            s.Location,
            YEAR(f.IdentifiedDate)                                              AS FindingYear,
            COUNT(*)                                                            AS TotalFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 1 THEN 1 ELSE 0 END)             AS ClosedFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 0 THEN 1 ELSE 0 END)             AS OpenFindings
        FROM Findings f
        INNER JOIN Audits a   ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        LEFT  JOIN Sites s    ON f.SiteId  = s.SiteId
        WHERE f.IsActive = 1
          AND f.IdentifiedDate IS NOT NULL
          AND (@CompanyId IS NULL OR a.companyId = @CompanyId)
          AND (@UserId    IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        GROUP BY s.SiteId, s.SiteName, s.Location, YEAR(f.IdentifiedDate)
        ORDER BY FindingYear DESC, TotalFindings DESC;

        RETURN;

        ErrorResponse:
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;

    END TRY
    BEGIN CATCH
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
