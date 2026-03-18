-- ==========================================
-- WORKORDER Module - Stored Procedures
-- Database: MYWORKDB
-- Module: WORKORDER
-- Description: Work Order and Task Management Procedures
-- Created: March 9, 2026
-- ==========================================

USE MYWORKDB;
GO

-- =====================================================
-- WORKORDER Utility Functions
-- =====================================================

IF OBJECT_ID('dbo.fn_GetTaskCompletionPercentage', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetTaskCompletionPercentage;
GO

-- Function: fn_GetTaskCompletionPercentage
-- Purpose: Calculate the completion percentage of tasks in a work order
-- Parameters:
--   @p_WorkOrderID: Work Order ID
-- Returns: Completion percentage (0-100)
CREATE FUNCTION dbo.fn_GetTaskCompletionPercentage (@p_WorkOrderID BIGINT)
RETURNS INT
AS BEGIN
    DECLARE @Percentage INT = 0, @Completed INT, @Total INT;
    BEGIN TRY
        SELECT @Completed = COUNT(*) FROM dbo.WORK_TASK WHERE WORK_ORDER_ID = @p_WorkOrderID AND TASK_STATUS = 'C';
        SELECT @Total = COUNT(*) FROM dbo.WORK_TASK WHERE WORK_ORDER_ID = @p_WorkOrderID;
        IF @Total > 0
            SET @Percentage = CAST((@Completed * 100.0) / @Total AS INT);
    END TRY BEGIN CATCH SET @Percentage = 0; END CATCH
    RETURN @Percentage;
END;
GO

-- =====================================================
-- WORKORDER Creation Procedures
-- =====================================================

IF OBJECT_ID('dbo.usp_CreateWorkOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateWorkOrder;
GO

-- Procedure: usp_CreateWorkOrder
-- Purpose: Create a new work order
-- Parameters:
--   @p_WorkOrderName: Name of the work order
--   @p_Description: Description of the work order
--   @p_DueDate: Due date for completion
--   @p_AssignedTo: Employee System ID to assign the work order
--   @p_CreatedBy: Employee System ID creating the work order
--   @p_WorkOrderID: Output - newly created work order ID
CREATE PROCEDURE dbo.usp_CreateWorkOrder
    @p_WorkOrderName VARCHAR(200), 
    @p_Description VARCHAR(500), 
    @p_DueDate DATE,
    @p_AssignedTo BIGINT, 
    @p_CreatedBy BIGINT, 
    @p_WorkOrderID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.WORK_ORDER (
            WORK_ORDER_NAME, 
            WORK_ORDER_DESCRIPTION, 
            DUE_DATE, 
            ASSIGNED_TO, 
            WORK_ORDER_STATUS, 
            CREATED_BY, 
            CREATED_ON
        )
        VALUES (
            @p_WorkOrderName, 
            @p_Description, 
            @p_DueDate, 
            @p_AssignedTo, 
            'O',  -- Open status
            @p_CreatedBy, 
            GETDATE()
        );
        
        SET @p_WorkOrderID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
        
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

-- =====================================================
-- WORKTASK Management Procedures
-- =====================================================

IF OBJECT_ID('dbo.usp_AssignTaskToWorkOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AssignTaskToWorkOrder;
GO

-- Procedure: usp_AssignTaskToWorkOrder
-- Purpose: Assign a task to a work order
-- Parameters:
--   @p_WorkOrderID: Work Order ID
--   @p_TaskName: Name of the task
--   @p_AssignedTo: Employee System ID to assign the task
--   @p_EstimatedHours: Estimated hours required
--   @p_CreatedBy: Employee System ID creating the task
--   @p_TaskID: Output - newly created task ID
CREATE PROCEDURE dbo.usp_AssignTaskToWorkOrder
    @p_WorkOrderID BIGINT, 
    @p_TaskName VARCHAR(100), 
    @p_AssignedTo BIGINT,
    @p_EstimatedHours INT, 
    @p_CreatedBy BIGINT, 
    @p_TaskID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.WORK_TASK (
            WORK_ORDER_ID, 
            TASK_NAME, 
            ASSIGNED_TO, 
            ESTIMATED_HOURS, 
            TASK_STATUS, 
            CREATED_BY, 
            CREATED_ON
        )
        VALUES (
            @p_WorkOrderID, 
            @p_TaskName, 
            @p_AssignedTo, 
            @p_EstimatedHours, 
            'O',  -- Open status
            @p_CreatedBy, 
            GETDATE()
        );
        
        SET @p_TaskID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
        
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

-- =====================================================
-- WORKTASK Completion Procedures
-- =====================================================

IF OBJECT_ID('dbo.usp_CompleteTask', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CompleteTask;
GO

-- Procedure: usp_CompleteTask
-- Purpose: Mark a task as completed
-- Parameters:
--   @p_TaskID: Task ID to complete
--   @p_ActualHours: Actual hours spent
--   @p_CompletionRemarks: Remarks on completion
--   @p_CompletedBy: Employee System ID completing the task
CREATE PROCEDURE dbo.usp_CompleteTask
    @p_TaskID BIGINT, 
    @p_ActualHours INT, 
    @p_CompletionRemarks VARCHAR(500), 
    @p_CompletedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.WORK_TASK
        SET 
            TASK_STATUS = 'C',  -- Completed status
            ACTUAL_HOURS = @p_ActualHours, 
            COMPLETION_REMARKS = @p_CompletionRemarks,
            COMPLETED_BY = @p_CompletedBy, 
            COMPLETED_ON = GETDATE(),
            UPDATED_BY = @p_CompletedBy,
            UPDATED_ON = GETDATE()
        WHERE TASK_ID = @p_TaskID;
        
        COMMIT TRANSACTION;
        
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

PRINT 'WORKORDER Module - Stored Procedures created successfully.';
GO
