-- ==========================================
-- Module: Employee
-- Database: PAYDB
-- ==========================================

USE [PAYDB];
GO

-- Table: EMP_COSTCENTREDET - Cost Centre ID
CREATE TABLE [EMP_COSTCENTREDET] (
    [FK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- Table: EMPLOYEE_INCCTC - Increment No
-- Corrected with columns required by usp_ProcessIncrementCTC
CREATE TABLE [EMPLOYEE_INCCTC] (
    [EIC_EMPSYSID] BIGINT PRIMARY KEY,
    [EIC_GROSS_CTC] DECIMAL(19,0) NULL,
    [EIC_BASIC_SALARY] DECIMAL(19,0) NULL,
    [EIC_EFFECTIVE_DATE] DATETIME2(3) NULL,
    [FK] VARCHAR(255) NULL  -- Original placeholder column
);
GO

-- Table: EMPPF_EMPSPECIFIC - PF Deduction sequence
CREATE TABLE [EMPPF_EMPSPECIFIC] (
    [PK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- Table: SALARY_INCREMENT_LOG - Missing table required by usp_ProcessIncrementCTC
CREATE TABLE [SALARY_INCREMENT_LOG] (
    [LOG_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [EMP_SYSID] BIGINT,
    [OLD_CTC] DECIMAL(19,0),
    [NEW_CTC] DECIMAL(19,0),
    [INCREMENT_PERCENT] DECIMAL(5,2),
    [EFFECTIVE_DATE] DATETIME2(3),
    [APPROVED_BY] BIGINT,
    [APPROVED_ON] DATETIME
);
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessIncrementCTC
-- Purpose:  Apply salary increment to employee CTC
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessIncrementCTC
(
    @p_EmpSysID BIGINT,
    @p_IncrementPercent DECIMAL(5,2),
    @p_EffectiveDate DATETIME2(3),
    @p_ApprovedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @OldCTC DECIMAL(19,0);
        DECLARE @NewCTC DECIMAL(19,0);
        
        -- Get current CTC
        SELECT @OldCTC = EIC_GROSS_CTC
        FROM EMPLOYEE_INCCTC
        WHERE EIC_EMPSYSID = @p_EmpSysID;
        
        IF @OldCTC IS NULL
            THROW 50001, 'Employee CTC not found', 1;
        
        -- Calculate new CTC
        SET @NewCTC = @OldCTC * (1 + (@p_IncrementPercent / 100.0));
        
        -- Update CTC
        UPDATE EMPLOYEE_INCCTC
        SET EIC_GROSS_CTC = @NewCTC,
            EIC_BASIC_SALARY = (EIC_BASIC_SALARY * (1 + (@p_IncrementPercent / 100.0))),
            EIC_EFFECTIVE_DATE = @p_EffectiveDate
        WHERE EIC_EMPSYSID = @p_EmpSysID;
        
        -- Log increment
        INSERT INTO SALARY_INCREMENT_LOG
        (EMP_SYSID, OLD_CTC, NEW_CTC, INCREMENT_PERCENT, EFFECTIVE_DATE, APPROVED_BY, APPROVED_ON)
        VALUES
        (@p_EmpSysID, @OldCTC, @NewCTC, @p_IncrementPercent, @p_EffectiveDate, @p_ApprovedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Increment processed: ' + CAST(@p_IncrementPercent AS VARCHAR) + '% for Employee ' + CAST(@p_EmpSysID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Increment processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO
