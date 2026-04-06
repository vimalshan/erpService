-- ==========================================
-- Database: PAYDB
-- Stored Procedures, Functions, Triggers
-- Payroll Management System
-- ==========================================

USE [PAYDB];
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

-- ------------------------------------------------------------------
-- Function: fn_GetTaxableIncome
-- Purpose:  Calculate taxable income after standard deductions
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetTaxableIncome
(
    @p_EmpSysID BIGINT,
    @p_FinancialYear INT
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @GrossIncome DECIMAL(19,0);
    DECLARE @StandardDeduction DECIMAL(19,0) = 50000;  -- Standard deduction
    DECLARE @TaxableIncome DECIMAL(19,0);
    
    -- Get total income for financial year
    SELECT @GrossIncome = ISNULL(SUM(EIC_GROSS_CTC), 0) * 12
    FROM EMPLOYEE_INCCTC
    WHERE EIC_EMPSYSID = @p_EmpSysID;
    
    SET @TaxableIncome = @GrossIncome - @StandardDeduction;
    
    RETURN CASE WHEN @TaxableIncome < 0 THEN 0 ELSE @TaxableIncome END;
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
        INSERT VALUES (source.AR_ID, source.PAY_EMPSYSID, source.AR_AMOUNT, source.AR_TYPE, source.AR_DATE);
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
