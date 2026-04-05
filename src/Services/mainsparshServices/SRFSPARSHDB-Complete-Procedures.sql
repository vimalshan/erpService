-- ==========================================
-- Database: SRFSPARSHDB - Complete Stored Procedures & Functions
-- SRF (Senior Research Fellow) Scholarship Management
-- Created: March 2, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- ==========================================
-- STIPEND FUNCTIONS & PROCEDURES
-- ==========================================

IF OBJECT_ID('dbo.fn_CalculateSRFStipend', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateSRFStipend;
GO
CREATE FUNCTION dbo.fn_CalculateSRFStipend (@p_ResearchCategoryID BIGINT, @p_RankID BIGINT)
RETURNS DECIMAL(19,2)
AS BEGIN
    DECLARE @Stipend DECIMAL(19,2) = 0;
    BEGIN TRY
        SELECT @Stipend = ISNULL(SRF_MONTHLY_STIPEND, 0) 
        FROM dbo.SRF_STIPEND_MASTER 
        WHERE RESEARCH_CATEGORY_ID = @p_ResearchCategoryID 
          AND SRF_RANK_ID = @p_RankID
          AND IS_ACTIVE = 1
          AND EFFECTIVE_FROM <= CAST(GETDATE() AS DATE)
          AND (EFFECTIVE_TO IS NULL OR EFFECTIVE_TO >= CAST(GETDATE() AS DATE));
    END TRY 
    BEGIN CATCH 
        SET @Stipend = 0;
    END CATCH
    RETURN @Stipend;
END;
GO

IF OBJECT_ID('dbo.usp_ProcessSRFMonthlyStipend', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ProcessSRFMonthlyStipend;
GO
CREATE PROCEDURE dbo.usp_ProcessSRFMonthlyStipend 
    @p_MonthYear VARCHAR(7), 
    @p_ProcessedBy BIGINT, 
    @p_RowsProcessed INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @ProcessDate DATETIME2(3) = GETDATE(), 
                @RowCount INT = 0;
        
        UPDATE dbo.SRF_STIPEND_DISBURSEMENT 
        SET DISBURSEMENT_STATUS = 'P', UPDATED_ON = @ProcessDate, UPDATED_BY = @p_ProcessedBy
        WHERE MONTH(DISBURSEMENT_DATE) = MONTH(CAST(@p_MonthYear + '-01' AS DATE))
          AND YEAR(DISBURSEMENT_DATE) = YEAR(CAST(@p_MonthYear + '-01' AS DATE))
          AND DISBURSEMENT_STATUS = 'D';
        
        SET @p_RowsProcessed = @@ROWCOUNT;
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetPendingDisbursements', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetPendingDisbursements;
GO
CREATE PROCEDURE dbo.usp_GetPendingDisbursements
AS BEGIN
    SELECT 
        DISBURSE_ID, SRF_ID, STIPEND_ID, DISBURSEMENT_DATE, 
        DISBURSEMENT_AMOUNT, DISBURSEMENT_STATUS
    FROM dbo.SRF_STIPEND_DISBURSEMENT
    WHERE DISBURSEMENT_STATUS IN ('D', 'P')
    ORDER BY DISBURSEMENT_DATE ASC;
END;
GO

-- ==========================================
-- BOOKING PROCEDURES
-- ==========================================

IF OBJECT_ID('dbo.usp_GetAvailableRooms', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAvailableRooms;
GO
CREATE PROCEDURE dbo.usp_GetAvailableRooms
    @p_BookingDate DATE,
    @p_TimeFrom TIME,
    @p_TimeTo TIME,
    @p_MinCapacity INT
AS BEGIN
    SELECT 
        rm.ROOM_ID,
        rm.ROOM_CODE,
        rm.ROOM_NAME,
        rm.ROOM_CAPACITY,
        lc.LOCATION_NAME
    FROM dbo.ROOM_MAST rm
    INNER JOIN dbo.LOCATION_CONTACT lc ON rm.LOCATION_ID = lc.LOCATION_ID
    WHERE rm.IS_ACTIVE = 1
      AND rm.ROOM_CAPACITY >= @p_MinCapacity
      AND rm.ROOM_ID NOT IN (
          SELECT ROOM_ID FROM dbo.BOOK_MAIN
          WHERE BOOKING_DATE = @p_BookingDate
            AND BOOKING_STATUS != 'CANCELLED'
            AND (
              (@p_TimeFrom >= BOOKING_TIME_FROM AND @p_TimeFrom < BOOKING_TIME_TO) OR
              (@p_TimeTo > BOOKING_TIME_FROM AND @p_TimeTo <= BOOKING_TIME_TO) OR
              (@p_TimeFrom <= BOOKING_TIME_FROM AND @p_TimeTo >= BOOKING_TIME_TO)
            )
      )
    ORDER BY rm.ROOM_CAPACITY, lc.LOCATION_NAME;
END;
GO

-- ==========================================
-- APPROVAL WORKFLOW PROCEDURES
-- ==========================================

IF OBJECT_ID('dbo.usp_GetPendingApprovals', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetPendingApprovals;
GO
CREATE PROCEDURE dbo.usp_GetPendingApprovals
    @p_ApproverId BIGINT
AS BEGIN
    SELECT 
        aw.WORKFLOW_ID,
        aw.Appraisal,
        aw.EMPLOYEE_ID,
        aw.WORKFLOW_STATUS,
        aw.CURRENT_APPROVAL_LEVEL,
        aw.CREATED_ON
    FROM dbo.ApprovalWorkflow aw
    WHERE aw.CURRENT_APPROVER_ID = @p_ApproverId
      AND aw.WORKFLOW_STATUS = 'SUBMITTED'
    ORDER BY aw.CREATED_ON ASC;
END;
GO

IF OBJECT_ID('dbo.usp_GetUserPolicies', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetUserPolicies;
GO
CREATE PROCEDURE dbo.usp_GetUserPolicies
    @p_UserId BIGINT
AS BEGIN
    SELECT 
        POLICY_ID,
        POLICY_CODE,
        POLICY_DESC,
        POLICY_VALUE,
        IS_ACTIVE
    FROM dbo.USER_POLICY
    WHERE USER_ID = @p_UserId
      AND IS_ACTIVE = 1
    ORDER BY POLICY_CODE;
END;
GO

-- ==========================================
-- AUDIT & MONITORING
-- ==========================================

IF OBJECT_ID('dbo.usp_GetAuditLog', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAuditLog;
GO
CREATE PROCEDURE dbo.usp_GetAuditLog
    @p_TableName NVARCHAR(255),
    @p_StartDate DATETIME2(3) = NULL,
    @p_EndDate DATETIME2(3) = NULL
AS BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT CREATED_ON, CREATED_BY, UPDATED_ON, UPDATED_BY FROM ' + QUOTENAME(@p_TableName) 
             + ' WHERE 1=1';
    
    IF @p_StartDate IS NOT NULL
        SET @SQL = @SQL + ' AND CREATED_ON >= @StartDate';
    
    IF @p_EndDate IS NOT NULL
        SET @SQL = @SQL + ' AND CREATED_ON <= @EndDate';
    
    SET @SQL = @SQL + ' ORDER BY CREATED_ON DESC';
    
    EXEC sp_executesql @SQL, N'@StartDate DATETIME2(3), @EndDate DATETIME2(3)', 
                       @p_StartDate, @p_EndDate;
END;
GO

-- ==========================================
-- DATA INTEGRITY CHECKS
-- ==========================================

IF OBJECT_ID('dbo.usp_ValidateBookingAttendees', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ValidateBookingAttendees;
GO
CREATE PROCEDURE dbo.usp_ValidateBookingAttendees
    @p_BookingId BIGINT,
    @p_IsValid BIT OUTPUT
AS BEGIN
    DECLARE @AttendeeCount INT = 0;
    
    SELECT @AttendeeCount = COUNT(*) 
    FROM dbo.BOOK_ATTENDEES
    WHERE BOOK_ID = @p_BookingId;
    
    SET @p_IsValid = CASE WHEN @AttendeeCount > 0 THEN 1 ELSE 0 END;
END;
GO

-- ==========================================
-- INDICES FOR PERFORMANCE
-- ==========================================

-- User Management Indices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_APPROVER_STATUS' AND object_id = OBJECT_ID('APPROVER_EMP'))
    CREATE NONCLUSTERED INDEX IDX_APPROVER_STATUS ON APPROVER_EMP(IS_ACTIVE, APPROVER_LEVEL);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_GROUP_ACTIVE' AND object_id = OBJECT_ID('GROUP_MAST'))
    CREATE NONCLUSTERED INDEX IDX_GROUP_ACTIVE ON GROUP_MAST(IS_ACTIVE);

-- Booking Indices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_BOOKING_DATE_ROOM' AND object_id = OBJECT_ID('BOOK_MAIN'))
    CREATE NONCLUSTERED INDEX IDX_BOOKING_DATE_ROOM ON BOOK_MAIN(BOOKING_DATE, ROOM_ID, BOOKING_STATUS);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_ATTENDEE_STATUS' AND object_id = OBJECT_ID('BOOK_ATTENDEES'))
    CREATE NONCLUSTERED INDEX IDX_ATTENDEE_STATUS ON BOOK_ATTENDEES(ATTENDANCE_STATUS);

-- Finance Indices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_DISBURSE_STATUS_DATE' AND object_id = OBJECT_ID('SRF_STIPEND_DISBURSEMENT'))
    CREATE NONCLUSTERED INDEX IDX_DISBURSE_STATUS_DATE ON SRF_STIPEND_DISBURSEMENT(DISBURSEMENT_STATUS, DISBURSEMENT_DATE);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_REIM_WORKFLOW' AND object_id = OBJECT_ID('REIM_TRAN'))
    CREATE NONCLUSTERED INDEX IDX_REIM_WORKFLOW ON REIM_TRAN(REIM_STATUS, USER_ID);

GO

PRINT 'SRFSPARSHDB Procedures and Functions created successfully.';
GO
