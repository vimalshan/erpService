-- ==========================================
-- Module: AGENCY & VENDOR
-- Description: Agency and vendor master data procedures
-- Procedures for agency and vendor management
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateAgency
-- Purpose: Create a new travel agency
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateAgency
(
    @p_AgencyCode BIGINT,
    @p_AgencyName VARCHAR(100),
    @p_AgencyType VARCHAR(5),  -- Air, Train, Bus, Cab
    @p_Email VARCHAR(250),
    @p_Phone VARCHAR(15),
    @p_Address1 VARCHAR(100),
    @p_Address2 VARCHAR(100) = NULL,
    @p_Address3 VARCHAR(100) = NULL,
    @p_Address4 VARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if agency already exists
        IF EXISTS(SELECT 1 FROM AGENCY_MASTER WHERE AM_AGN_CODE = @p_AgencyCode)
            THROW 50001, 'Agency code already exists', 1;
        
        -- Insert new agency
        INSERT INTO AGENCY_MASTER
        (
            AM_AGN_CODE, AM_AGN_NAM, AM_AGN_TYP, AM_EML_ID, AM_PHN_NO,
            AM_AGN_ADD1, AM_AGN_ADD2, AM_AGN_ADD3, AM_AGN_ADD4,
            AM_MODIFIEDON, AM_MODIFIEDBY
        )
        VALUES
        (
            @p_AgencyCode, @p_AgencyName, @p_AgencyType, @p_Email, @p_Phone,
            @p_Address1, @p_Address2, @p_Address3, @p_Address4,
            GETDATE(), 1
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Agency created successfully' AS [Message],
               @p_AgencyCode AS [AgencyCode];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [AgencyCode];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_UpdateAgency
-- Purpose: Update agency details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UpdateAgency
(
    @p_AgencyCode BIGINT,
    @p_AgencyName VARCHAR(100) = NULL,
    @p_Email VARCHAR(250) = NULL,
    @p_Phone VARCHAR(15) = NULL,
    @p_ModifiedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if agency exists
        IF NOT EXISTS(SELECT 1 FROM AGENCY_MASTER WHERE AM_AGN_CODE = @p_AgencyCode)
            THROW 50001, 'Agency not found', 1;
        
        -- Update agency
        UPDATE AGENCY_MASTER
        SET AM_AGN_NAM = ISNULL(@p_AgencyName, AM_AGN_NAM),
            AM_EML_ID = ISNULL(@p_Email, AM_EML_ID),
            AM_PHN_NO = ISNULL(@p_Phone, AM_PHN_NO),
            AM_MODIFIEDBY = @p_ModifiedBy,
            AM_MODIFIEDON = GETDATE()
        WHERE AM_AGN_CODE = @p_AgencyCode;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Agency updated successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateVendor
-- Purpose: Create a new vendor (hotel/service provider)
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateVendor
(
    @p_VendorID BIGINT,
    @p_VendorName VARCHAR(65),
    @p_CategoryType CHAR(1),  -- V=Vendor, H=Hotel
    @p_Email VARCHAR(30) = NULL,
    @p_Phone VARCHAR(20) = NULL,
    @p_Address1 VARCHAR(30),
    @p_CityCode BIGINT = NULL,
    @p_PAN CHAR(10) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if vendor already exists
        IF EXISTS(SELECT 1 FROM VENDOR_MASTER WHERE VM_ID = @p_VendorID)
            THROW 50001, 'Vendor ID already exists', 1;
        
        -- Validate category type
        IF @p_CategoryType NOT IN ('V', 'H')
            THROW 50002, 'Category type must be V (Vendor) or H (Hotel)', 1;
        
        -- Insert new vendor
        INSERT INTO VENDOR_MASTER
        (
            VM_ID, VM_NAME, VM_CAT_TYPE, VM_PHN_NO,
            VM_ADD_LN1, VM_CIT_COD, VM_IT_PAN
        )
        VALUES
        (
            @p_VendorID, @p_VendorName, @p_CategoryType, @p_Phone,
            @p_Address1, @p_CityCode, @p_PAN
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Vendor created successfully' AS [Message],
               @p_VendorID AS [VendorID];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [VendorID];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RegisterAirline
-- Purpose: Register an airline in the system
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RegisterAirline
(
    @p_AirlineCode CHAR(3),
    @p_AirlineName VARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if airline already exists
        IF EXISTS(SELECT 1 FROM AIRLINE_MAST WHERE AIR_LIN_COD = @p_AirlineCode)
            THROW 50001, 'Airline code already exists', 1;
        
        -- Insert airline
        INSERT INTO AIRLINE_MAST
        (AIR_LIN_COD, AIR_LIN_NAM)
        VALUES
        (@p_AirlineCode, @p_AirlineName);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Airline registered successfully' AS [Message],
               @p_AirlineCode AS [AirlineCode];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [AirlineCode];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetAgencyDetails
-- Purpose: Retrieve agency details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetAgencyDetails
(
    @p_AgencyCode BIGINT = NULL,
    @p_AgencyType VARCHAR(5) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        AM_AGN_CODE AS [AgencyCode],
        AM_AGN_NAM AS [AgencyName],
        AM_AGN_TYP AS [AgencyType],
        AM_EML_ID AS [Email],
        AM_PHN_NO AS [Phone],
        AM_AGN_ADD1 AS [Address1],
        AM_AGN_ADD2 AS [Address2],
        AM_MODIFIEDON AS [LastModified]
    FROM AGENCY_MASTER
    WHERE (@p_AgencyCode IS NULL OR AM_AGN_CODE = @p_AgencyCode)
      AND (@p_AgencyType IS NULL OR AM_AGN_TYP = @p_AgencyType)
    ORDER BY AM_AGN_NAM;
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetVendorDetails
-- Purpose: Retrieve vendor details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetVendorDetails
(
    @p_VendorID BIGINT = NULL,
    @p_CategoryType CHAR(1) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        VM_ID AS [VendorID],
        VM_NAME AS [VendorName],
        VM_CAT_TYPE AS [CategoryType],
        VM_PHN_NO AS [Phone],
        VM_ADD_LN1 AS [Address],
        VM_BNK_NAM AS [BankName],
        VM_ACC_NO AS [AccountNumber]
    FROM VENDOR_MASTER
    WHERE (@p_VendorID IS NULL OR VM_ID = @p_VendorID)
      AND (@p_CategoryType IS NULL OR VM_CAT_TYPE = @p_CategoryType)
    ORDER BY VM_NAME;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
