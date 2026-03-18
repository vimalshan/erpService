-- ==========================================
-- PRODUCTION MANAGEMENT MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Production Planning
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterProductionPlant', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterProductionPlant;
GO
CREATE PROCEDURE dbo.usp_RegisterProductionPlant
    @p_CompanyUnitID INT,
    @p_PlantName VARCHAR(60),
    @p_Location VARCHAR(25),
    @p_CreatedBy INT,
    @p_PlantID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.PRODUCTION_PLANT (COMPANY_UNIT_ID, PLANT_NAME, LOCATION, SCI_USER_ID_CREATED, CREATION_DATE)
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

PRINT 'PRODUCTION_MANAGEMENT_MODULE Procedures created successfully.';
GO
