-- ==========================================
-- Module: BOOKING
-- Description: Booking request and confirmation procedures
-- Procedures and triggers for booking management
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateBookingRequest
-- Purpose: Create a new booking request from employee
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateBookingRequest
(
    @p_UserCode VARCHAR(25),
    @p_UserNum BIGINT,
    @p_BookingType VARCHAR(1),  -- S=Stay, T=Travel, L=Local Conveyance
    @p_FromCity BIGINT,
    @p_ToCity BIGINT,
    @p_FromLocation VARCHAR(200),
    @p_ToLocation VARCHAR(200),
    @p_DepartDate DATETIME2(3),
    @p_ReturnDate DATETIME2(3),
    @p_PersonName VARCHAR(200),
    @p_BudgetAmount DECIMAL(19,0) = NULL,
    @p_RequestNum BIGINT OUTPUT,
    @p_BookingNum BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate dates
        IF @p_DepartDate < CAST(GETDATE() AS DATE)
            THROW 50001, 'Departure date cannot be in past', 1;
        
        IF @p_ReturnDate < @p_DepartDate
            THROW 50002, 'Return date must be after departure date', 1;
        
        -- Generate booking request number
        SELECT @p_BookingNum = ISNULL(MAX(BK_BOK_NUM), 0) + 1 FROM BOOK_REQUEST;
        
        -- Insert booking request
        INSERT INTO BOOK_REQUEST
        (
            BK_BOK_NUM, BK_SRL_NUM, BK_BOK_TYP, BK_USR_COD, BK_USR_NUM,
            BK_FRO_DAT, BK_RET_DAT, BK_FRO_CIT, BK_TO_CIT,
            BK_FRO_LOC, BK_TO_LOC, BK_PER_NAM, BK_APP_STS,
            BK_BUD_AMT, BK_ADM_SLF, BK_PER_STS
        )
        VALUES
        (
            @p_BookingNum, 1, @p_BookingType, @p_UserCode, @p_UserNum,
            @p_DepartDate, @p_ReturnDate, @p_FromCity, @p_ToCity,
            @p_FromLocation, @p_ToLocation, @p_PersonName, 'N',
            @p_BudgetAmount, 'N', 'S'
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Booking request created successfully' AS [Message],
               @p_BookingNum AS [BookingNumber];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [BookingNumber];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ConfirmBooking
-- Purpose: Confirm a booking request and create confirmation record
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ConfirmBooking
(
    @p_BookingNum BIGINT,
    @p_ModeOfTravel BIGINT,
    @p_VendorCode BIGINT = NULL,
    @p_TicketNumber VARCHAR(25) = NULL,
    @p_AdminRemarks VARCHAR(2000) = NULL,
    @p_ConfirmationNum BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate booking exists
        IF NOT EXISTS(SELECT 1 FROM BOOK_REQUEST WHERE BK_BOK_NUM = @p_BookingNum)
            THROW 50001, 'Invalid booking number', 1;
        
        -- Generate confirmation number
        SELECT @p_ConfirmationNum = ISNULL(MAX(BK_CNF_NUM), 0) + 1 FROM BOOK_CONFIRMATION;
        
        -- Get booking details
        DECLARE @FromCity BIGINT, @ToCity BIGINT, @FromDate DATETIME2(3), @ToDate DATETIME2(3);
        SELECT 
            @FromCity = BK_FRO_CIT,
            @ToCity = BK_TO_CIT,
            @FromDate = BK_FRO_DAT,
            @ToDate = BK_RET_DAT
        FROM BOOK_REQUEST
        WHERE BK_BOK_NUM = @p_BookingNum;
        
        -- Create confirmation record
        INSERT INTO BOOK_CONFIRMATION
        (
            BK_CNF_NUM, BK_CNF_SRL, BK_BOK_NUM, BK_SRL_NUM, BK_MOD_COD,
            BK_FRO_CIT, BK_TO_CIT, BK_FRO_DAT, BK_TO_DAT,
            BK_VND_COD, BK_TCK_NUM, BK_ADM_RMK, BK_STS_COD, BK_REQ_DAT
        )
        VALUES
        (
            @p_ConfirmationNum, 1, @p_BookingNum, 1, @p_ModeOfTravel,
            @FromCity, @ToCity, @FromDate, @ToDate,
            @p_VendorCode, @p_TicketNumber, @p_AdminRemarks, 'Y', GETDATE()
        );
        
        -- Update booking request status
        UPDATE BOOK_REQUEST
        SET BK_APP_STS = 'C', BK_CNF_NUM = @p_ConfirmationNum
        WHERE BK_BOK_NUM = @p_BookingNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Booking confirmed successfully' AS [Message],
               @p_ConfirmationNum AS [ConfirmationNumber];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [ConfirmationNumber];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CancelBooking
-- Purpose: Cancel a booking request
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CancelBooking
(
    @p_BookingNum BIGINT,
    @p_CancellationRemarks VARCHAR(200),
    @p_CancelledBy VARCHAR(25)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate booking exists and not already cancelled
        IF NOT EXISTS(SELECT 1 FROM BOOK_REQUEST WHERE BK_BOK_NUM = @p_BookingNum AND BK_APP_STS != 'C')
            THROW 50001, 'Invalid or already cancelled booking', 1;
        
        -- Update booking status
        UPDATE BOOK_REQUEST
        SET BK_APP_STS = 'K',
            BK_CAN_DAT = GETDATE(),
            BK_CAN_REM = @p_CancellationRemarks,
            BK_CAN_USR = @p_CancelledBy
        WHERE BK_BOK_NUM = @p_BookingNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Booking cancelled successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetBookingDetails
-- Purpose: Retrieve booking request and confirmation details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetBookingDetails
(
    @p_BookingNum BIGINT = NULL,
    @p_ConfirmationNum BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- If confirmation number provided, get from confirmation
    IF @p_ConfirmationNum IS NOT NULL
    BEGIN
        SELECT 
            bc.BK_CNF_NUM AS [ConfirmationNumber],
            bc.BK_BOK_NUM AS [BookingNumber],
            bc.BK_MOD_COD AS [ModeOfTravel],
            bc.BK_FRO_CIT AS [FromCity],
            bc.BK_TO_CIT AS [ToCity],
            bc.BK_FRO_DAT AS [DepartureDate],
            bc.BK_TO_DAT AS [ReturnDate],
            bc.BK_VND_COD AS [VendorCode],
            bc.BK_TCK_NUM AS [TicketNumber],
            bc.BK_STS_COD AS [Status],
            br.BK_PER_NAM AS [PersonName],
            br.BK_USR_COD AS [UserCode]
        FROM BOOK_CONFIRMATION bc
        LEFT JOIN BOOK_REQUEST br ON bc.BK_BOK_NUM = br.BK_BOK_NUM
        WHERE bc.BK_CNF_NUM = @p_ConfirmationNum;
    END
    ELSE IF @p_BookingNum IS NOT NULL
    BEGIN
        SELECT 
            br.BK_BOK_NUM AS [BookingNumber],
            br.BK_BOK_TYP AS [BookingType],
            br.BK_USR_COD AS [UserCode],
            br.BK_FRO_DAT AS [DepartureDate],
            br.BK_RET_DAT AS [ReturnDate],
            br.BK_FRO_LOC AS [FromLocation],
            br.BK_TO_LOC AS [ToLocation],
            br.BK_PER_NAM AS [PersonName],
            br.BK_APP_STS AS [Status],
            br.BK_BUD_AMT AS [BudgetAmount],
            br.BK_CNF_NUM AS [ConfirmationNumber],
            ISNULL(br.BK_CAN_REM, 'No cancellation') AS [CancellationRemark]
        FROM BOOK_REQUEST br
        WHERE br.BK_BOK_NUM = @p_BookingNum;
    END;
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_BookingConfirmation_ValidateStatus
-- Purpose: Ensure booking request exists before confirmation
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_BookingConfirmation_ValidateStatus
ON dbo.BOOK_CONFIRMATION
BEFORE INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS(
        SELECT 1 FROM BOOK_REQUEST br
        INNER JOIN inserted i ON br.BK_BOK_NUM = i.BK_BOK_NUM
        WHERE br.BK_APP_STS NOT IN ('C', 'K')
    )
    BEGIN
        RAISERROR('Booking request not found or already cancelled', 16, 1);
        ROLLBACK;
    END;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
