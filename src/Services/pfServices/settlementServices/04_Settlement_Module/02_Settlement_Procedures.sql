-- =========================================================================
-- SETTLEMENT MODULE - Stored Procedures and Functions
-- Database: PFDB
-- Module: PF Settlement and Withdrawal Management
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Create Settlement Request
IF OBJECT_ID('dbo.usp_CreateSettlementRequest', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_CreateSettlementRequest;
GO

CREATE PROCEDURE dbo.usp_CreateSettlementRequest
    @p_SettlementNum BIGINT,
    @p_MemberNo BIGINT,
    @p_SettlementType CHAR(1),
    @p_SettlementAmount DECIMAL(19,0),
    @p_SettlementDate DATETIME2(3),
    @p_CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.SET_MAIN (
            ST_SET_NUM, ST_MEMBER_NO, ST_SET_TYPE, ST_SETTLEMENT_AMOUNT,
            ST_SET_DATE, ST_UPDON, ST_UPDBY_EMP_SYSID, ST_STATUS
        ) VALUES (
            @p_SettlementNum, @p_MemberNo, @p_SettlementType, @p_SettlementAmount,
            @p_SettlementDate, GETDATE(), @p_CreatedBy, 'P'
        );
        
        COMMIT TRANSACTION;
        PRINT 'Settlement request created successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- VIEW: Settlement Summary
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_SettlementSummary' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_SettlementSummary AS
    SELECT 
        ST_SET_NUM,
        ST_MEMBER_NO,
        ST_SET_TYPE,
        ST_SETTLEMENT_AMOUNT,
        ST_SET_DATE,
        ST_STATUS,
        COUNT(*) OVER (PARTITION BY ST_MEMBER_NO) AS SettlementCount
    FROM dbo.SET_MAIN
    WHERE ST_STATUS IN ('P', 'A', 'C');
END
GO

PRINT 'Settlement Module Procedures created successfully!';
GO
