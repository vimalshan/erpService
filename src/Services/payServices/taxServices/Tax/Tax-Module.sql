-- ==========================================
-- Module: Tax
-- Database: PAYDB
-- ==========================================

USE [PAYDB];
GO

-- Table: TAX_MARDET - Marginal Tax Computation ID
CREATE TABLE [TAX_MARDET] (
    [FK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- Table: CONDED_MAST - Payee ID
CREATE TABLE [CONDED_MAST] (
    [FK] VARCHAR(255) NULL  -- NOT NULL
);
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

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
    -- Note: References EMPLOYEE_INCCTC from Employee module
    SELECT @GrossIncome = ISNULL(SUM(EIC_GROSS_CTC), 0) * 12
    FROM EMPLOYEE_INCCTC
    WHERE EIC_EMPSYSID = @p_EmpSysID;
    
    SET @TaxableIncome = @GrossIncome - @StandardDeduction;
    
    RETURN CASE WHEN @TaxableIncome < 0 THEN 0 ELSE @TaxableIncome END;
END;
GO
