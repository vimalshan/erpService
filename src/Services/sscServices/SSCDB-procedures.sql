-- SSCDB Stored Procedures, Functions & Triggers
-- Staff & Student Club Membership Management System
-- Created: February 13, 2026

USE SSCDB;
GO

IF OBJECT_ID('dbo.fn_GetActiveClubCount', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetActiveClubCount;
GO
CREATE FUNCTION dbo.fn_GetActiveClubCount ()
RETURNS INT
AS BEGIN
    DECLARE @Count INT = 0;
    BEGIN TRY
        SELECT @Count = COUNT(*) FROM dbo.CLUB_MASTER WHERE CLUB_STATUS = 'A';
    END TRY BEGIN CATCH SET @Count = 0; END CATCH
    RETURN @Count;
END;
GO

IF OBJECT_ID('dbo.usp_CreateClubMembership', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateClubMembership;
GO
CREATE PROCEDURE dbo.usp_CreateClubMembership
    @p_ClubID BIGINT, @p_MemberID BIGINT, @p_JoinDate DATE, 
    @p_MembershipFee DECIMAL(19,0), @p_EnrolledBy BIGINT, @p_MembershipID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM dbo.CLUB_MEMBERSHIP WHERE CLUB_ID = @p_ClubID AND MEMBER_ID = @p_MemberID AND MEMBERSHIP_STATUS = 'A')
            THROW 50001, 'Member already exists in this club', 1;
        INSERT INTO dbo.CLUB_MEMBERSHIP (CLUB_ID, MEMBER_ID, JOIN_DATE, MEMBERSHIP_FEE, MEMBERSHIP_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ClubID, @p_MemberID, @p_JoinDate, @p_MembershipFee, 'A', @p_EnrolledBy, GETDATE());
        SET @p_MembershipID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RecordClubActivity', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordClubActivity;
GO
CREATE PROCEDURE dbo.usp_RecordClubActivity
    @p_ClubID BIGINT, @p_ActivityName VARCHAR(100), @p_ActivityDate DATE,
    @p_Budget DECIMAL(19,0), @p_OrganizerID BIGINT, @p_ActivityID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.CLUB_ACTIVITY (CLUB_ID, ACTIVITY_NAME, ACTIVITY_DATE, ACTIVITY_BUDGET, 
            ORGANIZER_ID, ACTIVITY_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ClubID, @p_ActivityName, @p_ActivityDate, @p_Budget, @p_OrganizerID, 'P', @p_OrganizerID, GETDATE());
        SET @p_ActivityID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'SSCDB Procedures created successfully.';
GO
