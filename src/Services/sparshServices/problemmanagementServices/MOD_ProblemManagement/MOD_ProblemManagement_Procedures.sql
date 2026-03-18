-- ============================================================================
-- Module: Problem Management - Stored Procedures
-- Purpose: Procedures for managing problems, solutions, and approvals
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- Procedure: usp_PROBLEM_CreateProblem
-- Description: Create a new problem record
-- ============================================================================
IF OBJECT_ID('dbo.usp_PROBLEM_CreateProblem', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PROBLEM_CreateProblem;
GO

CREATE PROCEDURE dbo.usp_PROBLEM_CreateProblem
    @p_Owner BIGINT,
    @p_Description VARCHAR(255),
    @p_Category CHAR(1),
    @p_Impact VARCHAR(255),
    @p_ExpectedResult VARCHAR(255),
    @p_UnitId BIGINT,
    @p_SiteId BIGINT,
    @p_EnteredBy BIGINT,
    @p_ProblemId BIGINT OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.PROBLEM_MAIN (PR_ID, PR_OWNER, PR_ENTEREDBY, PR_DESCRIPTION, 
            PR_CATEGORY, PR_IMPACT, PR_EXPRESULT, PR_ENTEREDON, PR_STATUS, PR_UNITID, 
            PR_SITEID, PR_MODBY, PR_MODON)
        VALUES (NEXT VALUE FOR dbo.seq_PROBLEM_MAIN_Id, @p_Owner, @p_EnteredBy, @p_Description,
            @p_Category, @p_Impact, @p_ExpectedResult, GETDATE(), 'P', @p_UnitId, @p_SiteId,
            @p_EnteredBy, GETDATE());
        
        SET @p_ProblemId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Problem created successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_PROBLEM_RecordSolution
-- Description: Record a solution for a problem
-- ============================================================================
IF OBJECT_ID('dbo.usp_PROBLEM_RecordSolution', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PROBLEM_RecordSolution;
GO

CREATE PROCEDURE dbo.usp_PROBLEM_RecordSolution
    @p_ProblemId BIGINT,
    @p_Description VARCHAR(255),
    @p_EnteredBy BIGINT,
    @p_SolutionId BIGINT OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verify problem exists
        IF NOT EXISTS (SELECT 1 FROM dbo.PROBLEM_MAIN WHERE PR_ID = @p_ProblemId)
        BEGIN
            SET @p_ErrorMessage = 'Problem not found.';
            RETURN;
        END
        
        INSERT INTO dbo.PROBLEM_SOLUTION (SOL_ID, SOL_PRID, SOL_DESCRIPTION, SOL_ENTEREDBY, SOL_ENTEREDON)
        VALUES (NEXT VALUE FOR dbo.seq_PROBLEM_SOLUTION_Id, @p_ProblemId, @p_Description, @p_EnteredBy, GETDATE());
        
        SET @p_SolutionId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Solution recorded successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_PROBLEM_ApproveProblem
-- Description: Approve a problem for posting
-- ============================================================================
IF OBJECT_ID('dbo.usp_PROBLEM_ApproveProblem', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PROBLEM_ApproveProblem;
GO

CREATE PROCEDURE dbo.usp_PROBLEM_ApproveProblem
    @p_ProblemId BIGINT,
    @p_ApprovedBy BIGINT,
    @p_Status CHAR(1),
    @p_Reason VARCHAR(255),
    @p_AudienceFlag CHAR(1),
    @p_ApprovalId BIGINT OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verify problem exists
        IF NOT EXISTS (SELECT 1 FROM dbo.PROBLEM_MAIN WHERE PR_ID = @p_ProblemId)
        BEGIN
            SET @p_ErrorMessage = 'Problem not found.';
            RETURN;
        END
        
        INSERT INTO dbo.PROBLEM_APP (PRAPP_ID, PRAPP_PRID, PRAPP_BY, PRAPP_ON, PRAPP_STATUS, PRAPP_REASON, PRAPP_AUDFLAG)
        VALUES (NEXT VALUE FOR dbo.seq_PROBLEM_APP_Id, @p_ProblemId, @p_ApprovedBy, GETDATE(), @p_Status, @p_Reason, @p_AudienceFlag);
        
        SET @p_ApprovalId = SCOPE_IDENTITY();
        
        -- Update problem status
        UPDATE dbo.PROBLEM_MAIN
        SET PR_STATUS = CASE WHEN @p_Status = 'A' THEN 'A' ELSE 'R' END,
            PR_APPID = @p_ApprovalId,
            PR_MODON = GETDATE(),
            PR_MODBY = @p_ApprovedBy
        WHERE PR_ID = @p_ProblemId;
        
        SET @p_ErrorMessage = 'Problem approved successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_PROBLEM_GetProblemsByStatus
-- Description: Get problems filtered by status
-- ============================================================================
IF OBJECT_ID('dbo.usp_PROBLEM_GetProblemsByStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PROBLEM_GetProblemsByStatus;
GO

CREATE PROCEDURE dbo.usp_PROBLEM_GetProblemsByStatus
    @p_Status CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        PR_ID,
        PR_OWNER,
        PR_DESCRIPTION,
        PR_CATEGORY,
        PR_IMPACT,
        PR_EXPRESULT,
        PR_ENTEREDON,
        PR_STATUS,
        PR_UNITID,
        PR_SITEID
    FROM dbo.PROBLEM_MAIN
    WHERE PR_STATUS = @p_Status
    ORDER BY PR_ENTEREDON DESC;
END;
GO

-- ============================================================================
-- Procedure: usp_PROBLEM_GetSolutionsByProblem
-- Description: Get all solutions for a problem
-- ============================================================================
IF OBJECT_ID('dbo.usp_PROBLEM_GetSolutionsByProblem', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PROBLEM_GetSolutionsByProblem;
GO

CREATE PROCEDURE dbo.usp_PROBLEM_GetSolutionsByProblem
    @p_ProblemId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        SOL_ID,
        SOL_PRID,
        SOL_DESCRIPTION,
        SOL_IMPLEMENTATION,
        SOL_ENTEREDBY,
        SOL_ENTEREDON
    FROM dbo.PROBLEM_SOLUTION
    WHERE SOL_PRID = @p_ProblemId
    ORDER BY SOL_ENTEREDON DESC;
END;
GO

PRINT 'Problem Management Procedures created successfully.';
GO
