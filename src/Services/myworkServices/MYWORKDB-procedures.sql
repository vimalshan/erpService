-- MYWORKDB Stored Procedures, Functions & Triggers
-- Work Order & Project Task Management System
-- Created: February 13, 2026

USE MYWORKDB;
GO

IF OBJECT_ID('dbo.fn_GetTaskCompletionPercentage', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetTaskCompletionPercentage;
GO
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

IF OBJECT_ID('dbo.usp_CreateWorkOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateWorkOrder;
GO
CREATE PROCEDURE dbo.usp_CreateWorkOrder
    @p_WorkOrderName VARCHAR(200), @p_Description VARCHAR(500), @p_DueDate DATE,
    @p_AssignedTo BIGINT, @p_CreatedBy BIGINT, @p_WorkOrderID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.WORK_ORDER (WORK_ORDER_NAME, WORK_ORDER_DESCRIPTION, DUE_DATE, ASSIGNED_TO, 
            WORK_ORDER_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_WorkOrderName, @p_Description, @p_DueDate, @p_AssignedTo, 'O', @p_CreatedBy, GETDATE());
        SET @p_WorkOrderID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_AssignTaskToWorkOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AssignTaskToWorkOrder;
GO
CREATE PROCEDURE dbo.usp_AssignTaskToWorkOrder
    @p_WorkOrderID BIGINT, @p_TaskName VARCHAR(100), @p_AssignedTo BIGINT,
    @p_EstimatedHours INT, @p_CreatedBy BIGINT, @p_TaskID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.WORK_TASK (WORK_ORDER_ID, TASK_NAME, ASSIGNED_TO, ESTIMATED_HOURS, TASK_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_WorkOrderID, @p_TaskName, @p_AssignedTo, @p_EstimatedHours, 'O', @p_CreatedBy, GETDATE());
        SET @p_TaskID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_CompleteTask', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CompleteTask;
GO
CREATE PROCEDURE dbo.usp_CompleteTask
    @p_TaskID BIGINT, @p_ActualHours INT, @p_CompletionRemarks VARCHAR(500), @p_CompletedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.WORK_TASK
        SET TASK_STATUS = 'C', ACTUAL_HOURS = @p_ActualHours, COMPLETION_REMARKS = @p_CompletionRemarks,
            COMPLETED_BY = @p_CompletedBy, COMPLETED_ON = GETDATE()
        WHERE TASK_ID = @p_TaskID;
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'MYWORKDB Procedures created successfully.';
GO
