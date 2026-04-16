-- =============================================
-- Description: Get manage finding details by finding number (full management view)
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetManageFindingDetails]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetManageFindingDetails];
GO
CREATE PROCEDURE [dbo].[Sp_GetManageFindingDetails]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId        INT = NULL;
    DECLARE @FindingNumber NVARCHAR(255) = NULL;
    DECLARE @ErrorCode     NVARCHAR(50) = '';
    DECLARE @Message       NVARCHAR(500) = '';

    BEGIN TRY
        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId        = JSON_VALUE(@Parameters, '$.userId'),
                @FindingNumber = JSON_VALUE(@Parameters, '$.findingNumber');
        END
        ELSE
        BEGIN
            SET @ErrorCode = 'INVALID_JSON';
            SET @Message   = 'Invalid JSON format.';
            GOTO ErrorResponse;
        END

        IF @FindingNumber IS NULL OR LTRIM(RTRIM(@FindingNumber)) = ''
        BEGIN
            SET @ErrorCode = 'MISSING_FINDING_NUMBER';
            SET @Message   = 'Finding number is required.';
            GOTO ErrorResponse;
        END

        IF @UserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId AND IsActive = 1)
        BEGIN
            SET @ErrorCode = 'INVALID_USER';
            SET @Message   = 'User not found or inactive.';
            GOTO ErrorResponse;
        END

        DECLARE @FindingId INT;

        SELECT TOP 1
            @FindingId = f.FindingId
        FROM Findings f
        WHERE f.FindingNumber = @FindingNumber AND f.IsActive = 1;

        IF @FindingId IS NULL
        BEGIN
            SET @ErrorCode = 'FINDING_NOT_FOUND';
            SET @Message   = 'Finding not found or inactive.';
            GOTO ErrorResponse;
        END

        -- Full finding details
        SELECT
            f.FindingId,
            f.FindingNumber,
            f.AuditId,
            f.SiteId,
            s.SiteName,
            s.Location          AS SiteLocation,
            f.Title,
            f.Description,
            f.FindingType,
            f.Severity,
            f.FindingStatusId,
            fs.StatusName,
            fs.StatusCode,
            fs.Color            AS StatusColor,
            fs.IsClosedStatus,
            f.FindingCategoryId,
            fc.CategoryName,
            fc.CategoryCode,
            f.IdentifiedDate,
            f.DueDate,
            f.ClosedDate,
            f.CompletionDate,
            f.VerificationDate,
            f.IsActive,
            f.Evidence,
            f.RootCause,
            f.CorrectiveAction,
            f.PreventiveAction,
            f.VerificationMethod,
            f.CreatedDate,
            f.ModifiedDate,
            f.IdentifiedBy,
            uid_by.Username     AS IdentifiedByName,
            f.AssignedTo,
            u_assigned.Username AS AssignedToName,
            f.VerifiedBy,
            u_verified.Username AS VerifiedByName,
            a.companyId         AS CompanyId,
            a.type              AS AuditType,
            a.status            AS AuditStatus,
            a.startDate         AS AuditStartDate,
            a.endDate           AS AuditEndDate
        FROM Findings f
        INNER JOIN Audits a ON f.AuditId = a.auditId
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        LEFT  JOIN FindingCategories fc ON f.FindingCategoryId = fc.FindingCategoryId
        LEFT  JOIN Sites s ON f.SiteId = s.SiteId
        LEFT  JOIN Users uid_by    ON f.IdentifiedBy = uid_by.UserId
        LEFT  JOIN Users u_assigned ON f.AssignedTo  = u_assigned.UserId
        LEFT  JOIN Users u_verified ON f.VerifiedBy  = u_verified.UserId
        WHERE f.FindingId = @FindingId;

        -- Related clauses
        SELECT
            fclause.FindingClauseId,
            fclause.ClauseId,
            c.ClauseNumber,
            c.ClauseTitle,
            fclause.Notes
        FROM FindingClauses fclause
        INNER JOIN Clauses c ON fclause.ClauseId = c.ClauseId
        WHERE fclause.FindingId = @FindingId AND fclause.IsActive = 1
        ORDER BY c.ClauseNumber;

        -- Focus areas
        SELECT
            ffa.FindingFocusAreaId,
            ffa.FocusAreaId,
            fa.FocusAreaName,
            fa.FocusAreaCode,
            ffa.Notes
        FROM FindingFocusAreas ffa
        INNER JOIN FocusAreas fa ON ffa.FocusAreaId = fa.FocusAreaId
        WHERE ffa.FindingId = @FindingId AND ffa.IsActive = 1
        ORDER BY fa.FocusAreaName;

        -- Latest response
        SELECT TOP 1
            fr.FindingResponseId,
            fr.ResponseText,
            fr.ResponseType,
            fr.ResponseDate,
            fr.RespondedBy,
            u.Username      AS RespondedByName,
            fr.IsSubmittedToDNV,
            fr.SubmissionDate,
            fr.Status,
            fr.ReviewComments,
            fr.ReviewedBy,
            rv.Username     AS ReviewedByName,
            fr.ReviewDate
        FROM FindingResponses fr
        LEFT JOIN Users u  ON fr.RespondedBy = u.UserId
        LEFT JOIN Users rv ON fr.ReviewedBy  = rv.UserId
        WHERE fr.FindingId = @FindingId AND fr.IsActive = 1
        ORDER BY fr.ResponseDate DESC;

        RETURN;

        ErrorResponse:
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;

    END TRY
    BEGIN CATCH
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
