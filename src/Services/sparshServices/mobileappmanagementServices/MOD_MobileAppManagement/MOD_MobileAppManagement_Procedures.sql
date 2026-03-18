-- ============================================================================
-- Module: Mobile App Management - Stored Procedures
-- Purpose: Procedures for mobile app device and login management
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- Procedure: usp_MOB_RegisterDevice
-- Description: Register or update a mobile device for an employee
-- ============================================================================
IF OBJECT_ID('dbo.usp_MOB_RegisterDevice', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_MOB_RegisterDevice;
GO

CREATE PROCEDURE dbo.usp_MOB_RegisterDevice
    @p_EmpSysId DECIMAL(38),
    @p_DeviceId VARCHAR(200),
    @p_DeviceType CHAR(1),
    @p_ImeiNo VARCHAR(200),
    @p_UpdatedBy DECIMAL(38),
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if device already exists
        IF EXISTS (SELECT 1 FROM dbo.MOB_APPDEVICE_DETAILS 
                   WHERE MD_EMPSYSID = @p_EmpSysId AND MD_DEVICEID = @p_DeviceId)
        BEGIN
            -- Update existing device
            UPDATE dbo.MOB_APPDEVICE_DETAILS
            SET MD_ACTIVE = 'Y',
                MD_DEVICETYPE = @p_DeviceType,
                MD_IMEINO = @p_ImeiNo,
                MD_UPDATEDBY = @p_UpdatedBy,
                MD_UPDATEDON = GETDATE()
            WHERE MD_EMPSYSID = @p_EmpSysId AND MD_DEVICEID = @p_DeviceId;
        END
        ELSE
        BEGIN
            -- Insert new device
            INSERT INTO dbo.MOB_APPDEVICE_DETAILS (MD_EMPSYSID, MD_DEVICEID, MD_ACTIVE, 
                MD_DEVICETYPE, MD_IMEINO, MD_CREATEDON, MD_UPDATEDBY, MD_UPDATEDON)
            VALUES (@p_EmpSysId, @p_DeviceId, 'Y', @p_DeviceType, @p_ImeiNo, 
                GETDATE(), @p_UpdatedBy, GETDATE());
        END
        
        SET @p_ErrorMessage = 'Device registered successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_MOB_LogUserLogin
-- Description: Log user login event
-- ============================================================================
IF OBJECT_ID('dbo.usp_MOB_LogUserLogin', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_MOB_LogUserLogin;
GO

CREATE PROCEDURE dbo.usp_MOB_LogUserLogin
    @p_UserSysId DECIMAL(38),
    @p_DeviceId VARCHAR(200),
    @p_ImeiNo VARCHAR(200),
    @p_DeviceType CHAR(1),
    @p_LoginId DECIMAL(38) OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @GeneratedGuid NVARCHAR(255) = NEWID();
        
        INSERT INTO dbo.MOB_LOGINDET (LD_LOGINID, LD_USERSYSID, LD_DEVICEID, LD_LOGON, 
            LD_GUID, LD_IMEINO, LD_DEVICETYPE)
        VALUES (NEXT VALUE FOR dbo.seq_MOB_LoginId, @p_UserSysId, @p_DeviceId, GETDATE(), 
            @GeneratedGuid, @p_ImeiNo, @p_DeviceType);
        
        SET @p_LoginId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Login logged successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_MOB_GetDevicesByEmployee
-- Description: Retrieve all devices registered by an employee
-- ============================================================================
IF OBJECT_ID('dbo.usp_MOB_GetDevicesByEmployee', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_MOB_GetDevicesByEmployee;
GO

CREATE PROCEDURE dbo.usp_MOB_GetDevicesByEmployee
    @p_EmpSysId DECIMAL(38)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MD_EMPSYSID,
        MD_DEVICEID,
        MD_ACTIVE,
        MD_DEVICETYPE,
        MD_IMEINO,
        MD_CREATEDON,
        MD_UPDATEDON
    FROM dbo.MOB_APPDEVICE_DETAILS
    WHERE MD_EMPSYSID = @p_EmpSysId
    ORDER BY MD_UPDATEDON DESC;
END;
GO

PRINT 'Mobile App Management Procedures created successfully.';
GO
