-- ============================================================================
-- Module: Mobile Expense Management - Stored Procedures
-- Purpose: Procedures for managing mobile expenses and file attachments
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- Procedure: usp_EXP_RecordExpense
-- Description: Record a new mobile expense
-- ============================================================================
IF OBJECT_ID('dbo.usp_EXP_RecordExpense', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_EXP_RecordExpense;
GO

CREATE PROCEDURE dbo.usp_EXP_RecordExpense
    @p_TripId DECIMAL(38),
    @p_CategoryId DECIMAL(38),
    @p_Comment VARCHAR(500),
    @p_Amount DECIMAL(19,2),
    @p_CurrencyId DECIMAL(38),
    @p_EnteredBy DECIMAL(38),
    @p_ExpenseId DECIMAL(38) OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.MOBEXP_DET (MOBEXP_ID, MOBEXP_TPID, MOBEXP_CATID, MOBEXP_DATE, 
            MOBEXP_COMMENT, MOBEXP_AMOUNT, MOBEXP_CURRID, MOBEXP_ENTEREDBY, MOBEXP_ENTEREDON)
        VALUES (NEXT VALUE FOR dbo.seq_MOBEXP_Id, @p_TripId, @p_CategoryId, GETDATE(), 
            @p_Comment, @p_Amount, @p_CurrencyId, @p_EnteredBy, GETDATE());
        
        SET @p_ExpenseId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Expense recorded successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_EXP_AttachExpenseFile
-- Description: Attach a file to an expense record
-- ============================================================================
IF OBJECT_ID('dbo.usp_EXP_AttachExpenseFile', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_EXP_AttachExpenseFile;
GO

CREATE PROCEDURE dbo.usp_EXP_AttachExpenseFile
    @p_ExpenseId DECIMAL(38),
    @p_FileName VARCHAR(500),
    @p_FileData NVARCHAR(MAX),
    @p_FileId DECIMAL(38) OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verify expense exists
        IF NOT EXISTS (SELECT 1 FROM dbo.MOBEXP_DET WHERE MOBEXP_ID = @p_ExpenseId)
        BEGIN
            SET @p_ErrorMessage = 'Expense not found.';
            RETURN;
        END
        
        INSERT INTO dbo.MOBEXP_FILE (MOBEXPPHT_ID, MOBEXPPHT_EXPID, MOBEXPPHT_FILENAME, MOBEXPPHT_FILEDATA)
        VALUES (NEXT VALUE FOR dbo.seq_MOBEXP_File_Id, @p_ExpenseId, @p_FileName, @p_FileData);
        
        SET @p_FileId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'File attached successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_EXP_GetExpensesByTrip
-- Description: Retrieve all expenses for a trip
-- ============================================================================
IF OBJECT_ID('dbo.usp_EXP_GetExpensesByTrip', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_EXP_GetExpensesByTrip;
GO

CREATE PROCEDURE dbo.usp_EXP_GetExpensesByTrip
    @p_TripId DECIMAL(38)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MOBEXP_ID,
        MOBEXP_TPID,
        MOBEXP_CATID,
        MOBEXP_DATE,
        MOBEXP_COMMENT,
        MOBEXP_AMOUNT,
        MOBEXP_CURRID,
        MOBEXP_ENTEREDBY,
        MOBEXP_ENTEREDON
    FROM dbo.MOBEXP_DET
    WHERE MOBEXP_TPID = @p_TripId
    ORDER BY MOBEXP_DATE DESC;
END;
GO

-- ============================================================================
-- Procedure: usp_EXP_GetExpenseFiles
-- Description: Retrieve files attached to an expense
-- ============================================================================
IF OBJECT_ID('dbo.usp_EXP_GetExpenseFiles', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_EXP_GetExpenseFiles;
GO

CREATE PROCEDURE dbo.usp_EXP_GetExpenseFiles
    @p_ExpenseId DECIMAL(38)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MOBEXPPHT_ID,
        MOBEXPPHT_EXPID,
        MOBEXPPHT_FILENAME
    FROM dbo.MOBEXP_FILE
    WHERE MOBEXPPHT_EXPID = @p_ExpenseId;
END;
GO

PRINT 'Mobile Expense Management Procedures created successfully.';
GO
