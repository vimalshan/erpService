-- ==========================================
-- TOURDB Stored Procedures & Functions
-- Docker Initialization
-- ==========================================

USE TOURDB;
GO

-- ══════════════════════════════════════════
-- TOUR MODULE - Functions
-- ══════════════════════════════════════════

IF OBJECT_ID('dbo.fn_CalculateTourCostPerPerson', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateTourCostPerPerson;
GO
CREATE FUNCTION dbo.fn_CalculateTourCostPerPerson (@p_TourID BIGINT, @p_ParticipantCount INT)
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @CostPerPerson DECIMAL(19,0) = 0, @TotalCost DECIMAL(19,0);
    BEGIN TRY
        SELECT @TotalCost = TOUR_PACKAGE_COST FROM dbo.TOUR_PACKAGE WHERE TOUR_ID = @p_TourID;
        IF @p_ParticipantCount > 0
            SET @CostPerPerson = CAST(@TotalCost / @p_ParticipantCount AS DECIMAL(19,0));
    END TRY BEGIN CATCH SET @CostPerPerson = 0; END CATCH
    RETURN @CostPerPerson;
END;
GO

-- ══════════════════════════════════════════
-- TOUR MODULE - Procedures
-- ══════════════════════════════════════════

IF OBJECT_ID('dbo.usp_PlanTourPackage', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_PlanTourPackage;
GO
CREATE PROCEDURE dbo.usp_PlanTourPackage
    @p_TourName VARCHAR(200), @p_Destination VARCHAR(100), @p_StartDate DATE, @p_EndDate DATE,
    @p_totalCost DECIMAL(19,0), @p_MaxParticipants INT, @p_PlannerID BIGINT, @p_TourID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.TOUR_PACKAGE (TOUR_NAME, DESTINATION, START_DATE, END_DATE, TOUR_PACKAGE_COST,
            MAX_PARTICIPANTS, TOUR_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_TourName, @p_Destination, @p_StartDate, @p_EndDate, @p_totalCost, @p_MaxParticipants, 'P', @p_PlannerID, GETDATE());
        SET @p_TourID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RegisterTourParticipant', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterTourParticipant;
GO
CREATE PROCEDURE dbo.usp_RegisterTourParticipant
    @p_TourID BIGINT, @p_ParticipantID BIGINT, @p_RegistrationDate DATE,
    @p_RegisteredBy BIGINT, @p_RegistrationID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @CurrentCount INT, @MaxPart INT;
        SELECT @MaxPart = MAX_PARTICIPANTS FROM dbo.TOUR_PACKAGE WHERE TOUR_ID = @p_TourID;
        SELECT @CurrentCount = COUNT(*) FROM dbo.TOUR_REGISTRATION WHERE TOUR_ID = @p_TourID AND REGISTRATION_STATUS = 'R';
        IF @CurrentCount >= @MaxPart
        BEGIN
            RAISERROR('Tour is fully booked.', 16, 1);
            RETURN;
        END
        INSERT INTO dbo.TOUR_REGISTRATION (TOUR_ID, PARTICIPANT_ID, REGISTRATION_DATE, REGISTRATION_STATUS,
            PAYMENT_STATUS, AMOUNT_PAID, REGISTERED_BY, REGISTERED_ON)
        VALUES (@p_TourID, @p_ParticipantID, @p_RegistrationDate, 'R', 'P', 0, @p_RegisteredBy, GETDATE());
        SET @p_RegistrationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

-- ══════════════════════════════════════════
-- TRANSACTION MODULE - Procedures
-- ══════════════════════════════════════════

IF OBJECT_ID('dbo.usp_GetEmployeeJVById', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetEmployeeJVById;
GO
CREATE PROCEDURE dbo.usp_GetEmployeeJVById @p_JEID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT m.*, s.* FROM JVEMP_MAIN m
    LEFT JOIN JVEMP_SUB s ON m.JE_ID = s.JES_JEID
    WHERE m.JE_ID = @p_JEID;
END;
GO

IF OBJECT_ID('dbo.usp_GetSupplierJVById', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetSupplierJVById;
GO
CREATE PROCEDURE dbo.usp_GetSupplierJVById @p_JSID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT m.*, s.* FROM JVSUP_MAIN m
    LEFT JOIN JVSUP_SUB s ON m.JS_ID = s.JSS_JSID
    WHERE m.JS_ID = @p_JSID;
END;
GO

IF OBJECT_ID('dbo.usp_GetTravelBatchById', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetTravelBatchById;
GO
CREATE PROCEDURE dbo.usp_GetTravelBatchById @p_BatchID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT m.*, s.* FROM TRAVEL_BATCHMAIN m
    LEFT JOIN TRAVEL_BATCHSUB s ON m.TB_ID = s.TBS_BATCHID
    WHERE m.TB_ID = @p_BatchID;
END;
GO

IF OBJECT_ID('dbo.usp_GetTravelBatchesByStatus', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetTravelBatchesByStatus;
GO
CREATE PROCEDURE dbo.usp_GetTravelBatchesByStatus @p_Status CHAR(1)
AS BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TRAVEL_BATCHMAIN WHERE TB_STATUS = @p_Status ORDER BY TB_CREATEDON DESC;
END;
GO

IF OBJECT_ID('dbo.usp_GetEmployeePaymentsByEmployee', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetEmployeePaymentsByEmployee;
GO
CREATE PROCEDURE dbo.usp_GetEmployeePaymentsByEmployee @p_EmpSysId VARCHAR(255)
AS BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TRAVEL_EMPPAYDET WHERE TEP_EMPSYSID = @p_EmpSysId ORDER BY TEP_PAYDATE DESC;
END;
GO

IF OBJECT_ID('dbo.usp_GetAirlineInvoicesByBooking', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAirlineInvoicesByBooking;
GO
CREATE PROCEDURE dbo.usp_GetAirlineInvoicesByBooking @p_BookId BIGINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TICKET_AIRLINEINOVICE WHERE TAI_BOOKID = @p_BookId;
END;
GO

PRINT 'All stored procedures created successfully.';
GO
