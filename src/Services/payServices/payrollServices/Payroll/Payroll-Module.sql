-- ==========================================
-- Module: Payroll
-- Database: PAYDB
-- ==========================================

USE [PAYDB];
GO

-- Table: PAY_TRANDET - Transaction ID (Disbursement ID)
-- Corrected with columns required by usp_ProcessMonthlySalary
CREATE TABLE [PAY_TRANDET] (
    [TRN_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [TRN_EMPSYSID] BIGINT,
    [TRN_MONTH] VARCHAR(7),
    [TRN_GROSS] DECIMAL(19,0),
    [TRN_DEDUCTIONS] DECIMAL(19,0),
    [TRN_NET] DECIMAL(19,0),
    [TRN_STATUS] CHAR(1),
    [TRN_CREATEDBY] BIGINT,
    [TRN_CREATEDON] DATETIME,
    [FK] VARCHAR(255) NULL -- Original placeholder
);
GO

-- Table: PAYROLL_BATCH - Missing table required by usp_ProcessMonthlySalary
CREATE TABLE [PAYROLL_BATCH] (
    [BATCH_ID] BIGINT PRIMARY KEY,
    [BATCH_MONTH] VARCHAR(7),
    [BATCH_STATUS] CHAR(1),
    [BATCH_CREATEDBY] BIGINT,
    [BATCH_CREATEDON] DATETIME
);
GO

-- Table: PAYROLL_BATCHREVOKE - Revoke ID
CREATE TABLE [PAYROLL_BATCHREVOKE] (
    [PK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- Table: PAY_ARR - Pay ID
-- Corrected with columns required by fn_CalculateNetSalary and trigger
CREATE TABLE [PAY_ARR] (
    [AR_ID] BIGINT PRIMARY KEY,
    [PAY_EMPSYSID] BIGINT,
    [AR_AMOUNT] DECIMAL(19,0),
    [AR_TYPE] CHAR(1), -- Using 'AR_TYPE' as per trigger
    [PAY_TYPE] CHAR(1), -- Adding 'PAY_TYPE' as per fn_CalculateNetSalary if different
    [AR_DATE] DATETIME, -- Using 'AR_DATE' as per trigger
    [PAY_DATE] DATETIME, -- Adding 'PAY_DATE' as per fn_CalculateNetSalary if different
    [FK] VARCHAR(255) NULL -- Original placeholder
);
GO

-- Table: PAY_ADJWRK - Adjustment ID
CREATE TABLE [PAY_ADJWRK] (
    [FK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_CalculateNetSalary
-- Purpose:  Calculate net salary with all deductions and allowances
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_CalculateNetSalary
(
    @p_EmpSysID BIGINT,
    @p_MonthYear VARCHAR(7)  -- YYYY-MM
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @GrossSalary DECIMAL(19,0);
    DECLARE @BasicPay DECIMAL(19,0);
    DECLARE @Allowances DECIMAL(19,0);
    DECLARE @Deductions DECIMAL(19,0);
    DECLARE @NetSalary DECIMAL(19,0);
    
    -- Get basic pay
    SELECT TOP 1 @BasicPay = EIC_BASIC_SALARY
    FROM EMPLOYEE_INCCTC
    WHERE EIC_EMPSYSID = @p_EmpSysID;
    
    -- Calculate allowances
    -- Note: References PAY_TYPE/PAY_DATE which are added to PAY_ARR table above
    SELECT @Allowances = ISNULL(SUM(AR_AMOUNT), 0)
    FROM PAY_ARR
    WHERE PAY_EMPSYSID = @p_EmpSysID
      AND PAY_TYPE = 'A'  -- Allowance
      AND YEAR(PAY_DATE) = CAST(LEFT(@p_MonthYear, 4) AS INT)
      AND MONTH(PAY_DATE) = CAST(RIGHT(@p_MonthYear, 2) AS INT);
    
    SET @GrossSalary = ISNULL(@BasicPay, 0) + @Allowances;
    
    -- Calculate deductions (Loan EMI, Canteen, Tax)
    SELECT @Deductions = ISNULL(SUM(AR_AMOUNT), 0)
    FROM PAY_ARR
    WHERE PAY_EMPSYSID = @p_EmpSysID
      AND PAY_TYPE = 'D'  -- Deduction
      AND YEAR(PAY_DATE) = CAST(LEFT(@p_MonthYear, 4) AS INT)
      AND MONTH(PAY_DATE) = CAST(RIGHT(@p_MonthYear, 2) AS INT);
    
    SET @NetSalary = @GrossSalary - ISNULL(@Deductions, 0);
    
    RETURN ISNULL(@NetSalary, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessMonthlySalary
-- Purpose:  Process monthly salary for all employees
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessMonthlySalary
(
    @p_MonthYear VARCHAR(7),  -- YYYY-MM
    @p_ProcessedBy BIGINT,
    @p_BatchID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Create payroll batch
        SELECT @p_BatchID = ISNULL(MAX(BATCH_ID), 0) + 1 FROM PAYROLL_BATCH;
        
        INSERT INTO PAYROLL_BATCH
        (BATCH_ID, BATCH_MONTH, BATCH_STATUS, BATCH_CREATEDBY, BATCH_CREATEDON)
        VALUES (@p_BatchID, @p_MonthYear, 'P', @p_ProcessedBy, GETDATE());  -- P = Processing
        
        -- Process each employee
        DECLARE @EmpSysID BIGINT;
        DECLARE @NetSalary DECIMAL(19,0);
        DECLARE cur_emp CURSOR FOR
            SELECT DISTINCT EIC_EMPSYSID FROM EMPLOYEE_INCCTC;
        
        OPEN cur_emp;
        FETCH NEXT FROM cur_emp INTO @EmpSysID;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @NetSalary = dbo.fn_CalculateNetSalary(@EmpSysID, @p_MonthYear);
            
            -- Insert payroll transaction
            INSERT INTO PAY_TRANDET
            (
                TRN_EMPSYSID, TRN_MONTH, TRN_GROSS, TRN_DEDUCTIONS, TRN_NET,
                TRN_STATUS, TRN_CREATEDBY, TRN_CREATEDON
            )
            VALUES
            (
                @EmpSysID, @p_MonthYear, 0, 0, @NetSalary,
                'P', @p_ProcessedBy, GETDATE()
            );
            
            FETCH NEXT FROM cur_emp INTO @EmpSysID;
        END
        
        CLOSE cur_emp;
        DEALLOCATE cur_emp;
        
        -- Update batch status
        UPDATE PAYROLL_BATCH
        SET BATCH_STATUS = 'C'  -- Complete
        WHERE BATCH_ID = @p_BatchID;
        
        COMMIT TRANSACTION;
        PRINT 'Monthly salary processed for: ' + @p_MonthYear + ', Batch: ' + CAST(@p_BatchID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Salary processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_PayArr_ValidateAmount
-- Purpose:  Validate arrear/allowance/deduction amounts
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_PayArr_ValidateAmount
ON dbo.PAY_ARR
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Amount DECIMAL(19,0);
    DECLARE @Type CHAR(1);
    
    SELECT TOP 1 @Amount = AR_AMOUNT, @Type = AR_TYPE FROM inserted;
    
    -- Validate amount is not negative (except for some deduction types)
    IF @Amount < 0 AND @Type <> 'D'
    BEGIN
        RAISERROR('Invalid amount for arrear type', 16, 1);
        RETURN;
    END
    
    -- Proceed with insert/update
    MERGE INTO PAY_ARR AS target
    USING inserted AS source
    ON target.AR_ID = source.AR_ID
    WHEN MATCHED THEN
        UPDATE SET 
            target.AR_AMOUNT = source.AR_AMOUNT,
            target.AR_DATE = source.AR_DATE
    WHEN NOT MATCHED THEN
        INSERT VALUES (source.AR_ID, source.PAY_EMPSYSID, source.AR_AMOUNT, source.AR_TYPE, source.PAY_TYPE, source.AR_DATE, source.PAY_DATE);
END;
GO
