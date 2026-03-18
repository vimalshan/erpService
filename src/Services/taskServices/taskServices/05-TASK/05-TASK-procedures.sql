-- ==========================================
-- Module: TASK MANAGEMENT
-- Database: TASKDB
-- Purpose: Task Mail & Notification Management Procedures & Functions
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_TASK_CreateTaskMail
-- Purpose:  Create and assign task with email notification
-- Parameters:
--   @p_MailID - Mail/Task ID
--   @p_SysID - System user ID to assign task
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_TASK_CreateTaskMail
(
    @p_MailID DECIMAL(38),
    @p_SysID DECIMAL(38)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS(SELECT 1 FROM TASK_MAIL WHERE MID = @p_MailID)
        BEGIN
            INSERT INTO TASK_MAIL
            (
                MID, SYSID
            )
            VALUES
            (
                @p_MailID, @p_SysID
            );
        END
        
        COMMIT TRANSACTION;
        PRINT 'Task mail created: MailID ' + CAST(@p_MailID AS VARCHAR) + ', AssignedTo: ' + CAST(@p_SysID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Task mail creation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT - TASK MODULE PROCEDURES
-- ==========================================
