-- =============================================
-- Description: Submit/post latest finding response (mutation)
-- =============================================
IF OBJECT_ID('[dbo].[Sp_PostLatestFindingResponse]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_PostLatestFindingResponse];
GO
CREATE PROCEDURE [dbo].[Sp_PostLatestFindingResponse]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId           INT = NULL;
    DECLARE @FindingNumber    NVARCHAR(255) = NULL;
    DECLARE @IsSubmitToDnv    BIT = 0;
    DECLARE @RootCause        NVARCHAR(MAX) = NULL;
    DECLARE @CorrectiveAction NVARCHAR(MAX) = NULL;
    DECLARE @ResponseText     NVARCHAR(MAX) = NULL;
    DECLARE @ErrorCode        NVARCHAR(50) = '';
    DECLARE @Message          NVARCHAR(500) = '';

    BEGIN TRY
        BEGIN TRANSACTION;

        IF ISJSON(@Parameters) = 1
        BEGIN
            SELECT
                @UserId           = JSON_VALUE(@Parameters, '$.userId'),
                @FindingNumber    = JSON_VALUE(@Parameters, '$.request.findingNumber'),
                @IsSubmitToDnv    = CASE WHEN JSON_VALUE(@Parameters, '$.request.isSubmitToDnv') = 'true' THEN 1 ELSE 0 END,
                @RootCause        = JSON_VALUE(@Parameters, '$.request.rootCause'),
                @CorrectiveAction = JSON_VALUE(@Parameters, '$.request.correctiveAction'),
                @ResponseText     = JSON_VALUE(@Parameters, '$.request.responseText');
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

        IF @UserId IS NULL
        BEGIN
            SET @ErrorCode = 'MISSING_USER';
            SET @Message   = 'User ID is required.';
            GOTO ErrorResponse;
        END

        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId AND IsActive = 1)
        BEGIN
            SET @ErrorCode = 'INVALID_USER';
            SET @Message   = 'User not found or inactive.';
            GOTO ErrorResponse;
        END

        DECLARE @FindingId     INT;
        DECLARE @StatusId      INT;
        DECLARE @IsClosedStatus BIT;

        SELECT
            @FindingId      = f.FindingId,
            @StatusId       = f.FindingStatusId,
            @IsClosedStatus = fs.IsClosedStatus
        FROM Findings f
        INNER JOIN FindingStatuses fs ON f.FindingStatusId = fs.FindingStatusId
        WHERE f.FindingNumber = @FindingNumber AND f.IsActive = 1;

        IF @FindingId IS NULL
        BEGIN
            SET @ErrorCode = 'FINDING_NOT_FOUND';
            SET @Message   = 'Finding not found.';
            GOTO ErrorResponse;
        END

        IF @IsClosedStatus = 1
        BEGIN
            SET @ErrorCode = 'FINDING_CLOSED';
            SET @Message   = 'Cannot submit response to a closed finding.';
            GOTO ErrorResponse;
        END

        -- Insert response
        DECLARE @NewResponseId INT;

        INSERT INTO FindingResponses (
            FindingId, ResponseText, ResponseType, ResponseDate,
            RespondedBy, IsSubmittedToDNV, SubmissionDate,
            IsActive, Status, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy
        )
        VALUES (
            @FindingId,
            ISNULL(@ResponseText, CONCAT(
                ISNULL('Root Cause: ' + @RootCause + CHAR(10), ''),
                ISNULL('Corrective Action: ' + @CorrectiveAction, '')
            )),
            CASE WHEN @IsSubmitToDnv = 1 THEN 'SUBMISSION' ELSE 'DRAFT' END,
            GETDATE(),
            @UserId,
            @IsSubmitToDnv,
            CASE WHEN @IsSubmitToDnv = 1 THEN GETDATE() ELSE NULL END,
            1,
            CASE WHEN @IsSubmitToDnv = 1 THEN 'SUBMITTED' ELSE 'DRAFT' END,
            GETDATE(),
            GETDATE(),
            @UserId,
            @UserId
        );

        SET @NewResponseId = SCOPE_IDENTITY();

        -- Update finding root cause / corrective action if provided
        IF @RootCause IS NOT NULL OR @CorrectiveAction IS NOT NULL
        BEGIN
            UPDATE Findings
            SET RootCause        = ISNULL(@RootCause, RootCause),
                CorrectiveAction = ISNULL(@CorrectiveAction, CorrectiveAction),
                ModifiedDate     = GETDATE(),
                ModifiedBy       = @UserId
            WHERE FindingId = @FindingId;
        END

        -- If submitted to DNV, move to Pending Verification status
        IF @IsSubmitToDnv = 1
        BEGIN
            DECLARE @PendingVerifStatusId INT;
            SELECT @PendingVerifStatusId = FindingStatusId
            FROM FindingStatuses
            WHERE StatusCode = 'PENDING_VERIFICATION';

            IF @PendingVerifStatusId IS NOT NULL
            BEGIN
                UPDATE Findings
                SET FindingStatusId = @PendingVerifStatusId,
                    ModifiedDate    = GETDATE(),
                    ModifiedBy      = @UserId
                WHERE FindingId = @FindingId;
            END
        END

        COMMIT TRANSACTION;

        -- Return created response
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
            CAST(1 AS BIT)  AS IsSuccess
        FROM FindingResponses fr
        LEFT JOIN Users u ON fr.RespondedBy = u.UserId
        WHERE fr.FindingResponseId = @NewResponseId;

        RETURN;

        ErrorResponse:
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT CAST(0 AS BIT) AS IsSuccess, @ErrorCode AS ErrorCode, @Message AS Message;
        RETURN;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT CAST(0 AS BIT) AS IsSuccess, 'SERVER_ERROR' AS ErrorCode, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
