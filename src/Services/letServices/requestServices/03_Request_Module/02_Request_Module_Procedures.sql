-- ==========================================
-- REQUEST MODULE - Stored Procedures
-- Database: LETDB
-- Purpose: Request Management procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_Request_CreateTrainingRequest
-- Purpose: Create a new training request
IF OBJECT_ID('dbo.usp_Request_CreateTrainingRequest', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Request_CreateTrainingRequest;
GO
CREATE PROCEDURE dbo.usp_Request_CreateTrainingRequest
    @p_ReqID BIGINT,
    @p_EmpUser VARCHAR(25),
    @p_ReqDate DATETIME2(3),
    @p_SupervisorUser VARCHAR(25),
    @p_ReqID_OUTPUT BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.REQUEST_MAIN (
            RQ_REQ_ID,
            RQ_EMP_USR,
            RQ_REQ_DAT,
            RQ_SUP_USR
        )
        VALUES (
            @p_ReqID,
            @p_EmpUser,
            @p_ReqDate,
            @p_SupervisorUser
        );
        
        SET @p_ReqID_OUTPUT = @p_ReqID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Request_ApproveRequest
-- Purpose: Approve a training request
IF OBJECT_ID('dbo.usp_Request_ApproveRequest', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Request_ApproveRequest;
GO
CREATE PROCEDURE dbo.usp_Request_ApproveRequest
    @p_ReqID BIGINT,
    @p_SrlNum BIGINT,
    @p_AppNum BIGINT,
    @p_ApprovalDate DATETIME2(3),
    @p_ApprovalRemark VARCHAR(200),
    @p_AppUser CHAR(20)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.REQUEST_APP (
            RQ_REQ_ID,
            RQ_SRL_NUM,
            RQ_APP_DAT,
            RQ_APP_NUM,
            RQ_APP_REM,
            RQ_APP_USR
        )
        VALUES (
            @p_ReqID,
            @p_SrlNum,
            @p_ApprovalDate,
            @p_AppNum,
            @p_ApprovalRemark,
            @p_AppUser
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Request_CancelRequest
-- Purpose: Cancel a training request
IF OBJECT_ID('dbo.usp_Request_CancelRequest', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Request_CancelRequest;
GO
CREATE PROCEDURE dbo.usp_Request_CancelRequest
    @p_ReqID BIGINT,
    @p_SrlNum BIGINT,
    @p_CancellationDate DATETIME2(3),
    @p_CancellationRemark VARCHAR(255)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.REQUEST_SUB
        SET RQ_CAN_DAT = @p_CancellationDate,
            RQ_CAN_REM = @p_CancellationRemark
        WHERE RQ_REQ_ID = @p_ReqID
            AND RQ_SRL_NUM = @p_SrlNum;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Request_GetRequestDetails
-- Purpose: Retrieve request details
IF OBJECT_ID('dbo.usp_Request_GetRequestDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Request_GetRequestDetails;
GO
CREATE PROCEDURE dbo.usp_Request_GetRequestDetails
    @p_ReqID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.RQ_REQ_ID,
        m.RQ_EMP_USR,
        m.RQ_REQ_DAT,
        m.RQ_SUP_USR,
        s.RQ_SRL_NUM,
        s.RQ_TRN_NED,
        s.RQ_STS_COD,
        s.RQ_CRS_ID,
        s.RQ_STR_DAT,
        s.RQ_END_DAT
    FROM dbo.REQUEST_MAIN m
    LEFT JOIN dbo.REQUEST_SUB s ON m.RQ_REQ_ID = s.RQ_REQ_ID
    WHERE m.RQ_REQ_ID = @p_ReqID;
END;
GO

-- Procedure: usp_Request_GetPendingRequests
-- Purpose: Retrieve pending requests for approval
IF OBJECT_ID('dbo.usp_Request_GetPendingRequests', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Request_GetPendingRequests;
GO
CREATE PROCEDURE dbo.usp_Request_GetPendingRequests
    @p_SupervisorUser VARCHAR(25)
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.RQ_REQ_ID,
        m.RQ_EMP_USR,
        m.RQ_REQ_DAT,
        s.RQ_TRN_NED,
        s.RQ_STS_COD
    FROM dbo.REQUEST_MAIN m
    INNER JOIN dbo.REQUEST_SUB s ON m.RQ_REQ_ID = s.RQ_REQ_ID
    WHERE m.RQ_SUP_USR = @p_SupervisorUser
        AND s.RQ_STS_COD IN ('P', 'S')  -- Pending or Submitted
    ORDER BY m.RQ_REQ_DAT DESC;
END;
GO

PRINT 'Request Module Procedures created successfully.';
GO
