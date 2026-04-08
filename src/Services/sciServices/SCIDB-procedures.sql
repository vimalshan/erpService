-- SCIDB Stored Procedures, Functions & Triggers
-- Science Research Project Management System
-- Created: February 13, 2026

USE SCIDB;
GO

IF OBJECT_ID('dbo.fn_GetProjectBudgetUtilized', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetProjectBudgetUtilized;
GO
CREATE FUNCTION dbo.fn_GetProjectBudgetUtilized (@p_ProjectID BIGINT)
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @Utilized DECIMAL(19,0) = 0;
    BEGIN TRY
        SELECT @Utilized = ISNULL(SUM(EXPENSE_AMOUNT), 0) FROM dbo.PROJECT_EXPENSES 
        WHERE PROJECT_ID = @p_ProjectID AND EXPENSE_STATUS = 'A';
    END TRY BEGIN CATCH SET @Utilized = 0; END CATCH
    RETURN @Utilized;
END;
GO

IF OBJECT_ID('dbo.usp_RegisterProject', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterProject;
GO
CREATE PROCEDURE dbo.usp_RegisterProject
    @p_ProjectName VARCHAR(200), @p_PrincipalInvestigator BIGINT, @p_TotalBudget DECIMAL(19,0),
    @p_StartDate DATE, @p_EndDate DATE, @p_RegistrationBy BIGINT, @p_ProjectID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.PROJECT_MASTER (PROJECT_NAME, PI_EMP_ID, PROJECT_BUDGET, START_DATE, END_DATE, 
            PROJECT_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ProjectName, @p_PrincipalInvestigator, @p_TotalBudget, @p_StartDate, @p_EndDate, 'A', @p_RegistrationBy, GETDATE());
        SET @p_ProjectID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RecordProjectExpense', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordProjectExpense;
GO
CREATE PROCEDURE dbo.usp_RecordProjectExpense
    @p_ProjectID BIGINT, @p_ExpenseCategory VARCHAR(50), @p_Amount DECIMAL(19,0),
    @p_Remarks VARCHAR(500), @p_RecordedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @Utilized DECIMAL(19,0) = dbo.fn_GetProjectBudgetUtilized(@p_ProjectID), 
                @TotalBudget DECIMAL(19,0);
        SELECT @TotalBudget = PROJECT_BUDGET FROM dbo.PROJECT_MASTER WHERE PROJECT_ID = @p_ProjectID;
        IF (@Utilized + @p_Amount) > @TotalBudget
            THROW 50001, 'Expense exceeds project budget', 1;
        INSERT INTO dbo.PROJECT_EXPENSES (PROJECT_ID, EXPENSE_CATEGORY, EXPENSE_AMOUNT, EXPENSE_REMARKS, EXPENSE_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ProjectID, @p_ExpenseCategory, @p_Amount, @p_Remarks, 'A', @p_RecordedBy, GETDATE());
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'SCIDB Procedures created successfully.';
GO
