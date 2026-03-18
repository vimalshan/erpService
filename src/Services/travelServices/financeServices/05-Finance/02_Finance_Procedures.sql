-- ==========================================
-- Module: FINANCE & ACCOUNTS PAYABLE
-- Description: Financial transaction and payment processing
-- Procedures for invoice, batch, and payment management
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateInvoiceBatch
-- Purpose: Create an invoice batch for agency payments
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateInvoiceBatch
(
    @p_UnitCode CHAR(3),
    @p_AgencyCode BIGINT,
    @p_TotalAmount DECIMAL(19,0) = NULL,
    @p_InvoiceNum VARCHAR(25) = NULL,
    @p_AdminRemarks VARCHAR(200) = NULL,
    @p_BatchNum BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate batch number
        SELECT @p_BatchNum = ISNULL(MAX(TM_BAT_NUM), 0) + 1
        FROM TRAVEL_BATCH_MAIN
        WHERE TM_UNT_COD = @p_UnitCode;
        
        -- Insert batch main record
        INSERT INTO TRAVEL_BATCH_MAIN
        (
            TM_UNT_COD, TM_BAT_NUM, TM_BAT_DAT, TM_INV_NUM,
            TM_BAT_STS, TM_ADM_REM, TM_AGN_COD, TM_TOTAL
        )
        VALUES
        (
            @p_UnitCode, @p_BatchNum, GETDATE(), @p_InvoiceNum,
            'N', @p_AdminRemarks, @p_AgencyCode, @p_TotalAmount
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Invoice batch created successfully' AS [Message],
               @p_BatchNum AS [BatchNumber];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [BatchNumber];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_AddBatchLineItem
-- Purpose: Add line items to batch
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_AddBatchLineItem
(
    @p_UnitCode CHAR(3),
    @p_BatchNum BIGINT,
    @p_BookingNum BIGINT,
    @p_TicketCost DECIMAL(19,0),
    @p_GSTAmount DECIMAL(19,0) = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate serial number
        DECLARE @SerialNum DECIMAL(38);
        SELECT @SerialNum = ISNULL(MAX(TS_SRL_NUM), 0) + 1
        FROM TRAVEL_BATCH_SUB
        WHERE TS_UNT_COD = @p_UnitCode AND TS_BAT_NUM = @p_BatchNum;
        
        -- Insert batch line item
        INSERT INTO TRAVEL_BATCH_SUB
        (
            TS_UNT_COD, TS_BAT_NUM, TS_SRL_NUM, TS_BOK_NUM,
            TS_TKT_CST, TS_STATUS
        )
        VALUES
        (
            @p_UnitCode, @p_BatchNum, @SerialNum, @p_BookingNum,
            @p_TicketCost, 'N'
        );
        
        -- Update batch total
        UPDATE TRAVEL_BATCH_MAIN
        SET TM_TOTAL = ISNULL(TM_TOTAL, 0) + @p_TicketCost + @p_GSTAmount
        WHERE TM_UNT_COD = @p_UnitCode AND TM_BAT_NUM = @p_BatchNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Line item added to batch' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ApproveInvoiceBatch
-- Purpose: Approve invoice batch for payment
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApproveInvoiceBatch
(
    @p_UnitCode CHAR(3),
    @p_BatchNum BIGINT,
    @p_ApprovedBy BIGINT,
    @p_ApprovalRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate batch exists
        IF NOT EXISTS(SELECT 1 FROM TRAVEL_BATCH_MAIN 
                     WHERE TM_UNT_COD = @p_UnitCode AND TM_BAT_NUM = @p_BatchNum)
            THROW 50001, 'Batch not found', 1;
        
        -- Update batch status
        UPDATE TRAVEL_BATCH_MAIN
        SET TM_BAT_STS = 'Y',  -- Y = Approved
            TM_FIN_REM = @p_ApprovalRemarks
        WHERE TM_UNT_COD = @p_UnitCode AND TM_BAT_NUM = @p_BatchNum;
        
        -- Update all line items status
        UPDATE TRAVEL_BATCH_SUB
        SET TS_STATUS = 'Y'
        WHERE TS_UNT_COD = @p_UnitCode AND TS_BAT_NUM = @p_BatchNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Batch approved successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessPayment
-- Purpose: Process payment for approved batches
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessPayment
(
    @p_BatchNum BIGINT,
    @p_PaymentAmount DECIMAL(19,0),
    @p_PaymentMode VARCHAR(3),  -- CHQ=Cheque, BNK=Bank, CSH=Cash
    @p_ChequeNum VARCHAR(20) = NULL,
    @p_ProcessedBy BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate transaction number
        DECLARE @TrnNum BIGINT;
        SELECT @TrnNum = ISNULL(MAX(AC_TRN_NUM), 0) + 1 FROM TRAVEL_ACCOUNT;
        
        -- Insert payment record
        INSERT INTO TRAVEL_ACCOUNT
        (
            AC_TRN_NUM, AC_UNT_COD, AC_DC_FLG, AC_TRN_AMT,
            AC_REM_MRK, AC_ACC_TYP
        )
        VALUES
        (
            @TrnNum, '001', 'C', @p_PaymentAmount,
            'Batch Payment: ' + CAST(@p_BatchNum AS VARCHAR), 'SET'
        );
        
        -- Update batch status
        UPDATE TRAVEL_BATCH_MAIN
        SET TM_BAT_STS = 'P'  -- P = Payment In Progress
        WHERE TM_BAT_NUM = @p_BatchNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Payment processed successfully' AS [Message],
               @TrnNum AS [TransactionNumber];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [TransactionNumber];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetBatchDetails
-- Purpose: Retrieve batch and payment details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetBatchDetails
(
    @p_UnitCode CHAR(3) = NULL,
    @p_BatchNum BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        tm.TM_UNT_COD AS [UnitCode],
        tm.TM_BAT_NUM AS [BatchNumber],
        tm.TM_BAT_DAT AS [BatchDate],
        tm.TM_INV_NUM AS [InvoiceNumber],
        COUNT(ts.TS_SRL_NUM) AS [LineItemCount],
        tm.TM_TOTAL AS [TotalAmount],
        tm.TM_BAT_STS AS [Status],
        ISNULL(SUM(ts.TS_TKT_CST), 0) AS [LineItemTotal]
    FROM TRAVEL_BATCH_MAIN tm
    LEFT JOIN TRAVEL_BATCH_SUB ts ON tm.TM_UNT_COD = ts.TS_UNT_COD 
                                    AND tm.TM_BAT_NUM = ts.TS_BAT_NUM
    WHERE (@p_UnitCode IS NULL OR tm.TM_UNT_COD = @p_UnitCode)
      AND (@p_BatchNum IS NULL OR tm.TM_BAT_NUM = @p_BatchNum)
    GROUP BY tm.TM_UNT_COD, tm.TM_BAT_NUM, tm.TM_BAT_DAT, tm.TM_INV_NUM,
             tm.TM_TOTAL, tm.TM_BAT_STS;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
