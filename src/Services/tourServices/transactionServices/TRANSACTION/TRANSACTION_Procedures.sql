-- ==========================================
-- Module: TRANSACTION
-- Description: Stored Procedures for Transaction Operations
-- Database: TOURDB
-- ==========================================

USE TOURDB;
GO

-- ── Employee Journal Voucher Procedures ──────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_GetEmployeeJVById]
    @JvBatchId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*, s.*
    FROM [JVEMP_MAIN] m
    LEFT JOIN [JVEMP_SUB] s ON m.JV_BATCHID = s.JV_BATCHID
    WHERE m.JV_BATCHID = @JvBatchId;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetEmployeeJVsByEmployee]
    @EmpSysId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*
    FROM [JVEMP_MAIN] m
    WHERE m.JV_EMPSYSID = @EmpSysId
    ORDER BY m.JV_CREATEDON DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetEmployeeJVSummary]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.JV_BATCHID, m.JV_TPID, m.JV_TYPE, m.JV_DATE,
        m.JV_EMPSYSID, m.JV_STATUS, m.JV_TRNTYPE,
        m.JV_ORAREFNO, m.JV_NETAMT, m.JV_PAYUNITID,
        COUNT(s.JV_SUBID) AS LineCount
    FROM [JVEMP_MAIN] m
    LEFT JOIN [JVEMP_SUB] s ON m.JV_BATCHID = s.JV_BATCHID
    GROUP BY m.JV_BATCHID, m.JV_TPID, m.JV_TYPE, m.JV_DATE,
             m.JV_EMPSYSID, m.JV_STATUS, m.JV_TRNTYPE,
             m.JV_ORAREFNO, m.JV_NETAMT, m.JV_PAYUNITID
    ORDER BY m.JV_CREATEDON DESC;
END;
GO

-- ── Supplier Journal Voucher Procedures ──────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_GetSupplierJVById]
    @JvId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*, s.*
    FROM [JVSUP_MAIN] m
    LEFT JOIN [JVSUP_SUB] s ON m.JV_ID = s.JV_ID
    WHERE m.JV_ID = @JvId;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetSupplierJVsByVendor]
    @VendorId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*
    FROM [JVSUP_MAIN] m
    WHERE m.JV_VENDORID = @VendorId
    ORDER BY m.JV_CREATEDON DESC;
END;
GO

-- ── Travel Batch Procedures ──────────────────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_GetTravelBatchById]
    @BatchId VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*, s.*
    FROM [TRAVEL_BATCHMAIN] m
    LEFT JOIN [TRAVEL_BATCHSUB] s ON m.BATCH_ID = s.BATCHSUB_BATCHID
    WHERE m.BATCH_ID = @BatchId;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetTravelBatchesByStatus]
    @Status VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*
    FROM [TRAVEL_BATCHMAIN] m
    WHERE m.BATCH_STATUS = @Status
    ORDER BY m.BATCH_CREATEDON DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetTravelBatchSummary]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.BATCH_ID, m.BATCH_BATCHDATE, m.BATCH_STATUS,
        m.BATCH_BILAMT, m.BATCH_APPAMT, m.BATCH_TOTPAY,
        m.BATCH_VENDORID, m.BATCH_TYPE, m.BATCH_CREATEDBY,
        COUNT(s.BATCHSUB_ID) AS SubItemCount
    FROM [TRAVEL_BATCHMAIN] m
    LEFT JOIN [TRAVEL_BATCHSUB] s ON m.BATCH_ID = s.BATCHSUB_BATCHID
    GROUP BY m.BATCH_ID, m.BATCH_BATCHDATE, m.BATCH_STATUS,
             m.BATCH_BILAMT, m.BATCH_APPAMT, m.BATCH_TOTPAY,
             m.BATCH_VENDORID, m.BATCH_TYPE, m.BATCH_CREATEDBY
    ORDER BY m.BATCH_CREATEDON DESC;
END;
GO

-- ── Employee Payment Procedures ──────────────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_GetEmployeePaymentsByEmployee]
    @EmpSysId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [JVEMPPAY_DET]
    WHERE PAY_EMPSYSID = @EmpSysId
    ORDER BY PAY_DATE DESC;
END;
GO

-- ── Airline Invoice Procedures ───────────────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_GetAirlineInvoicesByBooking]
    @BookCnfId VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [TICKET_AIRLINEINOVICE]
    WHERE AIRTICKET_BOOKCNFID = @BookCnfId
    ORDER BY AIRTICKET_INVOICEDATE DESC;
END;
GO

-- ── Batch Reconciliation ─────────────────────────────────────────────────────

CREATE OR ALTER PROCEDURE [dbo].[usp_ReconcileBatchTotals]
    @BatchId VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.BATCH_ID,
        m.BATCH_BILAMT AS HeaderTotal,
        SUM(CAST(ISNULL(s.BATCHSUB_TOTAMT, '0') AS DECIMAL(19,2))) AS SubItemTotal,
        m.BATCH_BILAMT - SUM(CAST(ISNULL(s.BATCHSUB_TOTAMT, '0') AS DECIMAL(19,2))) AS Difference
    FROM [TRAVEL_BATCHMAIN] m
    LEFT JOIN [TRAVEL_BATCHSUB] s ON m.BATCH_ID = s.BATCHSUB_BATCHID
    WHERE m.BATCH_ID = @BatchId
    GROUP BY m.BATCH_ID, m.BATCH_BILAMT;
END;
GO

PRINT 'TRANSACTION Module - Stored Procedures created successfully.';
GO
