-- ============================================================================
-- Module: Employee Pride Management - Stored Procedures
-- Purpose: Procedures for managing employee pride moments and achievements
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- Procedure: usp_PRIDE_CreatePrideMoment
-- Description: Create a new employee pride moment record
-- ============================================================================
IF OBJECT_ID('dbo.usp_PRIDE_CreatePrideMoment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PRIDE_CreatePrideMoment;
GO

CREATE PROCEDURE dbo.usp_PRIDE_CreatePrideMoment
    @p_Title VARCHAR(50),
    @p_Body NVARCHAR(MAX),
    @p_EmployeeSysId DECIMAL(38),
    @p_Footer VARCHAR(500),
    @p_Location VARCHAR(100),
    @p_ImagePath VARCHAR(200),
    @p_ModifiedBy BIGINT,
    @p_PrideMomentId DECIMAL(38) OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.MOMENT_PRIDE (MOMENTPRIDE_ID, MOMENTPRIDE_TITLE, MOMENTPRIDE_BODY, 
            MOMENTPRIDE_EMPSYSID, MOMENTPRIDE_FOOTER, MOMENTPRIDE_LOCATION, MOMENTPRIDE_IMAGE, 
            MOMENTPRIDE_MODIFIEDBY, MOMENTPRIDE_MODIFIEDON)
        VALUES (NEXT VALUE FOR dbo.seq_MOMENT_PRIDE_Id, @p_Title, @p_Body, @p_EmployeeSysId, 
            @p_Footer, @p_Location, @p_ImagePath, @p_ModifiedBy, GETDATE());
        
        SET @p_PrideMomentId = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Pride moment created successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_PRIDE_GetPrideMomentsByEmployee
-- Description: Retrieve all pride moments for an employee
-- ============================================================================
IF OBJECT_ID('dbo.usp_PRIDE_GetPrideMomentsByEmployee', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PRIDE_GetPrideMomentsByEmployee;
GO

CREATE PROCEDURE dbo.usp_PRIDE_GetPrideMomentsByEmployee
    @p_EmployeeSysId DECIMAL(38)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MOMENTPRIDE_ID,
        MOMENTPRIDE_TITLE,
        MOMENTPRIDE_BODY,
        MOMENTPRIDE_EMPSYSID,
        MOMENTPRIDE_FOOTER,
        MOMENTPRIDE_LOCATION,
        MOMENTPRIDE_IMAGE,
        MOMENTPRIDE_MODIFIEDBY,
        MOMENTPRIDE_MODIFIEDON
    FROM dbo.MOMENT_PRIDE
    WHERE MOMENTPRIDE_EMPSYSID = @p_EmployeeSysId
    ORDER BY MOMENTPRIDE_MODIFIEDON DESC;
END;
GO

-- ============================================================================
-- Procedure: usp_PRIDE_GetAllPrideMoments
-- Description: Retrieve all pride moments (paginated)
-- ============================================================================
IF OBJECT_ID('dbo.usp_PRIDE_GetAllPrideMoments', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PRIDE_GetAllPrideMoments;
GO

CREATE PROCEDURE dbo.usp_PRIDE_GetAllPrideMoments
    @p_PageNumber INT = 1,
    @p_PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@p_PageNumber - 1) * @p_PageSize;
    
    SELECT 
        MOMENTPRIDE_ID,
        MOMENTPRIDE_TITLE,
        MOMENTPRIDE_BODY,
        MOMENTPRIDE_EMPSYSID,
        MOMENTPRIDE_FOOTER,
        MOMENTPRIDE_LOCATION,
        MOMENTPRIDE_IMAGE,
        MOMENTPRIDE_MODIFIEDBY,
        MOMENTPRIDE_MODIFIEDON
    FROM dbo.MOMENT_PRIDE
    ORDER BY MOMENTPRIDE_MODIFIEDON DESC
    OFFSET @Offset ROWS
    FETCH NEXT @p_PageSize ROWS ONLY;
END;
GO

-- ============================================================================
-- Procedure: usp_PRIDE_UpdatePrideMoment
-- Description: Update an existing pride moment
-- ============================================================================
IF OBJECT_ID('dbo.usp_PRIDE_UpdatePrideMoment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PRIDE_UpdatePrideMoment;
GO

CREATE PROCEDURE dbo.usp_PRIDE_UpdatePrideMoment
    @p_PrideMomentId DECIMAL(38),
    @p_Title VARCHAR(50),
    @p_Body NVARCHAR(MAX),
    @p_Footer VARCHAR(500),
    @p_Location VARCHAR(100),
    @p_ImagePath VARCHAR(200),
    @p_ModifiedBy BIGINT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.MOMENT_PRIDE WHERE MOMENTPRIDE_ID = @p_PrideMomentId)
        BEGIN
            SET @p_ErrorMessage = 'Pride moment not found.';
            RETURN;
        END
        
        UPDATE dbo.MOMENT_PRIDE
        SET MOMENTPRIDE_TITLE = @p_Title,
            MOMENTPRIDE_BODY = @p_Body,
            MOMENTPRIDE_FOOTER = @p_Footer,
            MOMENTPRIDE_LOCATION = @p_Location,
            MOMENTPRIDE_IMAGE = @p_ImagePath,
            MOMENTPRIDE_MODIFIEDBY = @p_ModifiedBy,
            MOMENTPRIDE_MODIFIEDON = GETDATE()
        WHERE MOMENTPRIDE_ID = @p_PrideMomentId;
        
        SET @p_ErrorMessage = 'Pride moment updated successfully.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

PRINT 'Employee Pride Management Procedures created successfully.';
GO
