-- ==========================================
-- Module: COMPLAINT MANAGEMENT
-- Database: TASKDB
-- Purpose: Complaint/NCR Ticket Management Procedures & Functions
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetComplaintStatus
-- Purpose:  Get current complaint/NCR status with escalation level
-- Parameters: @p_TicketNum - Complaint ticket number
-- Returns: VARCHAR(50) - Current status
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetComplaintStatus
(
    @p_TicketNum DECIMAL(38)
)
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @Status VARCHAR(50);
    DECLARE @CurrentLevel DECIMAL(38);
    DECLARE @TargetDate DATETIME2(3);
    DECLARE @HoursElapsed INT;
    
    SELECT @CurrentLevel = CA_CUR_ESCLEVEL, @TargetDate = CD_TARGET_DATE
    FROM COMPL_ACTION CA
    INNER JOIN COMPL_DET CD ON CA.CA_TASK_NUM = CD.CD_TICKET_NUM
    WHERE CD.CD_TICKET_NUM = @p_TicketNum;
    
    -- Calculate escalation status
    SET @HoursElapsed = DATEDIFF(HOUR, @TargetDate, GETDATE());
    
    IF @HoursElapsed < 0
        SET @Status = 'On Target';
    ELSE IF @HoursElapsed < 24
        SET @Status = 'At Risk';
    ELSE IF @HoursElapsed < 48
        SET @Status = 'Escalated - Level 1';
    ELSE
        SET @Status = 'Escalated - Level 2+';
    
    RETURN ISNULL(@Status, 'Unknown');
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_COMPLAINT_CreateComplaint
-- Purpose:  Register new complaint/NCR ticket
-- Parameters:
--   @p_GroupID - Complaint group ID
--   @p_Type - Complaint type
--   @p_Location - Location code
--   @p_Department - Department code
--   @p_Subject - Complaint subject
--   @p_Description - Detailed description
--   @p_IsNCR - Flag indicating if NCR (Y/N)
--   @p_TargetResolutionHours - Target resolution time in hours
--   @p_CreatedBy - User ID creating complaint
--   @p_TicketID - Output parameter for generated ticket ID
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_COMPLAINT_CreateComplaint
(
    @p_GroupID VARCHAR(255),
    @p_Type DECIMAL(38),
    @p_Location DECIMAL(38),
    @p_Department DECIMAL(38),
    @p_Subject VARCHAR(500),
    @p_Description VARCHAR(4000),
    @p_IsNCR CHAR(1) = 'N',
    @p_TargetResolutionHours INT = 48,
    @p_CreatedBy DECIMAL(38),
    @p_TicketID DECIMAL(38) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate ticket ID
        SELECT @p_TicketID = ISNULL(MAX(CD_TICKET_NUM), 0) + 1 FROM COMPL_DET;
        
        -- Create complaint detail
        INSERT INTO COMPL_DET
        (
            CD_TICKET_NUM, CD_GROUPID, CD_TYPE, CD_LOCATION, CD_DEPARTMENT,
            CD_SUBJECT, CD_DESCRIPTION, CD_NCR, CD_TARGET_DATE
        )
        VALUES
        (
            @p_TicketID, @p_GroupID, @p_Type, @p_Location, @p_Department,
            @p_Subject, @p_Description, @p_IsNCR,
            DATEADD(HOUR, @p_TargetResolutionHours, GETDATE())
        );
        
        -- Create action record
        DECLARE @ActionNum DECIMAL(38);
        SELECT @ActionNum = ISNULL(MAX(CA_ACTION_NUM), 0) + 1 FROM COMPL_ACTION;
        
        INSERT INTO COMPL_ACTION
        (
            CA_ACTION_NUM, CA_TASK_NUM, CA_TRG_DAT, CA_CUR_ESCLEVEL
        )
        VALUES
        (
            @ActionNum, @p_TicketID, GETDATE(), 0
        );
        
        -- Create history record
        DECLARE @HistoryNum DECIMAL(38);
        SELECT @HistoryNum = ISNULL(MAX(CH_HISTORY_NUM), 0) + 1 FROM COMPL_HIST;
        
        INSERT INTO COMPL_HIST
        (
            CH_HISTORY_NUM, CH_ACTION_NUM, CH_SERIAL_NUM, CH_FROM, CH_TO,
            CH_ACTION_DATE, CH_ACTION_TYPE, CH_REMARKS
        )
        VALUES
        (
            @HistoryNum, @ActionNum, 1, 'Open', 'New Ticket', GETDATE(), 'O', @p_Subject
        );
        
        COMMIT TRANSACTION;
        PRINT 'Complaint/NCR created: Ticket #' + CAST(@p_TicketID AS VARCHAR) + 
              ', Target Resolution: ' + CAST(@p_TargetResolutionHours AS VARCHAR) + ' hours';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Complaint creation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_COMPLAINT_UpdateAction
-- Purpose:  Record action/resolution for complaint at different escalation levels
-- Parameters:
--   @p_ActionNum - Action number from COMPL_ACTION
--   @p_ActionLevel - Action level (P=Primary, S=Secondary, F=Forward, C=Corrective)
--   @p_Solution - Solution text
--   @p_ActionBy - User ID taking action
--   @p_ActionDate - Date of action (defaults to current)
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_COMPLAINT_UpdateAction
(
    @p_ActionNum DECIMAL(38),
    @p_ActionLevel CHAR(1),
    @p_Solution VARCHAR(4000),
    @p_ActionBy DECIMAL(38),
    @p_ActionDate DATETIME2(3) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ActionDate DATETIME2(3) = ISNULL(@p_ActionDate, GETDATE());
        
        -- Update action based on level
        IF @p_ActionLevel = 'P'
        BEGIN
            UPDATE COMPL_ACTION
            SET CA_PRM_ACTBY = @p_ActionBy,
                CA_PRM_ACTDATE = @ActionDate,
                CA_PRM_SOLUTION = @p_Solution
            WHERE CA_ACTION_NUM = @p_ActionNum;
        END
        ELSE IF @p_ActionLevel = 'S'
        BEGIN
            UPDATE COMPL_ACTION
            SET CA_SEC_ACTBY = @p_ActionBy,
                CA_SEC_ACTDATE = @ActionDate,
                CA_SEC_SOLUTION = @p_Solution
            WHERE CA_ACTION_NUM = @p_ActionNum;
        END
        ELSE IF @p_ActionLevel = 'F'
        BEGIN
            UPDATE COMPL_ACTION
            SET CA_FWD_ACTBY = @p_ActionBy,
                CA_FWD_ACTDATE = @ActionDate,
                CA_FWD_SOLUTION = @p_Solution
            WHERE CA_ACTION_NUM = @p_ActionNum;
        END
        ELSE IF @p_ActionLevel = 'C'
        BEGIN
            UPDATE COMPL_ACTION
            SET CA_CORR_ACTBY = @p_ActionBy,
                CA_CORR_ACTDATE = @ActionDate,
                CA_CORR_SOLUTION = @p_Solution
            WHERE CA_ACTION_NUM = @p_ActionNum;
        END
        
        COMMIT TRANSACTION;
        PRINT 'Action recorded for level: ' + @p_ActionLevel;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Action update failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_COMPLAINT_CloseComplaint
-- Purpose:  Close complaint with final resolution and audit trail
-- Parameters:
--   @p_TicketID - Complaint ticket number
--   @p_FinalRemarks - Final remarks on closure
--   @p_ClosedBy - User ID closing complaint
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_COMPLAINT_CloseComplaint
(
    @p_TicketID DECIMAL(38),
    @p_FinalRemarks VARCHAR(500) = NULL,
    @p_ClosedBy DECIMAL(38)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Update complaint detail with closure date
        UPDATE COMPL_DET
        SET CD_CLOSURE_DATE = GETDATE()
        WHERE CD_TICKET_NUM = @p_TicketID;
        
        -- Update action record with closure date
        UPDATE COMPL_ACTION
        SET CA_CLS_DAT = GETDATE()
        WHERE CA_TASK_NUM = @p_TicketID;
        
        -- Create final history record
        DECLARE @ActionNum DECIMAL(38);
        DECLARE @HistoryNum DECIMAL(38);
        SELECT @ActionNum = CA_ACTION_NUM FROM COMPL_ACTION WHERE CA_TASK_NUM = @p_TicketID;
        SELECT @HistoryNum = ISNULL(MAX(CH_HISTORY_NUM), 0) + 1 FROM COMPL_HIST;
        
        INSERT INTO COMPL_HIST
        (
            CH_HISTORY_NUM, CH_ACTION_NUM, CH_SERIAL_NUM, CH_FROM, CH_TO,
            CH_ACTION_DATE, CH_ACTION_TYPE, CH_REMARKS
        )
        VALUES
        (
            @HistoryNum, @ActionNum, (SELECT MAX(CH_SERIAL_NUM) + 1 FROM COMPL_HIST WHERE CH_ACTION_NUM = @ActionNum),
            'In Progress', 'Closed', GETDATE(), 'C', @p_FinalRemarks
        );
        
        COMMIT TRANSACTION;
        PRINT 'Complaint closed: Ticket #' + CAST(@p_TicketID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Complaint closure failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT - COMPLAINT MODULE PROCEDURES
-- ==========================================
