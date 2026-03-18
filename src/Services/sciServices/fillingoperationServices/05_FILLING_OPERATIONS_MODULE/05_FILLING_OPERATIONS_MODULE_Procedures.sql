-- ==========================================
-- FILLING OPERATIONS MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Filling Plant & Operations
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterFillingPlant', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterFillingPlant;
GO
CREATE PROCEDURE dbo.usp_RegisterFillingPlant
    @p_CompanyUnitID INT,
    @p_PlantName VARCHAR(40),
    @p_Location VARCHAR(20),
    @p_CreatedBy INT,
    @p_PlantID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.FILLING_PLANT (COMPANY_UNIT_ID, FILLING_PLANT_NAME, LOCATION, SCI_USER_ID_CREATED, CREATION_DATE)
            VALUES (@p_CompanyUnitID, @p_PlantName, @p_Location, @p_CreatedBy, GETDATE());
            SET @p_PlantID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetFillingPointCapacity', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetFillingPointCapacity;
GO
CREATE PROCEDURE dbo.usp_GetFillingPointCapacity
    @p_FillingPointGroupID INT,
    @p_ProductID INT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT fc.FILLING_POINT_GROUP_ID, fc.MAIN_PRODUCT_ID, fc.CAPACITY_PER_SHIFT, fc.USAGE_PRIORITY
        FROM dbo.FILLING_CAPACITY fc
        WHERE fc.FILLING_POINT_GROUP_ID = @p_FillingPointGroupID
        AND fc.MAIN_PRODUCT_ID = @p_ProductID
        ORDER BY fc.USAGE_PRIORITY;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'FILLING_OPERATIONS_MODULE Procedures created successfully.';
GO
