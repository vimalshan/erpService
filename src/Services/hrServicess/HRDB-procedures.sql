-- ==========================================
-- Database: HRDB
-- Stored Procedures, Functions, Triggers
-- Human Resources Management
-- ==========================================

USE [HRDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetEmployeeStatus
-- Purpose:  Get current employee status (Active/Inactive/Left)
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetEmployeeStatus
(
    @p_EmpSysID DECIMAL(38)
)
RETURNS CHAR(1)
AS
BEGIN
    DECLARE @Status CHAR(1);
    DECLARE @DOJ DATETIME2(3);
    DECLARE @DOL DATETIME2(3);
    
    SELECT TOP 1 
        @DOJ = EMP_DOJ,
        @DOL = EMP_DOL
    FROM EMPLOYEE_MASTER
    WHERE EMP_SYSID = @p_EmpSysID;
    
    -- Status: A=Active, I=Inactive, L=Left, P=Probation
    IF @DOJ IS NULL
        SET @Status = 'I';  -- Inactive if not found
    ELSE IF @DOL IS NOT NULL
        SET @Status = 'L';  -- Left
    ELSE IF DATEDIFF(DAY, @DOJ, GETDATE()) <= 180  -- 6 months probation
        SET @Status = 'P';  -- Probation
    ELSE
        SET @Status = 'A';  -- Active
    
    RETURN @Status;
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_GetServiceTenure
-- Purpose:  Calculate years of service for an employee
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetServiceTenure
(
    @p_EmpSysID DECIMAL(38),
    @p_AsOnDate DATETIME2(3) = NULL
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Tenure DECIMAL(5,2);
    DECLARE @DOJ DATETIME2(3);
    DECLARE @AsOn DATETIME2(3) = ISNULL(@p_AsOnDate, GETDATE());
    
    SELECT @DOJ = EMP_DOJ
    FROM EMPLOYEE_MASTER
    WHERE EMP_SYSID = @p_EmpSysID;
    
    IF @DOJ IS NOT NULL
        SET @Tenure = CAST(DATEDIFF(DAY, @DOJ, @AsOn) AS DECIMAL(5,2)) / 365.25;
    ELSE
        SET @Tenure = 0;
    
    RETURN ISNULL(@Tenure, 0);
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_GetLeaveEntitlement
-- Purpose:  Get annual leave entitlement based on grade and tenure
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetLeaveEntitlement
(
    @p_EmpSysID DECIMAL(38),
    @p_LeaveType CHAR(2)  -- CL=Casual, SL=Sick, PL=Privilege
)
RETURNS INT
AS
BEGIN
    DECLARE @Entitlement INT;
    DECLARE @Grade CHAR(3);
    DECLARE @Tenure DECIMAL(5,2);
    
    -- Get employee grade
    SELECT TOP 1 @Grade = EMP_GRADE
    FROM EMPLOYEE_MASTER
    WHERE EMP_SYSID = @p_EmpSysID;
    
    -- Get tenure
    SET @Tenure = dbo.fn_GetServiceTenure(@p_EmpSysID, GETDATE());
    
    -- Determine entitlement by grade and leave type
    SET @Entitlement = CASE
        WHEN @p_LeaveType = 'CL' THEN 12  -- Casual Leave
        WHEN @p_LeaveType = 'SL' THEN 10  -- Sick Leave
        WHEN @p_LeaveType = 'PL' THEN CASE
            WHEN @Tenure < 5 THEN 15
            WHEN @Tenure < 10 THEN 20
            ELSE 30
        END
        ELSE 0
    END;
    
    RETURN ISNULL(@Entitlement, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_CreateEmployee
-- Purpose:  Create new employee master record
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateEmployee
(
    @p_EmpName VARCHAR(100),
    @p_EmpDOJ DATETIME2(3),
    @p_EmpGrade CHAR(3),
    @p_EmpDept DECIMAL(38),
    @p_EmpLocation DECIMAL(38),
    @p_ManagerSysID DECIMAL(38) = NULL,
    @p_CreatedBy DECIMAL(38),
    @p_EmpSysID DECIMAL(38) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate inputs
        IF LEN(LTRIM(@p_EmpName)) = 0
            THROW 50001, 'Employee name cannot be empty', 1;
        
        IF @p_EmpDOJ > GETDATE()
            THROW 50002, 'DOJ cannot be in future', 1;
        
        -- Generate Employee Sys ID
        SELECT @p_EmpSysID = ISNULL(MAX(EMP_SYSID), 0) + 1 FROM EMPLOYEE_MASTER;
        
        -- Insert employee
        INSERT INTO EMPLOYEE_MASTER
        (
            EMP_SYSID, EMP_NAME, EMP_DOJ, EMP_GRADE, EMP_DEPT,
            EMP_LOCATION, EMP_MANAGER_SYSID, EMP_STATUS,
            EMP_CREATEDBY, EMP_CREATEDON, EMP_UPDATEDBY, EMP_UPDATEDON
        )
        VALUES
        (
            @p_EmpSysID, @p_EmpName, @p_EmpDOJ, @p_EmpGrade, @p_EmpDept,
            @p_EmpLocation, @p_ManagerSysID, 'A',  -- A = Active
            @p_CreatedBy, GETDATE(), @p_CreatedBy, GETDATE()
        );
        
        COMMIT TRANSACTION;
        PRINT 'Employee created: ID = ' + CAST(@p_EmpSysID AS VARCHAR) + ', Name = ' + @p_EmpName;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Employee creation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_UpdateEmployeeGrade
-- Purpose:  Promote or change employee grade
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UpdateEmployeeGrade
(
    @p_EmpSysID DECIMAL(38),
    @p_NewGrade CHAR(3),
    @p_EffectiveDate DATETIME2(3),
    @p_Remarks VARCHAR(500) = NULL,
    @p_UpdatedBy DECIMAL(38)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @OldGrade CHAR(3);
        
        -- Get current grade
        SELECT @OldGrade = EMP_GRADE
        FROM EMPLOYEE_MASTER
        WHERE EMP_SYSID = @p_EmpSysID;
        
        -- Update employee grade
        UPDATE EMPLOYEE_MASTER
        SET EMP_GRADE = @p_NewGrade,
            EMP_UPDATEDBY = @p_UpdatedBy,
            EMP_UPDATEDON = GETDATE()
        WHERE EMP_SYSID = @p_EmpSysID;
        
        -- Log grade change in audit
        INSERT INTO EMPLOYEE_GRADE_AUDIT
        (EMP_SYSID, OLD_GRADE, NEW_GRADE, EFFECTIVE_DATE, REMARKS, CHANGEDBY, CHANGEDON)
        VALUES
        (@p_EmpSysID, @OldGrade, @p_NewGrade, @p_EffectiveDate, @p_Remarks, @p_UpdatedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Grade updated: ' + @OldGrade + ' → ' + @p_NewGrade + ' for Employee ' + CAST(@p_EmpSysID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Grade update failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RecordProbationReview
-- Purpose:  Record probation review and confirmation status
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordProbationReview
(
    @p_ProbationID DECIMAL(38),
    @p_FinalStatus CHAR(1),  -- A=Confirmed, B=Extended, C=Terminated
    @p_ConfirmationDate DATETIME2(3) = NULL,
    @p_Remarks VARCHAR(500) = NULL,
    @p_ReviewedBy DECIMAL(38)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_FinalStatus NOT IN ('A', 'B', 'C')
            THROW 50001, 'Invalid status. Use A=Confirmed, B=Extended, C=Terminated', 1;
        
        -- Update probation record
        UPDATE AA_EMP_PROBATION
        SET PROB_FINSTATUS = @p_FinalStatus,
            PROB_CONFDATE = ISNULL(@p_ConfirmationDate, GETDATE())
        WHERE PROB_ID = @p_ProbationID;
        
        -- If confirmed (A), update employee status to Active
        IF @p_FinalStatus = 'A'
        BEGIN
            DECLARE @EmpSysID DECIMAL(38);
            SELECT @EmpSysID = PROB_EMP_SYSID FROM AA_EMP_PROBATION WHERE PROB_ID = @p_ProbationID;
            
            UPDATE EMPLOYEE_MASTER
            SET EMP_STATUS = 'A'  -- Confirmed as active employee
            WHERE EMP_SYSID = @EmpSysID;
        END
        
        COMMIT TRANSACTION;
        PRINT 'Probation review recorded: Status = ' + 
              CASE @p_FinalStatus WHEN 'A' THEN 'Confirmed' WHEN 'B' THEN 'Extended' ELSE 'Terminated' END;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Probation review failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_SetupAlertGroup
-- Purpose:  Create alert group and map employees
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_SetupAlertGroup
(
    @p_AlertGroupName VARCHAR(100),
    @p_AlertGroupType CHAR(1),  -- R=Reporting Unit, P=Payroll Unit, C=Calendar Wise
    @p_CreatedBy DECIMAL(38),
    @p_AlertGroupID DECIMAL(22,0) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_AlertGroupType NOT IN ('R', 'P', 'C')
            THROW 50001, 'Invalid alert group type', 1;
        
        -- Generate alert group ID
        SELECT @p_AlertGroupID = ISNULL(MAX(ALGRP_ID), 0) + 1 FROM ALERTGRP_MASTER;
        
        -- Create alert group
        INSERT INTO ALERTGRP_MASTER
        (ALGRP_ID, ALGRP_NAME, ALGRP_TYPE, ALGRP_CREATEDBY, ALGRP_CREATEDON)
        VALUES
        (@p_AlertGroupID, @p_AlertGroupName, @p_AlertGroupType, @p_CreatedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Alert group created: ID = ' + CAST(@p_AlertGroupID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Alert group setup failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_EmployeeMaster_ValidateGrade
-- Purpose:  Validate grade exists before employee assignment
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_EmployeeMaster_ValidateGrade
ON dbo.EMPLOYEE_MASTER
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Grade CHAR(3);
    SELECT TOP 1 @Grade = EMP_GRADE FROM inserted;
    
    -- Validate grade exists (assuming GRADE_MASTER table)
    IF NOT EXISTS (SELECT 1 FROM GRADE_MASTER WHERE GRADE_CODE = @Grade)
    BEGIN
        RAISERROR('Invalid grade code: %s', 16, 1, @Grade);
        RETURN;
    END
    
    -- Proceed with insert
    INSERT INTO EMPLOYEE_MASTER
    SELECT * FROM inserted;
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_EmployeeMaster_Audit
-- Purpose:  Audit all employee master changes
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_EmployeeMaster_Audit
ON dbo.EMPLOYEE_MASTER
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(EMP_GRADE) OR UPDATE(EMP_DEPT) OR UPDATE(EMP_STATUS)
    BEGIN
        INSERT INTO EMPLOYEE_AUDIT
        SELECT 
            I.EMP_SYSID,
            'UPDATE',
            GETDATE()
        FROM inserted I
        OUTER APPLY deleted D ON I.EMP_SYSID = D.EMP_SYSID;
    END
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_AAEmpProbation_AutoExtend
-- Purpose:  Auto-extend probation if not reviewed before due date
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_AAEmpProbation_AutoExtend
ON dbo.AA_EMP_PROBATION
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE AP
    SET AP.PROB_FINSTATUS = 'B',  -- Mark as Extended
        AP.PROB_NXTREVIEWDATE = DATEADD(MONTH, 3, AP.PROB_DUEDATE)
    FROM AA_EMP_PROBATION AP
    INNER JOIN inserted I ON AP.PROB_ID = I.PROB_ID
    WHERE AP.PROB_FINSTATUS IS NULL
      AND AP.PROB_DUEDATE < GETDATE();
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
