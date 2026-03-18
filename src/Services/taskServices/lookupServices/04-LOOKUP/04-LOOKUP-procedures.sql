-- ==========================================
-- Module: LOOKUP & CONFIGURATION
-- Database: TASKDB
-- Purpose: Master Data & Lookup Management Procedures & Functions
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_LOOKUP_InsertLOV
-- Purpose:  Insert new List of Values entry
-- Parameters:
--   @p_LOVType - LOV type code
--   @p_LOVName - LOV name/description
--   @p_LOVId - Output parameter for generated LOV ID
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_LOOKUP_InsertLOV
(
    @p_LOVType CHAR(3),
    @p_LOVName VARCHAR(200),
    @p_LOVId BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate LOV ID
        SELECT @p_LOVId = ISNULL(MAX(LOV_ID), 0) + 1 FROM LOV_MASTER;
        
        INSERT INTO LOV_MASTER
        (
            LOV_TYPE, LOV_ID, LOV_NAME
        )
        VALUES
        (
            @p_LOVType, @p_LOVId, @p_LOVName
        );
        
        COMMIT TRANSACTION;
        PRINT 'LOV inserted: Type ' + @p_LOVType + ', ID ' + CAST(@p_LOVId AS VARCHAR) + ' - ' + @p_LOVName;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('LOV insertion failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_LOOKUP_MapLOVToUnit
-- Purpose:  Create mapping between LOV and Unit
-- Parameters:
--   @p_LOVId - LOV ID
--   @p_UnitCode - Unit code
--   @p_Flag - Active flag (Y/N)
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_LOOKUP_MapLOVToUnit
(
    @p_LOVId DECIMAL(38),
    @p_UnitCode CHAR(3),
    @p_Flag CHAR(1) = 'Y'
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MapID DECIMAL(38);
        SELECT @MapID = ISNULL(MAX(LU_MAPID), 0) + 1 FROM LOV_UNITMAP;
        
        INSERT INTO LOV_UNITMAP
        (
            LU_MAPID, LU_LOVID, LU_UNITCODE, LU_FLAG
        )
        VALUES
        (
            @MapID, @p_LOVId, @p_UnitCode, @p_Flag
        );
        
        COMMIT TRANSACTION;
        PRINT 'LOV mapped to Unit: LOV ID ' + CAST(@p_LOVId AS VARCHAR) + ' -> Unit ' + @p_UnitCode;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('LOV mapping failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_LOOKUP_InsertProcess
-- Purpose:  Register new process in master
-- Parameters:
--   @p_ProcessID - Process ID
--   @p_ProcessName - Process name
--   @p_LiveFlag - Active flag (Y/N)
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_LOOKUP_InsertProcess
(
    @p_ProcessID DECIMAL(38),
    @p_ProcessName VARCHAR(50),
    @p_LiveFlag CHAR(1) = 'Y'
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS(SELECT 1 FROM PROCESS_MASTER WHERE PROCESS_ID = @p_ProcessID)
        BEGIN
            INSERT INTO PROCESS_MASTER
            (
                PROCESS_ID, PROCESS_NAME, PROCESS_LIVFLAG
            )
            VALUES
            (
                @p_ProcessID, @p_ProcessName, @p_LiveFlag
            );
        END
        ELSE
        BEGIN
            UPDATE PROCESS_MASTER
            SET PROCESS_NAME = @p_ProcessName,
                PROCESS_LIVFLAG = @p_LiveFlag
            WHERE PROCESS_ID = @p_ProcessID;
        END
        
        COMMIT TRANSACTION;
        PRINT 'Process' + CASE WHEN NOT EXISTS(SELECT 1 FROM PROCESS_MASTER WHERE PROCESS_ID = @p_ProcessID) THEN ' created' ELSE ' updated' END + 
              ': ' + @p_ProcessName + ' (ID: ' + CAST(@p_ProcessID AS VARCHAR) + ')';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Process operation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_LOOKUP_MapUnitProcess
-- Purpose:  Create mapping between Unit and Process
-- Parameters:
--   @p_UnitCode - Unit code
--   @p_ProcessID - Process ID
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_LOOKUP_MapUnitProcess
(
    @p_UnitCode CHAR(3),
    @p_ProcessID DECIMAL(38)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MapID DECIMAL(38);
        SELECT @MapID = ISNULL(MAX(UP_MAPID), 0) + 1 FROM UNIT_PROCESS_MAP;
        
        INSERT INTO UNIT_PROCESS_MAP
        (
            UP_MAPID, UP_UNIT_CODE, UP_PROCESS_ID
        )
        VALUES
        (
            @MapID, @p_UnitCode, @p_ProcessID
        );
        
        COMMIT TRANSACTION;
        PRINT 'Unit-Process mapping created: Unit ' + @p_UnitCode + ' -> Process ' + CAST(@p_ProcessID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Unit-Process mapping failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT - LOOKUP MODULE PROCEDURES
-- ==========================================
