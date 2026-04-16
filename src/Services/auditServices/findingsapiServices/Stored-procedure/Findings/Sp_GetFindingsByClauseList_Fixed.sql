-- =============================================
-- Description: Get findings grouped by clause with category counts
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetFindingsByClauseList]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetFindingsByClauseList];
GO
CREATE PROCEDURE [dbo].[Sp_GetFindingsByClauseList]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId    INT = NULL;
    DECLARE @StartDate DATE = NULL;
    DECLARE @EndDate   DATE = NULL;
    DECLARE @CompanyId INT = NULL;
    DECLARE @SiteId    INT = NULL;
    DECLARE @ErrorCode NVARCHAR(50) = '';
    DECLARE @Message   NVARCHAR(500) = '';

    BEGIN TRY
        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId    = JSON_VALUE(@Parameters, '$.userId'),
                @StartDate = TRY_CAST(JSON_VALUE(@Parameters, '$.startDate') AS DATE),
                @EndDate   = TRY_CAST(JSON_VALUE(@Parameters, '$.endDate')   AS DATE),
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

        -- Return findings grouped by clause
        SELECT
            c.ClauseId,
            c.ClauseNumber,
            c.ClauseTitle,
            fc2.CategoryName,
            COUNT(DISTINCT f.FindingId)                                         AS TotalFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 1 THEN 1 ELSE 0 END)             AS ClosedFindings,
            SUM(CASE WHEN fs.IsClosedStatus = 0 THEN 1 ELSE 0 END)             AS OpenFindings,
            SUM(CASE WHEN f.Severity = 'Critical' THEN 1 ELSE 0 END)           AS CriticalCount,
            SUM(CASE WHEN f.Severity = 'High'     THEN 1 ELSE 0 END)           AS HighCount,
            SUM(CASE WHEN f.Severity = 'Medium'   THEN 1 ELSE 0 END)           AS MediumCount,
            SUM(CASE WHEN f.Severity = 'Low'      THEN 1 ELSE 0 END)           AS LowCount
        FROM FindingClauses fclause
        INNER JOIN Findings f         ON fclause.FindingId = f.FindingId
        INNER JOIN Clauses c          ON fclause.ClauseId  = c.ClauseId
        INNER JOIN Audits a           ON f.AuditId         = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId  = fs.FindingStatusId
        LEFT  JOIN FindingCategories fc2 ON f.FindingCategoryId = fc2.FindingCategoryId
        WHERE f.IsActive = 1
          AND fclause.IsActive = 1
          AND (@CompanyId IS NULL OR a.companyId = @CompanyId)
          AND (@SiteId    IS NULL OR f.SiteId    = @SiteId)
          AND (@StartDate IS NULL OR CAST(f.IdentifiedDate AS DATE) >= @StartDate)
          AND (@EndDate   IS NULL OR CAST(f.IdentifiedDate AS DATE) <= @EndDate)
          AND (@UserId    IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        GROUP BY c.ClauseId, c.ClauseNumber, c.ClauseTitle, fc2.CategoryName
        ORDER BY TotalFindings DESC, c.ClauseNumber;

        RETURN;

        ErrorResponse:
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;

    END TRY
    BEGIN CATCH
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
