-- =============================================
-- Description: Get paginated list of findings with filtering
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetFindingList]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetFindingList];
GO
CREATE PROCEDURE [dbo].[Sp_GetFindingList]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId     INT = NULL;
    DECLARE @CompanyId  INT = NULL;
    DECLARE @SiteId     INT = NULL;
    DECLARE @StatusCode NVARCHAR(50) = NULL;
    DECLARE @CategoryId INT = NULL;
    DECLARE @PageNumber INT = 1;
    DECLARE @PageSize   INT = 100;
    DECLARE @ErrorCode  NVARCHAR(50) = '';
    DECLARE @Message    NVARCHAR(500) = '';

    BEGIN TRY
        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId     = JSON_VALUE(@Parameters, '$.userId'),
                @CompanyId  = JSON_VALUE(@Parameters, '$.companyId'),
                @SiteId     = JSON_VALUE(@Parameters, '$.siteId'),
                @StatusCode = JSON_VALUE(@Parameters, '$.status'),
                @CategoryId = JSON_VALUE(@Parameters, '$.categoryId'),
                @PageNumber = ISNULL(CAST(JSON_VALUE(@Parameters, '$.pageNumber') AS INT), 1),
                @PageSize   = ISNULL(CAST(JSON_VALUE(@Parameters, '$.pageSize')   AS INT), 100);
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

        -- Return total count
        SELECT COUNT(*) AS TotalCount
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        LEFT  JOIN FindingCategories fc ON f.FindingCategoryId = fc.FindingCategoryId
        LEFT  JOIN Sites s ON f.SiteId = s.SiteId
        WHERE f.IsActive = 1
          AND (@CompanyId  IS NULL OR a.companyId             = @CompanyId)
          AND (@SiteId     IS NULL OR f.SiteId                = @SiteId)
          AND (@StatusCode IS NULL OR fs.StatusCode           = @StatusCode)
          AND (@CategoryId IS NULL OR f.FindingCategoryId     = @CategoryId)
          AND (@UserId     IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ));

        -- Return paged data
        SELECT
            f.FindingId,
            f.FindingNumber,
            f.AuditId,
            f.SiteId,
            s.SiteName,
            f.Title,
            f.FindingType,
            f.Severity,
            f.FindingStatusId,
            fs.StatusName,
            fs.StatusCode,
            fs.Color        AS StatusColor,
            f.FindingCategoryId,
            fc.CategoryName,
            f.IdentifiedDate,
            f.DueDate,
            f.ClosedDate,
            f.IsActive,
            f.CreatedDate,
            f.ModifiedDate,
            a.companyId     AS CompanyId,
            a.type          AS AuditType
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        LEFT  JOIN FindingCategories fc ON f.FindingCategoryId = fc.FindingCategoryId
        LEFT  JOIN Sites s ON f.SiteId = s.SiteId
        WHERE f.IsActive = 1
          AND (@CompanyId  IS NULL OR a.companyId             = @CompanyId)
          AND (@SiteId     IS NULL OR f.SiteId                = @SiteId)
          AND (@StatusCode IS NULL OR fs.StatusCode           = @StatusCode)
          AND (@CategoryId IS NULL OR f.FindingCategoryId     = @CategoryId)
          AND (@UserId     IS NULL OR EXISTS (
                SELECT 1 FROM UserCompanyAccess uca
                WHERE uca.UserId = @UserId AND uca.CompanyId = a.companyId AND uca.IsActive = 1
              ))
        ORDER BY f.DueDate ASC, f.IdentifiedDate DESC
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        RETURN;

        ErrorResponse:
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;

    END TRY
    BEGIN CATCH
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
