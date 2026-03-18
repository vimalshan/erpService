-- ==========================================
-- Module: UNIT MANAGEMENT
-- Database: TASKDB
-- Purpose: Unit & Equipment Management Procedures & Functions
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_UNIT_RegisterEquipment
-- Purpose:  Register new equipment/asset in the system
-- Parameters:
--   @p_EquipmentID - Unique equipment ID
--   @p_EquipmentName - Name of equipment
--   @p_UnitCode - Unit code
--   @p_Category - Equipment category
--   @p_ModifiedBy - User registering equipment
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UNIT_RegisterEquipment
(
    @p_EquipmentID INT,
    @p_EquipmentName VARCHAR(65),
    @p_UnitCode CHAR(3),
    @p_Category VARCHAR(25),
    @p_ModifiedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS(SELECT 1 FROM UM_EQUIPMENT_MASTER WHERE EM_EQUIPMENT_ID = @p_EquipmentID)
        BEGIN
            INSERT INTO UM_EQUIPMENT_MASTER
            (
                EM_EQUIPMENT_ID, EM_EQUIPMENT_NAME, EM_UNIT_CODE, EM_CATEGORY,
                EM_START_DATE, EM_LAST_MODIFIEDBY, EM_LAST_MODIFIEDON
            )
            VALUES
            (
                @p_EquipmentID, @p_EquipmentName, @p_UnitCode, @p_Category,
                GETDATE(), @p_ModifiedBy, GETDATE()
            );
        END
        ELSE
        BEGIN
            UPDATE UM_EQUIPMENT_MASTER
            SET EM_EQUIPMENT_NAME = @p_EquipmentName,
                EM_CATEGORY = @p_Category,
                EM_LAST_MODIFIEDBY = @p_ModifiedBy,
                EM_LAST_MODIFIEDON = GETDATE()
            WHERE EM_EQUIPMENT_ID = @p_EquipmentID;
        END
        
        COMMIT TRANSACTION;
        PRINT 'Equipment registered: ' + @p_EquipmentName + ' (ID: ' + CAST(@p_EquipmentID AS VARCHAR) + ')';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Equipment registration failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_UNIT_UpdateEquipmentStatus
-- Purpose:  Record equipment status change
-- Parameters:
--   @p_StatusID - Equipment status ID
--   @p_EquipmentID - Equipment ID
--   @p_StatusDesc - Status description
--   @p_StatusCode - Status code
--   @p_Remarks - Status remarks
--   @p_Hours - Operating hours (if applicable)
--   @p_CreatedBy - User recording status
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UNIT_UpdateEquipmentStatus
(
    @p_StatusID INT,
    @p_EquipmentID INT,
    @p_StatusDesc VARCHAR(65),
    @p_StatusCode VARCHAR(5),
    @p_Remarks VARCHAR(200) = NULL,
    @p_Hours BIGINT = NULL,
    @p_CreatedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO UM_EQUIP_STATUS
        (
            ES_ID, ES_EQUIPMENT_ID, ES_STATUS_DESC, ES_STATUS_ID,
            ES_START_DATE, ES_REMARKS, ES_HOURS, ES_CREATED_BY, ES_CREATED_ON,
            ES_LAST_MODIFIED_BY, ES_LAST_MODIFIED_ON
        )
        VALUES
        (
            @p_StatusID, @p_EquipmentID, @p_StatusDesc, @p_StatusCode,
            CONVERT(VARCHAR(255), GETDATE(), 121), @p_Remarks, @p_Hours, @p_CreatedBy,
            CONVERT(VARCHAR(255), GETDATE(), 121), @p_CreatedBy, GETDATE()
        );
        
        COMMIT TRANSACTION;
        PRINT 'Equipment status updated: Equipment ' + CAST(@p_EquipmentID AS VARCHAR) + ' - ' + @p_StatusDesc;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Status update failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_UNIT_GrantUnitAccess
-- Purpose:  Grant employee access to a specific unit
-- Parameters:
--   @p_UnitCode - Unit code
--   @p_EmployeeSysID - Employee system ID
--   @p_AccessType - Access type (R=Read, W=Write, A=Admin)
--   @p_Module - Module code
--   @p_ModifiedBy - User granting access
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UNIT_GrantUnitAccess
(
    @p_UnitCode CHAR(3),
    @p_EmployeeSysID INT,
    @p_AccessType CHAR(1),
    @p_Module VARCHAR(5),
    @p_ModifiedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @AccessID INT;
        SELECT @AccessID = ISNULL(MAX(UA_ID), 0) + 1 FROM UM_ACCESS_MASTER;
        
        INSERT INTO UM_ACCESS_MASTER
        (
            UA_ID, UA_UNIT_CODE, UA_EMP_SYSID, UA_ACCESS_TYPE,
            UA_START_DATE, UA_LAST_MODIFIEDBY, UA_LAST_MODIFIEDON, UA_MODULE
        )
        VALUES
        (
            @AccessID, @p_UnitCode, @p_EmployeeSysID, @p_AccessType,
            GETDATE(), @p_ModifiedBy, GETDATE(), @p_Module
        );
        
        COMMIT TRANSACTION;
        PRINT 'Access granted: Employee ' + CAST(@p_EmployeeSysID AS VARCHAR) + ' to Unit ' + @p_UnitCode;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Access grant failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT - UNIT MODULE PROCEDURES
-- ==========================================
