-- ==========================================
-- SECURITY MODULE - Stored Procedures
-- Database: SCIDB
-- Module: User, Role & Access Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_AssignUserRole', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AssignUserRole;
GO
CREATE PROCEDURE dbo.usp_AssignUserRole
    @p_UserID BIGINT,
    @p_RoleID BIGINT,
    @p_StartDate DATETIME2,
    @p_EndDate DATETIME2 = NULL,
    @p_CreatedBy VARCHAR(25)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.USER_ROLE (UR_USR_NUM, UR_ROL_COD, UR_STR_DAT, UR_END_DAT, UR_UPD_USR, UR_UPD_NUM, UR_UPD_DAT)
            VALUES (@p_UserID, @p_RoleID, @p_StartDate, @p_EndDate, @p_CreatedBy, @p_UserID, GETDATE());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RevokeUserRole', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RevokeUserRole;
GO
CREATE PROCEDURE dbo.usp_RevokeUserRole
    @p_UserID BIGINT,
    @p_RoleID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            DELETE FROM dbo.USER_ROLE
            WHERE UR_USR_NUM = @p_UserID AND UR_ROL_COD = @p_RoleID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetUserRoles', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetUserRoles;
GO
CREATE PROCEDURE dbo.usp_GetUserRoles
    @p_UserID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT ur.UR_ROL_COD, rm.RL_ROL_NAM, ur.UR_STR_DAT, ur.UR_END_DAT
        FROM dbo.USER_ROLE ur
        INNER JOIN dbo.ROLE_MAST rm ON ur.UR_ROL_COD = rm.RL_ROL_COD
        WHERE ur.UR_USR_NUM = @p_UserID
        AND (ur.UR_END_DAT IS NULL OR ur.UR_END_DAT >= GETDATE());
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'SECURITY_MODULE Procedures created successfully.';
GO
