-- =========================================================================
-- TRUST MODULE - Stored Procedures and Functions
-- Database: PFDB
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Create Trust
IF OBJECT_ID('dbo.usp_CreateTrust', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_CreateTrust;
GO

CREATE PROCEDURE dbo.usp_CreateTrust
    @p_TrustCode CHAR(3),
    @p_TrustName VARCHAR(65),
    @p_TrustType CHAR(3),
    @p_StartDate DATETIME2(3),
    @p_Address1 VARCHAR(200),
    @p_CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.TRUST_MASTER (
            TRUST_CODE, TRUST_SHORT_NAME, TRUST_TYPE, TRUST_START_DATE,
            ADDRESS_LINE1, TRUST_STATUS, CREATED_DATE
        ) VALUES (
            @p_TrustCode, @p_TrustName, @p_TrustType, @p_StartDate,
            @p_Address1, 'A', GETDATE()
        );
        
        COMMIT TRANSACTION;
        PRINT 'Trust created successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE: Add Trust User Role
IF OBJECT_ID('dbo.usp_AddTrustUserRole', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_AddTrustUserRole;
GO

CREATE PROCEDURE dbo.usp_AddTrustUserRole
    @p_TrustCode CHAR(3),
    @p_RoleID INT,
    @p_RoleCode CHAR(3),
    @p_UserID VARCHAR(25),
    @p_UserNo BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO dbo.TRUST_ROLE (
            TR_TRUST_CODE, TR_ROLE_ID, TR_ROLE_CODE, TR_USER_ID, TR_USER_NO,
            TR_EFF_DATE, TR_STATUS
        ) VALUES (
            @p_TrustCode, @p_RoleID, @p_RoleCode, @p_UserID, @p_UserNo,
            GETDATE(), 'A'
        );
        
        PRINT 'User role assigned successfully';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- VIEW: Active Trusts
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_ActiveTrusts' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_ActiveTrusts AS
    SELECT 
        TRUST_CODE,
        TRUST_SHORT_NAME,
        TRUST_TYPE,
        TRUST_START_DATE,
        ADDRESS_LINE1,
        EMAIL,
        PHONE_NO,
        TRUST_STATUS
    FROM dbo.TRUST_MASTER
    WHERE TRUST_STATUS = 'A';
END
GO

-- VIEW: Trust User Roles
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_TrustUserRoles' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_TrustUserRoles AS
    SELECT 
        tr.TR_TRUST_CODE,
        tr.TR_USER_ID,
        tr.TR_ROLE_CODE,
        rm.ROLE_NAME,
        tr.TR_EFF_DATE,
        tr.TR_STATUS
    FROM dbo.TRUST_ROLE tr
    LEFT JOIN dbo.ROLE_MASTER rm ON tr.TR_ROLE_ID = rm.ROLE_CODE
    WHERE tr.TR_STATUS = 'A';
END
GO

PRINT 'Trust Module Procedures created successfully!';
GO
