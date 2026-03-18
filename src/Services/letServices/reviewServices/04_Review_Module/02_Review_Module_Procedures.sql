-- ==========================================
-- REVIEW MODULE - Stored Procedures
-- Database: LETDB
-- Purpose: Review & Feedback procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_Review_SubmitFeedback
-- Purpose: Submit course feedback
IF OBJECT_ID('dbo.usp_Review_SubmitFeedback', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Review_SubmitFeedback;
GO
CREATE PROCEDURE dbo.usp_Review_SubmitFeedback
    @p_CourseID BIGINT,
    @p_UserID VARCHAR(255),
    @p_ReviewDate DATETIME2(3),
    @p_GeneralRemarks VARCHAR(255),
    @p_RequestNum BIGINT,
    @p_OverallRating BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.COURSE_FEEDMAIN (
            FD_CRS_ID,
            FD_USR_ID,
            FD_REV_DAT,
            FD_GEN_REM,
            FD_REQ_NUM,
            FD_MOD_DAT
        )
        VALUES (
            @p_CourseID,
            @p_UserID,
            @p_ReviewDate,
            @p_GeneralRemarks,
            @p_RequestNum,
            GETDATE()
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Review_SubmitCourseReview
-- Purpose: Submit course review
IF OBJECT_ID('dbo.usp_Review_SubmitCourseReview', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Review_SubmitCourseReview;
GO
CREATE PROCEDURE dbo.usp_Review_SubmitCourseReview
    @p_ReviewSrlNum BIGINT,
    @p_FeedbackNum BIGINT,
    @p_Status CHAR(1),
    @p_ReviewDate DATETIME2(3),
    @p_Remarks1 VARCHAR(4000),
    @p_Remarks2 VARCHAR(4000)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.REVIEW_MAIN (
            REV_SRL_NUM,
            REV_FED_NUM,
            REV_REM_MRK1,
            REV_REM_MRK2,
            REV_STATUS,
            REV_ENT_DATE,
            REV_NEXT_DATE
        )
        VALUES (
            @p_ReviewSrlNum,
            @p_FeedbackNum,
            @p_Remarks1,
            @p_Remarks2,
            @p_Status,
            CAST(GETDATE() AS VARCHAR),
            @p_ReviewDate
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Review_GetCourseReviews
-- Purpose: Retrieve course reviews
IF OBJECT_ID('dbo.usp_Review_GetCourseReviews', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Review_GetCourseReviews;
GO
CREATE PROCEDURE dbo.usp_Review_GetCourseReviews
    @p_CourseID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        FD_CRS_ID,
        FD_USR_ID,
        FD_REV_DAT,
        FD_GEN_REM,
        FD_REQ_NUM,
        FD_MOD_DAT
    FROM dbo.COURSE_FEEDMAIN
    WHERE FD_CRS_ID = @p_CourseID
    ORDER BY FD_REV_DAT DESC;
END;
GO

-- Procedure: usp_Review_AddReviewDetail
-- Purpose: Add review details/sub-records
IF OBJECT_ID('dbo.usp_Review_AddReviewDetail', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Review_AddReviewDetail;
GO
CREATE PROCEDURE dbo.usp_Review_AddReviewDetail
    @p_ReviewMainSrl BIGINT,
    @p_ReviewNum BIGINT,
    @p_ReviewDate DATETIME2(3),
    @p_ReviewedBy BIGINT,
    @p_ReviewStatus VARCHAR(10),
    @p_Remarks VARCHAR(4000)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.REVIEW_SUB (
            REV_MAIN_SRL,
            REV_REV_NUM,
            REV_DATE,
            REV_BY,
            REV_STATUS,
            REV_REM_MRK
        )
        VALUES (
            @p_ReviewMainSrl,
            @p_ReviewNum,
            @p_ReviewDate,
            @p_ReviewedBy,
            @p_ReviewStatus,
            @p_Remarks
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Review_GetFeedbackSummary
-- Purpose: Get feedback summary for a course
IF OBJECT_ID('dbo.usp_Review_GetFeedbackSummary', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Review_GetFeedbackSummary;
GO
CREATE PROCEDURE dbo.usp_Review_GetFeedbackSummary
    @p_CourseID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalFeedbacks,
        AVG(CAST(FD_FIN_RAT AS DECIMAL(5,2))) AS AverageRating
    FROM dbo.COURSE_FEEDBACKMAIN
    WHERE FD_REQ_NUM IN (
        SELECT FD_REQ_NUM FROM dbo.COURSE_FEEDMAIN 
        WHERE FD_CRS_ID = @p_CourseID
    );
END;
GO

PRINT 'Review Module Procedures created successfully.';
GO
