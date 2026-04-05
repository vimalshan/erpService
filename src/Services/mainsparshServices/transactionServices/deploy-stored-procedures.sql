-- ==========================================
-- TransactionService - Deploy Stored Procedures
-- Fixes parameter names to match Dapper repository calls
-- and uses correct EF-created table names
-- ==========================================

USE SRFSPARSHDB;
GO

-- ==========================================
-- Create missing tables if they don't exist
-- (needed by usp_GetAvailableRooms)
-- ==========================================

IF OBJECT_ID('dbo.LOCATION_CONTACT', 'U') IS NULL
CREATE TABLE [LOCATION_CONTACT] (
    [LOCATION_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [LOCATION_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [LOCATION_NAME] VARCHAR(255) NOT NULL,
    [LOCATION_ADDRESS] NVARCHAR(MAX),
    [CONTACT_PERSON] VARCHAR(255),
    [CONTACT_PHONE] VARCHAR(20),
    [CONTACT_EMAIL] VARCHAR(255),
    [IS_ACTIVE] BIT NOT NULL DEFAULT 1,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [CREATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3),
    [UPDATED_BY] BIGINT
);
GO

IF OBJECT_ID('dbo.ROOM_MAST', 'U') IS NULL
CREATE TABLE [ROOM_MAST] (
    [ROOM_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [ROOM_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [ROOM_NAME] VARCHAR(255) NOT NULL,
    [LOCATION_ID] BIGINT NOT NULL,
    [ROOM_CAPACITY] INT,
    [ROOM_DESC] NVARCHAR(MAX),
    [IS_ACTIVE] BIT NOT NULL DEFAULT 1,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [CREATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3),
    [UPDATED_BY] BIGINT,
    FOREIGN KEY (LOCATION_ID) REFERENCES LOCATION_CONTACT(LOCATION_ID)
);
GO

-- ==========================================
-- 1. fn_CalculateSRFStipend
--    Params: @ResearchCategoryId BIGINT, @RankId BIGINT
--    Dapper: SELECT dbo.fn_CalculateSRFStipend(@ResearchCategoryId, @RankId)
-- ==========================================

IF OBJECT_ID('dbo.fn_CalculateSRFStipend', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateSRFStipend;
GO
CREATE FUNCTION dbo.fn_CalculateSRFStipend (
    @ResearchCategoryId BIGINT,
    @RankId BIGINT
)
RETURNS DECIMAL(19,2)
AS BEGIN
    DECLARE @Stipend DECIMAL(19,2) = 0;
    SELECT @Stipend = ISNULL(SRF_MONTHLY_STIPEND, 0)
    FROM dbo.SRF_STIPEND_MASTER
    WHERE RESEARCH_CATEGORY_ID = @ResearchCategoryId
      AND SRF_RANK_ID = @RankId
      AND STATUS = 'A'
      AND EFFECTIVE_FROM <= CAST(GETDATE() AS DATE)
      AND (EFFECTIVE_TO IS NULL OR EFFECTIVE_TO >= CAST(GETDATE() AS DATE));
    RETURN @Stipend;
END;
GO

-- ==========================================
-- 2. usp_ProcessSRFMonthlyStipend
--    Dapper: EXEC dbo.usp_ProcessSRFMonthlyStipend @Month, @Year, @ProcessedBy, @RowsProcessed OUTPUT
-- ==========================================

IF OBJECT_ID('dbo.usp_ProcessSRFMonthlyStipend', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ProcessSRFMonthlyStipend;
GO
CREATE PROCEDURE dbo.usp_ProcessSRFMonthlyStipend
    @Month INT,
    @Year INT,
    @ProcessedBy BIGINT,
    @RowsProcessed INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.SRF_STIPEND_DISBURSEMENT
        SET DISBURSEMENT_STATUS = 'P',
            UPDATED_ON = GETDATE(),
            UPDATED_BY = @ProcessedBy
        WHERE MONTH(DISBURSEMENT_DATE) = @Month
          AND YEAR(DISBURSEMENT_DATE) = @Year
          AND DISBURSEMENT_STATUS = 'D';

        SET @RowsProcessed = @@ROWCOUNT;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ==========================================
-- 3. usp_GetPendingDisbursements
--    Dapper: EXEC dbo.usp_GetPendingDisbursements (no params)
-- ==========================================

IF OBJECT_ID('dbo.usp_GetPendingDisbursements', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetPendingDisbursements;
GO
CREATE PROCEDURE dbo.usp_GetPendingDisbursements
AS BEGIN
    SET NOCOUNT ON;
    SELECT
        DISBURSEMENT_ID, SRF_ID, STIPEND_ID, DISBURSEMENT_DATE,
        DISBURSEMENT_AMOUNT, DISBURSEMENT_STATUS
    FROM dbo.SRF_STIPEND_DISBURSEMENT
    WHERE DISBURSEMENT_STATUS IN ('D', 'P')
    ORDER BY DISBURSEMENT_DATE ASC;
END;
GO

-- ==========================================
-- 4. usp_GetAvailableRooms
--    Dapper: EXEC dbo.usp_GetAvailableRooms @Date, @StartTime, @EndTime
-- ==========================================

IF OBJECT_ID('dbo.usp_GetAvailableRooms', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAvailableRooms;
GO
CREATE PROCEDURE dbo.usp_GetAvailableRooms
    @Date DATE,
    @StartTime TIME,
    @EndTime TIME
AS BEGIN
    SET NOCOUNT ON;
    -- Returns all active rooms with location info.
    -- BOOK_MAIN uses LOCATION_CODE (not ROOM_ID) so room-level
    -- time-slot conflict checking is deferred to the booking service.
    SELECT
        rm.ROOM_ID,
        rm.ROOM_CODE,
        rm.ROOM_NAME,
        rm.ROOM_CAPACITY,
        lc.LOCATION_NAME
    FROM dbo.ROOM_MAST rm
    INNER JOIN dbo.LOCATION_CONTACT lc ON rm.LOCATION_ID = lc.LOCATION_ID
    WHERE rm.IS_ACTIVE = 1
    ORDER BY rm.ROOM_CAPACITY, lc.LOCATION_NAME;
END;
GO

-- ==========================================
-- 5. usp_GetPendingApprovals
--    Dapper: EXEC dbo.usp_GetPendingApprovals @ApproverId
--    Uses EF-created APPROVAL_WORKFLOW table
-- ==========================================

IF OBJECT_ID('dbo.usp_GetPendingApprovals', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetPendingApprovals;
GO
CREATE PROCEDURE dbo.usp_GetPendingApprovals
    @ApproverId BIGINT = NULL
AS BEGIN
    SET NOCOUNT ON;
    SELECT
        aw.WORKFLOW_ID,
        aw.WORKFLOW_CODE,
        aw.ENTITY_TYPE,
        aw.ENTITY_ID,
        aw.EMPLOYEE_ID,
        aw.WORKFLOW_STATUS,
        aw.CURRENT_APPROVAL_LEVEL,
        aw.CURRENT_APPROVER_ID,
        aw.CREATED_ON
    FROM dbo.APPROVAL_WORKFLOW aw
    WHERE aw.WORKFLOW_STATUS IN ('SUBMITTED', 'IN_REVIEW')
      AND (@ApproverId IS NULL OR aw.CURRENT_APPROVER_ID = @ApproverId)
    ORDER BY aw.CREATED_ON ASC;
END;
GO

-- ==========================================
-- 6. usp_GetAuditLog
--    Dapper: EXEC dbo.usp_GetAuditLog @EntityType, @EntityId, @FromDate, @ToDate
--    Queries TRANSACTION_LOG table instead of dynamic SQL
-- ==========================================

IF OBJECT_ID('dbo.usp_GetAuditLog', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAuditLog;
GO
CREATE PROCEDURE dbo.usp_GetAuditLog
    @EntityType NVARCHAR(100) = NULL,
    @EntityId BIGINT = NULL,
    @FromDate DATETIME2(3) = NULL,
    @ToDate DATETIME2(3) = NULL
AS BEGIN
    SET NOCOUNT ON;
    SELECT
        LOG_ID,
        TRANSACTION_TYPE,
        TRANSACTION_ID,
        ACTION,
        ACTION_BY,
        ACTION_DATA,
        PREVIOUS_STATUS,
        NEW_STATUS,
        IP_ADDRESS,
        CREATED_ON
    FROM dbo.TRANSACTION_LOG
    WHERE (@EntityType IS NULL OR TRANSACTION_TYPE = @EntityType)
      AND (@EntityId IS NULL OR TRANSACTION_ID = @EntityId)
      AND (@FromDate IS NULL OR CREATED_ON >= @FromDate)
      AND (@ToDate IS NULL OR CREATED_ON <= @ToDate)
    ORDER BY CREATED_ON DESC;
END;
GO

-- ==========================================
-- 7. usp_ValidateBookingAttendees
--    Dapper: EXEC dbo.usp_ValidateBookingAttendees @BookingId
--    Returns SELECT (int) instead of OUTPUT param for ExecuteScalar
-- ==========================================

IF OBJECT_ID('dbo.usp_ValidateBookingAttendees', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ValidateBookingAttendees;
GO
CREATE PROCEDURE dbo.usp_ValidateBookingAttendees
    @BookingId BIGINT
AS BEGIN
    SET NOCOUNT ON;
    DECLARE @AttendeeCount INT = 0;

    SELECT @AttendeeCount = COUNT(*)
    FROM dbo.BOOK_ATTENDEES
    WHERE BOOKING_ID = @BookingId;

    SELECT CASE WHEN @AttendeeCount > 0 THEN 1 ELSE 0 END AS IsValid;
END;
GO

-- ==========================================
-- Performance Indices
-- ==========================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_BOOKING_DATE_STATUS' AND object_id = OBJECT_ID('BOOK_MAIN'))
    CREATE NONCLUSTERED INDEX IDX_BOOKING_DATE_STATUS ON BOOK_MAIN(BOOKING_DATE, BOOKING_STATUS);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_DISBURSE_STATUS_DATE' AND object_id = OBJECT_ID('SRF_STIPEND_DISBURSEMENT'))
    CREATE NONCLUSTERED INDEX IDX_DISBURSE_STATUS_DATE ON SRF_STIPEND_DISBURSEMENT(DISBURSEMENT_STATUS, DISBURSEMENT_DATE);

GO

PRINT 'All stored procedures deployed successfully.';
GO
