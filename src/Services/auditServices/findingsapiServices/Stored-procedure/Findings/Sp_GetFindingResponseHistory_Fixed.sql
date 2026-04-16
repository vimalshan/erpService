-- =============================================
-- Description: Get finding response history for a specific finding
-- =============================================
IF OBJECT_ID('[dbo].[Sp_GetFindingResponseHistory]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetFindingResponseHistory];
GO
CREATE PROCEDURE [dbo].[Sp_GetFindingResponseHistory]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId        INT = NULL;
    DECLARE @FindingNumber NVARCHAR(100);
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
            SET @ErrorCode = 'MISSING_PARAMETERS';
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

        SELECT @FindingId = f.FindingId
        FROM Findings f
        WHERE f.FindingNumber = @FindingNumber AND f.IsActive = 1;

        IF @FindingId IS NULL
        BEGIN
            SET @ErrorCode = 'FINDING_NOT_FOUND';
            SET @Message   = 'Finding not found.';
            GOTO ErrorResponse;
        END

        -- Return finding summary
        SELECT
            f.FindingId,
            f.FindingNumber,
            f.Title,
            f.FindingType,
            f.Severity,
            fs.StatusName,
            fs.StatusCode,
            f.IdentifiedDate,
            f.DueDate,
            f.ClosedDate
        FROM Findings f
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        WHERE f.FindingId = @FindingId;

        -- Return response history ordered by date desc
        SELECT
            fr.FindingResponseId,
            fr.FindingId,
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
            fr.ReviewDate,
            fr.AttachmentPath,
            fr.IsActive,
            fr.CreatedDate
        FROM FindingResponses fr
        LEFT JOIN Users u  ON fr.RespondedBy = u.UserId
        LEFT JOIN Users rv ON fr.ReviewedBy  = rv.UserId
        WHERE fr.FindingId = @FindingId
          AND fr.IsActive = 1
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
