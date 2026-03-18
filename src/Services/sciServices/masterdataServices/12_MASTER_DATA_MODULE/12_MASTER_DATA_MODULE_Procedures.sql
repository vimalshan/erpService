-- ==========================================
-- MASTER DATA MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Master Data Configuration
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterCompanyUnit', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterCompanyUnit;
GO
CREATE PROCEDURE dbo.usp_RegisterCompanyUnit
    @p_CompanyUnitCode CHAR(3),
    @p_CompanyUnitName VARCHAR(1000),
    @p_UnitID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.COMPANY_UNITMASTER (COMPANY_UNIT_CODE, COMPANY_UNIT_NAME)
            VALUES (@p_CompanyUnitCode, @p_CompanyUnitName);
            SET @p_UnitID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetCompanyUnits', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetCompanyUnits;
GO
CREATE PROCEDURE dbo.usp_GetCompanyUnits
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT COMPANY_UNIT_ID, COMPANY_UNIT_CODE, COMPANY_UNIT_NAME
        FROM dbo.COMPANY_UNITMASTER
        ORDER BY COMPANY_UNIT_CODE;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'MASTER_DATA_MODULE Procedures created successfully.';
GO
