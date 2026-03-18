-- ==========================================
-- DISPATCH PLANNING MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Dispatch Plan Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_CreateDispatchPlan', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateDispatchPlan;
GO
CREATE PROCEDURE dbo.usp_CreateDispatchPlan
    @p_PlanType CHAR(1),
    @p_PlanMonth DATETIME2,
    @p_CompanyUnitID INT,
    @p_ModifiedBy INT,
    @p_PlanHeaderID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.DISPATCH_PLAN_HEADER (DISPATCH_PLAN_TYPE, DISPATCH_PLAN_MONTH, DISPATCH_PLAN_ENTRYDATE, COMPANY_UNIT_ID, SCI_USER_ID_MODIFIED, MODIFIED_DATE)
            VALUES (@p_PlanType, @p_PlanMonth, GETDATE(), @p_CompanyUnitID, @p_ModifiedBy, GETDATE());
            SET @p_PlanHeaderID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_AddDispatchPlanItem', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AddDispatchPlanItem;
GO
CREATE PROCEDURE dbo.usp_AddDispatchPlanItem
    @p_PlanHeaderID INT,
    @p_ItemID INT,
    @p_TargetWeek1 BIGINT,
    @p_TargetWeek2 BIGINT,
    @p_TargetWeek3 BIGINT,
    @p_TargetWeek4 BIGINT,
    @p_TargetWeek5 BIGINT,
    @p_ModifiedBy INT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.DISPATCH_PLAN_ITEMWISE (DISPATCH_PLAN_HEADER_ID, BREAKUP_ITEM_ID, TARGET_WEEK1, TARGET_WEEK2, TARGET_WEEK3, TARGET_WEEK4, TARGET_WEEK5, SCI_USER_ID_MODIFIED, MODIFIED_DATE)
            VALUES (@p_PlanHeaderID, @p_ItemID, @p_TargetWeek1, @p_TargetWeek2, @p_TargetWeek3, @p_TargetWeek4, @p_TargetWeek5, @p_ModifiedBy, GETDATE());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT 'DISPATCH_PLANNING_MODULE Procedures created successfully.';
GO
