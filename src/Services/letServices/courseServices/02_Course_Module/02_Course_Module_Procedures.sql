-- ==========================================
-- COURSE MODULE - Stored Procedures
-- Database: LETDB
-- Purpose: Course Management procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_Course_CreateSchedule
-- Purpose: Create a course schedule entry
IF OBJECT_ID('dbo.usp_Course_CreateSchedule', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_CreateSchedule;
GO
CREATE PROCEDURE dbo.usp_Course_CreateSchedule
    @p_CourseID BIGINT,
    @p_SchSerialNum BIGINT,
    @p_ScheduleDate DATETIME2(3),
    @p_StartTime CHAR(5),
    @p_EndTime CHAR(5),
    @p_LocationName VARCHAR(65),
    @p_TrainerName VARCHAR(65)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.COURSE_SCHEDULE (
            CS_CRS_ID,
            CS_SCH_SRL,
            CS_SCH_DAT,
            CS_STR_TIM,
            CS_END_TIM,
            CS_LOC_NAM,
            CS_TRN_NAM
        )
        VALUES (
            @p_CourseID,
            @p_SchSerialNum,
            @p_ScheduleDate,
            @p_StartTime,
            @p_EndTime,
            @p_LocationName,
            @p_TrainerName
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Course_RegisterParticipant
-- Purpose: Register a participant for a course
IF OBJECT_ID('dbo.usp_Course_RegisterParticipant', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_RegisterParticipant;
GO
CREATE PROCEDURE dbo.usp_Course_RegisterParticipant
    @p_CourseID BIGINT,
    @p_UserCode VARCHAR(255),
    @p_NominationStatus BIGINT,
    @p_EnrollmentDate DATETIME2(3),
    @p_ApprovalStatus CHAR(1)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.COURSE_PARTICIPANT_MGT (
            CS_CRS_ID,
            CS_USR_COD,
            CS_NOM_STS,
            CS_ENR_DAT,
            CS_APPR_APPROV
        )
        VALUES (
            @p_CourseID,
            @p_UserCode,
            @p_NominationStatus,
            @p_EnrollmentDate,
            @p_ApprovalStatus
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Course_CancelParticipant
-- Purpose: Cancel participant registration
IF OBJECT_ID('dbo.usp_Course_CancelParticipant', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_CancelParticipant;
GO
CREATE PROCEDURE dbo.usp_Course_CancelParticipant
    @p_CourseID BIGINT,
    @p_UserCode VARCHAR(255),
    @p_CancellationDate DATETIME2(3),
    @p_CancellationRemark VARCHAR(255)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.COURSE_PARTICIPANT_MGT
        SET CS_CAN_DAT = @p_CancellationDate,
            CS_CAN_REM = @p_CancellationRemark
        WHERE CS_CRS_ID = @p_CourseID
            AND CS_USR_COD = @p_UserCode;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Course_GetParticipants
-- Purpose: Retrieve all participants for a course
IF OBJECT_ID('dbo.usp_Course_GetParticipants', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_GetParticipants;
GO
CREATE PROCEDURE dbo.usp_Course_GetParticipants
    @p_CourseID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CS_CRS_ID,
        CS_USR_COD,
        CS_NOM_STS,
        CS_ENR_DAT,
        CS_APPR_APPROV,
        CS_CAN_DAT,
        CS_CAN_REM,
        CS_ATTEN
    FROM dbo.COURSE_PARTICIPANT_MGT
    WHERE CS_CRS_ID = @p_CourseID
    ORDER BY CS_ENR_DAT DESC;
END;
GO

-- Procedure: usp_Course_GetSchedules
-- Purpose: Retrieve course schedule details
IF OBJECT_ID('dbo.usp_Course_GetSchedules', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_GetSchedules;
GO
CREATE PROCEDURE dbo.usp_Course_GetSchedules
    @p_CourseID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CS_CRS_ID,
        CS_SCH_SRL,
        CS_SCH_DAT,
        CS_STR_TIM,
        CS_END_TIM,
        CS_LOC_NAM,
        CS_TRN_NAM
    FROM dbo.COURSE_SCHEDULE
    WHERE CS_CRS_ID = @p_CourseID
    ORDER BY CS_SCH_DAT;
END;
GO

-- Procedure: usp_Course_UpdateParticipantAttendance
-- Purpose: Update participant attendance status
IF OBJECT_ID('dbo.usp_Course_UpdateParticipantAttendance', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_UpdateParticipantAttendance;
GO
CREATE PROCEDURE dbo.usp_Course_UpdateParticipantAttendance
    @p_CourseID BIGINT,
    @p_UserCode VARCHAR(255),
    @p_AttendanceStatus CHAR(1)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.COURSE_PARTICIPANT_MGT
        SET CS_ATTEN = @p_AttendanceStatus
        WHERE CS_CRS_ID = @p_CourseID
            AND CS_USR_COD = @p_UserCode;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Course_GetCourseDetails
-- Purpose: Retrieve course master details
IF OBJECT_ID('dbo.usp_Course_GetCourseDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Course_GetCourseDetails;
GO
CREATE PROCEDURE dbo.usp_Course_GetCourseDetails
    @p_CourseID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CR_CRS_ID,
        CR_CRS_TYP,
        CR_CRS_DES,
        CR_EFF_DAT,
        CR_CLS_DAT,
        CR_STR_DAT,
        CR_END_DAT,
        CR_NO_DYS,
        CR_TRN_TYP,
        CR_TRN_NAM1,
        CR_TRN_NAM2,
        CR_TRN_NAM3
    FROM dbo.COURSE_MAST
    WHERE CR_CRS_ID = @p_CourseID;
END;
GO

PRINT 'Course Module Procedures created successfully.';
GO
