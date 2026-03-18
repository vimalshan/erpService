-- ==========================================
-- Module: INSURANCE
-- Description: Travel insurance procedures and management
-- Procedures for insurance processing
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RegisterTravelInsurance
-- Purpose: Register travel insurance for an employee
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RegisterTravelInsurance
(
    @p_CompanyCode CHAR(3),
    @p_PlanNum BIGINT,
    @p_InsuranceType CHAR(3),
    @p_PassportNum VARCHAR(50) = NULL,
    @p_VisaPlace VARCHAR(50) = NULL,
    @p_Nominee1 VARCHAR(200) = NULL,
    @p_Nominee2 VARCHAR(200) = NULL,
    @p_Remarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert insurance record
        INSERT INTO TRAVEL_INSURANCE
        (
            IN_COM_COD, IN_PLN_NUM, IN_INS_TYP, IN_PASS_NUM,
            IN_VIS_PLC, IN_NOM_NAM1, IN_NOM_NAM2, IN_INS_STS,
            IN_UPD_DAT, IN_REM_MRK
        )
        VALUES
        (
            @p_CompanyCode, @p_PlanNum, @p_InsuranceType, @p_PassportNum,
            @p_VisaPlace, @p_Nominee1, @p_Nominee2, 'A',
            GETDATE(), @p_Remarks
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Travel insurance registered successfully' AS [Message],
               @p_PlanNum AS [PlanNumber];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [PlanNumber];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetInsuranceDetails
-- Purpose: Retrieve insurance details for a travel plan
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetInsuranceDetails
(
    @p_CompanyCode CHAR(3) = NULL,
    @p_PlanNum BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IN_COM_COD AS [CompanyCode],
        IN_PLN_NUM AS [PlanNumber],
        IN_INS_TYP AS [InsuranceType],
        IN_PASS_NUM AS [PassportNumber],
        IN_ISS_DAT AS [IssueDate],
        IN_VIS_DAT AS [VisaDate],
        IN_NOM_NAM1 AS [Nominee1],
        IN_NOM_NAM2 AS [Nominee2],
        IN_INS_STS AS [Status],
        IN_CRT_NUM AS [CertificateNumber],
        IN_UPD_DAT AS [LastUpdated],
        IN_REM_MRK AS [Remarks]
    FROM TRAVEL_INSURANCE
    WHERE (@p_CompanyCode IS NULL OR IN_COM_COD = @p_CompanyCode)
      AND (@p_PlanNum IS NULL OR IN_PLN_NUM = @p_PlanNum)
    ORDER BY IN_UPD_DAT DESC;
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_UpdateInsuranceStatus
-- Purpose: Update insurance certificate status
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_UpdateInsuranceStatus
(
    @p_CompanyCode CHAR(3),
    @p_PlanNum BIGINT,
    @p_Status CHAR(1),  -- A=Active, I=Inactive, E=Expired
    @p_CertificateNum VARCHAR(200) = NULL,
    @p_UpdatedBy BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Update insurance record
        UPDATE TRAVEL_INSURANCE
        SET IN_INS_STS = @p_Status,
            IN_CRT_NUM = ISNULL(@p_CertificateNum, IN_CRT_NUM),
            IN_UPD_DAT = GETDATE(),
            IN_UPD_UNUM = @p_UpdatedBy
        WHERE IN_COM_COD = @p_CompanyCode AND IN_PLN_NUM = @p_PlanNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Insurance status updated successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
