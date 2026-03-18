-- ==========================================
-- DEVELOPMENT MODULE - Stored Procedures
-- Database: LETDB
-- Purpose: Development Plan procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_Development_CreateLearningPlan
-- Purpose: Create a learning and development plan
IF OBJECT_ID('dbo.usp_Development_CreateLearningPlan', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Development_CreateLearningPlan;
GO
CREATE PROCEDURE dbo.usp_Development_CreateLearningPlan
    @p_ReqNum BIGINT,
    @p_UserID VARCHAR(255),
    @p_PinNum BIGINT,
    @p_DevSource VARCHAR(255),
    @p_DevNeed VARCHAR(255),
    @p_Priority BIGINT,
    @p_EntDate DATETIME2(3),
    @p_ReqNum_OUTPUT BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.DD_LETPLAN (
            DD_REQNUM,
            DD_USERID,
            DD_PINNUM,
            DD_DEVSOURCE,
            DD_DEVNEED,
            DD_PRIORITY,
            DD_ENTDATE,
            DD_APPSTATUS
        )
        VALUES (
            @p_ReqNum,
            @p_UserID,
            @p_PinNum,
            @p_DevSource,
            @p_DevNeed,
            @p_Priority,
            @p_EntDate,
            'F'  -- Appraisee status
        );
        
        SET @p_ReqNum_OUTPUT = @p_ReqNum;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Development_ApprovePlan
-- Purpose: Approve a learning plan
IF OBJECT_ID('dbo.usp_Development_ApprovePlan', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Development_ApprovePlan;
GO
CREATE PROCEDURE dbo.usp_Development_ApprovePlan
    @p_ReqNum BIGINT,
    @p_AppStatus CHAR(1),
    @p_BHRStatus CHAR(1) = NULL
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.DD_LETPLAN
        SET DD_APPSTATUS = @p_AppStatus,
            DD_BHRSTATUS = ISNULL(@p_BHRStatus, DD_BHRSTATUS)
        WHERE DD_REQNUM = @p_ReqNum;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Development_CreateBHRPlan
-- Purpose: Create a BHR approved training plan
IF OBJECT_ID('dbo.usp_Development_CreateBHRPlan', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Development_CreateBHRPlan;
GO
CREATE PROCEDURE dbo.usp_Development_CreateBHRPlan
    @p_ReqNum BIGINT,
    @p_UserID VARCHAR(255),
    @p_TrainingProgram VARCHAR(255),
    @p_TrainingCode DECIMAL(38),
    @p_Priority DECIMAL(38),
    @p_BHRAccept CHAR(1)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.DD_LETBHRPLAN (
            DD_REQNUM,
            DD_USERID,
            DD_TRAININGPROGRAM,
            DD_TRAININGCODE,
            DD_PRIORITY,
            DD_BHRACCEPT
        )
        VALUES (
            @p_ReqNum,
            @p_UserID,
            @p_TrainingProgram,
            @p_TrainingCode,
            @p_Priority,
            @p_BHRAccept
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Development_GetPlans
-- Purpose: Retrieve learning plans
IF OBJECT_ID('dbo.usp_Development_GetPlans', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Development_GetPlans;
GO
CREATE PROCEDURE dbo.usp_Development_GetPlans
    @p_UserID VARCHAR(255) = NULL,
    @p_Status CHAR(1) = NULL
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        DD_REQNUM,
        DD_USERID,
        DD_DEVNEED,
        DD_DEVSOURCE,
        DD_TRAININGPROGRAM,
        DD_PRIORITY,
        DD_APPSTATUS,
        DD_BHRSTATUS,
        DD_ENTDATE
    FROM dbo.DD_LETPLAN
    WHERE (@p_UserID IS NULL OR DD_USERID = @p_UserID)
        AND (@p_Status IS NULL OR DD_APPSTATUS = @p_Status)
    ORDER BY DD_PRIORITY DESC, DD_ENTDATE DESC;
END;
GO

-- Procedure: usp_Development_GetCompetencyIndicators
-- Purpose: Retrieve competency indicators
IF OBJECT_ID('dbo.usp_Development_GetCompetencyIndicators', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Development_GetCompetencyIndicators;
GO
CREATE PROCEDURE dbo.usp_Development_GetCompetencyIndicators
    @p_CompNum BIGINT = NULL,
    @p_Band VARCHAR(50) = NULL
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        SRL_NO,
        BAND,
        COMP_NUM,
        IND_FLAG,
        IND_DEFN
    FROM dbo.DD_COMPETENCY_IND
    WHERE (@p_CompNum IS NULL OR COMP_NUM = @p_CompNum)
        AND (@p_Band IS NULL OR BAND = @p_Band);
END;
GO

PRINT 'Development Module Procedures created successfully.';
GO
