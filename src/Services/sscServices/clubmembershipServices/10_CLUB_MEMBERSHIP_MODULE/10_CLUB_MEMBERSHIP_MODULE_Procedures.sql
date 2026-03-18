-- ==========================================
-- Module: CLUB MEMBERSHIP MODULE - Stored Procedures
-- Description: Stored procedures for club membership management
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- Function: fn_GetActiveClubCount
-- Description: Get count of active clubs
-- Returns: INT - Total count of active clubs
-- ==========================================
IF OBJECT_ID('dbo.fn_GetActiveClubCount', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetActiveClubCount;
GO
CREATE FUNCTION dbo.fn_GetActiveClubCount ()
RETURNS INT
AS BEGIN
    DECLARE @Count INT = 0;
    BEGIN TRY
        SELECT @Count = COUNT(*) FROM dbo.CLUB_MASTER WHERE CLUB_STATUS = 'A';
    END TRY BEGIN CATCH 
        SET @Count = 0; 
    END CATCH
    RETURN @Count;
END;
GO

-- ==========================================
-- Procedure: usp_CreateClubMembership
-- Description: Create a new club membership
-- Parameters:
--   @p_ClubID - Club ID
--   @p_MemberID - Member ID
--   @p_JoinDate - Date of joining
--   @p_MembershipFee - Membership fee amount
--   @p_EnrolledBy - User ID who enrolled the member
--   @p_MembershipID - Output: Generated membership ID
-- ==========================================
IF OBJECT_ID('dbo.usp_CreateClubMembership', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateClubMembership;
GO
CREATE PROCEDURE dbo.usp_CreateClubMembership
    @p_ClubID BIGINT, 
    @p_MemberID BIGINT, 
    @p_JoinDate DATE, 
    @p_MembershipFee DECIMAL(19,2), 
    @p_EnrolledBy BIGINT, 
    @p_MembershipID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if member already exists in this club
        IF EXISTS (SELECT 1 FROM dbo.CLUB_MEMBERSHIP 
                   WHERE CLUB_ID = @p_ClubID AND MEMBER_ID = @p_MemberID AND MEMBERSHIP_STATUS = 'A')
        BEGIN
            THROW 50001, 'Member already exists in this club', 1;
        END
        
        -- Insert new membership record
        INSERT INTO dbo.CLUB_MEMBERSHIP 
        (CLUB_ID, MEMBER_ID, JOIN_DATE, MEMBERSHIP_FEE, MEMBERSHIP_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ClubID, @p_MemberID, @p_JoinDate, @p_MembershipFee, 'A', @p_EnrolledBy, GETDATE());
        
        SET @p_MembershipID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

-- ==========================================
-- Procedure: usp_RecordClubActivity
-- Description: Record a new club activity
-- Parameters:
--   @p_ClubID - Club ID
--   @p_ActivityName - Name of the activity
--   @p_ActivityDate - Date of activity
--   @p_Budget - Activity budget
--   @p_OrganizerID - Organizer user ID
--   @p_ActivityID - Output: Generated activity ID
-- ==========================================
IF OBJECT_ID('dbo.usp_RecordClubActivity', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordClubActivity;
GO
CREATE PROCEDURE dbo.usp_RecordClubActivity
    @p_ClubID BIGINT, 
    @p_ActivityName VARCHAR(100), 
    @p_ActivityDate DATE,
    @p_Budget DECIMAL(19,2), 
    @p_OrganizerID BIGINT, 
    @p_ActivityID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert new activity record
        INSERT INTO dbo.CLUB_ACTIVITY 
        (CLUB_ID, ACTIVITY_NAME, ACTIVITY_DATE, ACTIVITY_BUDGET, ORGANIZER_ID, ACTIVITY_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ClubID, @p_ActivityName, @p_ActivityDate, @p_Budget, @p_OrganizerID, 'P', @p_OrganizerID, GETDATE());
        
        SET @p_ActivityID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW; 
    END CATCH
END;
GO

PRINT 'CLUB_MEMBERSHIP_MODULE Procedures created successfully.';
GO
