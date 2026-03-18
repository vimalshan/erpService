-- ==========================================
-- Module: TOUR
-- Description: Tour Package & Registration Procedures
-- Database: TOURDB
-- Created: March 9, 2026
-- ==========================================

USE TOURDB;
GO

IF OBJECT_ID('dbo.fn_CalculateTourCostPerPerson', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateTourCostPerPerson;
GO
CREATE FUNCTION dbo.fn_CalculateTourCostPerPerson (@p_TourID BIGINT, @p_ParticipantCount INT)
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @CostPerPerson DECIMAL(19,0) = 0, @TotalCost DECIMAL(19,0);
    BEGIN TRY
        SELECT @TotalCost = TOUR_PACKAGE_COST FROM dbo.TOUR_PACKAGE WHERE TOUR_ID = @p_TourID;
        IF @ParticipantCount > 0
            SET @CostPerPerson = CAST(@TotalCost / @p_ParticipantCount AS DECIMAL(19,0));
    END TRY BEGIN CATCH SET @CostPerPerson = 0; END CATCH
    RETURN @CostPerPerson;
END;
GO

IF OBJECT_ID('dbo.usp_PlanTourPackage', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_PlanTourPackage;
GO
-- Procedure: usp_PlanTourPackage - Plan a new tour package
CREATE PROCEDURE dbo.usp_PlanTourPackage
    @p_TourName VARCHAR(200), 
    @p_Destination VARCHAR(100), 
    @p_StartDate DATE, 
    @p_EndDate DATE,
    @p_totalCost DECIMAL(19,0), 
    @p_MaxParticipants INT, 
    @p_PlannerID BIGINT, 
    @p_TourID BIGINT OUTPUT
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
-- Procedure: usp_RegisterTourParticipant - Register a participant for a tour
CREATE PROCEDURE dbo.usp_RegisterTourParticipant
    @p_TourID BIGINT, 
    @p_ParticipantID BIGINT, 
    @p_RegistrationDate DATE, 
    @p_RegisteredBy BIGINT, 
    @p_RegistrationID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @CurrentCount INT;
        SELECT @CurrentCount = COUNT(*) FROM dbo.TOUR_REGISTRATION WHERE TOUR_ID = @p_TourID AND REGISTRATION_STATUS = 'A';
        DECLARE @MaxCount INT;
        SELECT @MaxCount = MAX_PARTICIPANTS FROM dbo.TOUR_PACKAGE WHERE TOUR_ID = @p_TourID;
        IF @CurrentCount >= @MaxCount
            THROW 50001, 'Tour is fully booked', 1;
        INSERT INTO dbo.TOUR_REGISTRATION (TOUR_ID, PARTICIPANT_ID, REGISTRATION_DATE, REGISTRATION_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_TourID, @p_ParticipantID, @p_RegistrationDate, 'A', @p_RegisteredBy, GETDATE());
        SET @p_RegistrationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'TOUR Module - Procedures created successfully.';
GO
