-- Module: DemandManagement Procedures
USE DDDB;
GO

CREATE FUNCTION dbo.fn_GetDemandStatusCount (@p_Status CHAR(1))
RETURNS INT
AS BEGIN
    DECLARE @Count INT = 0;
    BEGIN TRY
        SELECT @Count = COUNT(*) FROM dbo.DEMAND_MASTER WHERE DEMAND_STATUS = @p_Status;
    END TRY BEGIN CATCH SET @Count = 0; END CATCH
    RETURN @Count;
END;
GO


CREATE PROCEDURE dbo.usp_CreateDemand
    @p_DemandType VARCHAR(50), @p_DepartmentID BIGINT, @p_Description VARCHAR(500), 
    @p_RequiredDate DATE, @p_Priority VARCHAR(10), @p_CreatedBy BIGINT, @p_DemandID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.DEMAND_MASTER (DEMAND_TYPE, DEPARTMENT_ID, DEMAND_DESCRIPTION, REQUIRED_DATE, 
            PRIORITY, DEMAND_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_DemandType, @p_DepartmentID, @p_Description, @p_RequiredDate, @p_Priority, 'O', @p_CreatedBy, GETDATE());
        SET @p_DemandID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO


CREATE PROCEDURE dbo.usp_ProcessDemandApproval
    @p_DemandID BIGINT, @p_ApprovalStatus CHAR(1),  -- 'A' = Approved, 'R' = Rejected
    @p_ApprovalRemarks VARCHAR(500), @p_ApprovedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF @p_ApprovalStatus NOT IN ('A', 'R')
            THROW 50001, 'Invalid approval status', 1;
        UPDATE dbo.DEMAND_MASTER
        SET DEMAND_STATUS = @p_ApprovalStatus, APPROVAL_REMARKS = @p_ApprovalRemarks,
            APPROVED_BY = @p_ApprovedBy, APPROVAL_DATE = GETDATE()
        WHERE DEMAND_ID = @p_DemandID;
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO


CREATE PROCEDURE dbo.usp_RecordDemandCompletion
    @p_DemandID BIGINT, @p_CompletionRemarks VARCHAR(500), @p_CompletedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.DEMAND_MASTER
        SET DEMAND_STATUS = 'C', COMPLETION_REMARKS = @p_CompletionRemarks,
            COMPLETED_BY = @p_CompletedBy, COMPLETION_DATE = GETDATE()
        WHERE DEMAND_ID = @p_DemandID;
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO
