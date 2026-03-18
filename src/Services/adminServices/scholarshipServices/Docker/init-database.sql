-- =====================================================
-- Docker init script for ADMINDB (Scholarship Service)
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- =====================================================
-- 1. Create database
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ADMINDB')
BEGIN
    CREATE DATABASE [ADMINDB];
    PRINT '+ ADMINDB created';
END
ELSE
    PRINT '+ ADMINDB already exists';
GO

USE [ADMINDB];
GO

-- =====================================================
-- 2. Scholarship Tables
-- =====================================================

-- SCHOLARSHIP_AMOUNT
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_AMOUNT]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_AMOUNT] (
        [SCH_AMTID]              BIGINT        NOT NULL,
        [SCH_ORGID]              BIGINT        NOT NULL,
        [SCH_GRADECAT]           CHAR(3)       NOT NULL,
        [SCH_ELGIBLEEXAM]        VARCHAR(2)    NOT NULL,
        [SCH_APPLICABLEALLGRADE] CHAR(1)       NOT NULL,
        [SCH_GRADEID]            DECIMAL(38,0) NOT NULL,
        [SCH_FROMYEAR]           DECIMAL(38,0) NOT NULL,
        [SCH_CLOSEYEAR]          DECIMAL(38,0) NULL,
        [SCH_ELGIBLEAMOUNT]      BIGINT        NOT NULL,
        [SCH_ELGIBLEYEAR]        INT           NOT NULL,
        [SCH_CUTOFFMARKS]        INT           NOT NULL,
        [SCH_CREATEDON]          BIGINT        NULL,
        [SCH_CREATEDBY]          DATETIME2(3)  NULL,
        [SCH_UPDATEDON]          DATETIME2(3)  NULL,
        [SCH_UPDATEDBY]          BIGINT        NULL,
        CONSTRAINT [PK_SCHOLARSHIP_AMOUNT] PRIMARY KEY ([SCH_AMTID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_AMOUNT_GRADECAT] ON [dbo].[SCHOLARSHIP_AMOUNT]([SCH_GRADECAT], [SCH_ELGIBLEEXAM]);
    PRINT '+ Table SCHOLARSHIP_AMOUNT created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_AMOUNT already exists';
GO

-- SCHOLARSHIP_MAIN must be created before SCHOLARSHIP_DETAIL (FK dependency)
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_MAIN]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_MAIN] (
        [SCH_ID]               INT           NOT NULL,
        [SCH_EMPSYSID]         INT           NOT NULL,
        [SCH_GRADEID]          INT           NOT NULL,
        [SCH_DEPENDID]         INT           NOT NULL,
        [SCH_CHILDNAME]        VARCHAR(100)  NOT NULL,
        [SCH_LASTSCHOOL]       VARCHAR(100)  NOT NULL,
        [SCH_LASTYEAROFSCHOOL] DECIMAL(38,0) NOT NULL,
        [SCH_LASTEXAM]         CHAR(2)       NOT NULL,
        [SCH_CGPAFLAG]         CHAR(1)       NOT NULL,
        [SCH_MARKSPER]         DECIMAL(19,0) NOT NULL,
        [SCH_MARKSGPA]         DECIMAL(19,0) NOT NULL,
        [SCH_MARKSFILE]        VARCHAR(100)  NOT NULL,
        [SCH_COURSENAME]       VARCHAR(100)  NOT NULL,
        [SCH_COURSEJOINYEAR]   INT           NOT NULL,
        [SCH_COURSEJOINMONTH]  DECIMAL(20,0) NOT NULL,
        [SCH_COURSEDURATION]   BIGINT        NOT NULL,
        [SCH_ADMRECPTFILE]     VARCHAR(100)  NULL,
        [SCH_PAYMODE]          CHAR(3)       NULL,
        [SCH_CHILDACCNO]       VARCHAR(20)   NULL,
        [SCH_CHILLDBANKIFSC]   VARCHAR(12)   NULL,
        [SCH_CHILLDBANKMICR]   VARCHAR(12)   NULL,
        [SCH_ENTRYSTATUS]      CHAR(1)       NULL,
        [SCH_SOURCE]           CHAR(1)       NOT NULL,
        [SCH_DISBAMOUNT]       DECIMAL(19,0) NOT NULL,
        [SCH_DISBFREQ]         CHAR(1)       NOT NULL,
        [SCH_LIVESTATUS]       CHAR(1)       NOT NULL,
        [SCH_CREATEDON]        DATETIME2(3)  NOT NULL,
        [SCH_CREATEDBY]        INT           NOT NULL,
        [SCH_UPDATEDON]        DATETIME2(3)  NOT NULL,
        [SCH_UPDATEDBY]        BIGINT        NOT NULL,
        [SCH_APPROVALBY]       INT           NOT NULL,
        [SCH_APPROVALON]       DATETIME2(3)  NOT NULL,
        [SCH_APPREMARKS]       VARCHAR(200)  NOT NULL,
        [SCH_STOPREASON]       VARCHAR(200)  NOT NULL,
        [SCH_STOPDATE]         DATETIME2(3)  NOT NULL,
        [SCH_STOPENTEREDON]    DATETIME2(3)  NOT NULL,
        [SCH_STOPENTEREDBY]    INT           NOT NULL,
        [SCH_OFFLINE]          CHAR(1)       NOT NULL,
        [SCH_OFFLINEYEAR]      INT           NULL,
        CONSTRAINT [PK_SCHOLARSHIP_MAIN] PRIMARY KEY ([SCH_ID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_MAIN_EMPSYSID] ON [dbo].[SCHOLARSHIP_MAIN]([SCH_EMPSYSID]);
    PRINT '+ Table SCHOLARSHIP_MAIN created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_MAIN already exists';
GO

-- SCHOLARSHIP_DETAIL
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_DETAIL]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_DETAIL] (
        [SCHDET_ID]            BIGINT       NOT NULL,
        [SCHDET_MAINID]        BIGINT       NOT NULL,
        [SCHDET_YEAR]          INT          NOT NULL,
        [SCHDET_MARKSFILE]     VARCHAR(100) NOT NULL,
        [SCHDET_MARKSTATUS]    CHAR(1)      NOT NULL,
        [SCHDET_PAYSTATUS]     CHAR(1)      NOT NULL,
        [SCHDET_CREATEDON]     DATETIME2(3) NOT NULL,
        [SCHDET_CREATEDBY]     BIGINT       NOT NULL,
        [SCHDET_UPDATEDON]     DATETIME2(3) NULL,
        [SCHDET_UPDATEDBY]     BIGINT       NULL,
        [SCHDET_APPROVEDON]    DATETIME2(3) NULL,
        [SCHDET_APPROVEDBY]    BIGINT       NULL,
        [SCHDET_PAYAPPROVEDON] DATETIME2(3) NULL,
        [SCHDET_PAYAPPROVEDBY] BIGINT       NULL,
        [SCHDET_PAYDATE]       DATETIME2(3) NULL,
        [SCHDET_PAYAMOUNT]     BIGINT       NULL,
        [SCHDET_PAYUPDATEDON]  DATETIME2(3) NULL,
        [SCHDET_PAYUPDATEDBY]  BIGINT       NULL,
        CONSTRAINT [PK_SCHOLARSHIP_DETAIL]      PRIMARY KEY ([SCHDET_ID]),
        CONSTRAINT [FK_SCHOLARSHIP_DETAIL_MAIN] FOREIGN KEY ([SCHDET_MAINID])
            REFERENCES [dbo].[SCHOLARSHIP_MAIN]([SCH_ID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_DETAIL_MAINID] ON [dbo].[SCHOLARSHIP_DETAIL]([SCHDET_MAINID]);
    PRINT '+ Table SCHOLARSHIP_DETAIL created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_DETAIL already exists';
GO

-- =====================================================
-- 3. Functions
-- =====================================================

CREATE OR ALTER FUNCTION dbo.fn_GetScholarshipEligibleAmount
(
    @p_GradeCat     CHAR(3),
    @p_EligibleExam VARCHAR(2),
    @p_Year         INT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @Amount BIGINT;
    SELECT TOP 1 @Amount = SCH_ELGIBLEAMOUNT
    FROM dbo.SCHOLARSHIP_AMOUNT
    WHERE SCH_GRADECAT    = @p_GradeCat
      AND SCH_ELGIBLEEXAM = @p_EligibleExam
      AND @p_Year BETWEEN SCH_FROMYEAR AND ISNULL(SCH_CLOSEYEAR, @p_Year)
    ORDER BY SCH_FROMYEAR DESC;
    RETURN ISNULL(@Amount, 0);
END;
GO
PRINT '+ fn_GetScholarshipEligibleAmount created';
GO

-- =====================================================
-- 4. Stored Procedures
-- =====================================================

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApplication
(
    @p_SCH_EMPSYSID         INT,
    @p_SCH_GRADEID          INT,
    @p_SCH_DEPENDID         INT,
    @p_SCH_CHILDNAME        VARCHAR(100),
    @p_SCH_LASTSCHOOL       VARCHAR(100),
    @p_SCH_LASTYEAROFSCHOOL DECIMAL(38,0),
    @p_SCH_LASTEXAM         CHAR(2),
    @p_SCH_CGPAFLAG         CHAR(1),
    @p_SCH_MARKSPER         DECIMAL(19,0),
    @p_SCH_MARKSGPA         DECIMAL(19,0),
    @p_SCH_MARKSFILE        VARCHAR(100),
    @p_SCH_COURSENAME       VARCHAR(100),
    @p_SCH_COURSEJOINYEAR   INT,
    @p_SCH_COURSEJOINMONTH  DECIMAL(20,0),
    @p_SCH_COURSEDURATION   BIGINT,
    @p_SCH_ADMRECPTFILE     VARCHAR(100) = NULL,
    @p_SCH_PAYMODE          CHAR(3)      = NULL,
    @p_SCH_CHILDACCNO       VARCHAR(20)  = NULL,
    @p_SCH_CHILLDBANKIFSC   VARCHAR(12)  = NULL,
    @p_SCH_CHILLDBANKMICR   VARCHAR(12)  = NULL,
    @p_SCH_ENTRYSTATUS      CHAR(1)      = 'E',
    @p_SCH_SOURCE           CHAR(1),
    @p_SCH_DISBAMOUNT       DECIMAL(19,0),
    @p_SCH_DISBFREQ         CHAR(1),
    @p_SCH_LIVESTATUS       CHAR(1)      = 'A',
    @p_CreatedBy            INT,
    @p_SCH_OFFLINE          CHAR(1)      = 'N',
    @p_SCH_OFFLINEYEAR      INT          = NULL,
    @p_NewSchID             INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @p_NewSchID = ISNULL(MAX(SCH_ID), 0) + 1 FROM dbo.SCHOLARSHIP_MAIN;

        INSERT INTO dbo.SCHOLARSHIP_MAIN
        (
            SCH_ID, SCH_EMPSYSID, SCH_GRADEID, SCH_DEPENDID, SCH_CHILDNAME,
            SCH_LASTSCHOOL, SCH_LASTYEAROFSCHOOL, SCH_LASTEXAM, SCH_CGPAFLAG,
            SCH_MARKSPER, SCH_MARKSGPA, SCH_MARKSFILE, SCH_COURSENAME,
            SCH_COURSEJOINYEAR, SCH_COURSEJOINMONTH, SCH_COURSEDURATION,
            SCH_ADMRECPTFILE, SCH_PAYMODE, SCH_CHILDACCNO, SCH_CHILLDBANKIFSC,
            SCH_CHILLDBANKMICR, SCH_ENTRYSTATUS, SCH_SOURCE, SCH_DISBAMOUNT,
            SCH_DISBFREQ, SCH_LIVESTATUS, SCH_CREATEDON, SCH_CREATEDBY,
            SCH_UPDATEDON, SCH_UPDATEDBY, SCH_APPROVALBY, SCH_APPROVALON,
            SCH_APPREMARKS, SCH_STOPREASON, SCH_STOPDATE, SCH_STOPENTEREDON,
            SCH_STOPENTEREDBY, SCH_OFFLINE, SCH_OFFLINEYEAR
        )
        VALUES
        (
            @p_NewSchID, @p_SCH_EMPSYSID, @p_SCH_GRADEID, @p_SCH_DEPENDID, @p_SCH_CHILDNAME,
            @p_SCH_LASTSCHOOL, @p_SCH_LASTYEAROFSCHOOL, @p_SCH_LASTEXAM, @p_SCH_CGPAFLAG,
            @p_SCH_MARKSPER, @p_SCH_MARKSGPA, @p_SCH_MARKSFILE, @p_SCH_COURSENAME,
            @p_SCH_COURSEJOINYEAR, @p_SCH_COURSEJOINMONTH, @p_SCH_COURSEDURATION,
            @p_SCH_ADMRECPTFILE, @p_SCH_PAYMODE, @p_SCH_CHILDACCNO, @p_SCH_CHILLDBANKIFSC,
            @p_SCH_CHILLDBANKMICR, @p_SCH_ENTRYSTATUS, @p_SCH_SOURCE, @p_SCH_DISBAMOUNT,
            @p_SCH_DISBFREQ, @p_SCH_LIVESTATUS, GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy, 0, GETDATE(),
            '', '', GETDATE(), GETDATE(), 0,
            @p_SCH_OFFLINE, @p_SCH_OFFLINEYEAR
        );

        DECLARE @NewDetID BIGINT;
        SELECT @NewDetID = ISNULL(MAX(SCHDET_ID), 0) + 1 FROM dbo.SCHOLARSHIP_DETAIL;

        INSERT INTO dbo.SCHOLARSHIP_DETAIL
        (
            SCHDET_ID, SCHDET_MAINID, SCHDET_YEAR, SCHDET_MARKSFILE,
            SCHDET_MARKSTATUS, SCHDET_PAYSTATUS, SCHDET_CREATEDON, SCHDET_CREATEDBY,
            SCHDET_UPDATEDON, SCHDET_UPDATEDBY, SCHDET_APPROVEDON, SCHDET_APPROVEDBY,
            SCHDET_PAYAPPROVEDON, SCHDET_PAYAPPROVEDBY, SCHDET_PAYDATE,
            SCHDET_PAYAMOUNT, SCHDET_PAYUPDATEDON, SCHDET_PAYUPDATEDBY
        )
        VALUES
        (
            @NewDetID, @p_NewSchID, @p_SCH_COURSEJOINYEAR, @p_SCH_MARKSFILE,
            'P', 'S',
            GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy,
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipApplication created';
GO

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApprove
(
    @p_SCH_ID     INT,
    @p_ApprovedBy INT,
    @p_AppRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.SCHOLARSHIP_MAIN
        SET SCH_ENTRYSTATUS = 'A',
            SCH_APPROVALBY  = @p_ApprovedBy,
            SCH_APPROVALON  = GETDATE(),
            SCH_APPREMARKS  = ISNULL(@p_AppRemarks, ''),
            SCH_UPDATEDON   = GETDATE(),
            SCH_UPDATEDBY   = @p_ApprovedBy
        WHERE SCH_ID = @p_SCH_ID;

        UPDATE dbo.SCHOLARSHIP_DETAIL
        SET SCHDET_MARKSTATUS = 'A',
            SCHDET_APPROVEDON = GETDATE(),
            SCHDET_APPROVEDBY = @p_ApprovedBy,
            SCHDET_UPDATEDON  = GETDATE(),
            SCHDET_UPDATEDBY  = @p_ApprovedBy
        WHERE SCHDET_MAINID    = @p_SCH_ID
          AND SCHDET_MARKSTATUS = 'P';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipApprove created';
GO

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipStop
(
    @p_SCH_ID      INT,
    @p_StopReason  VARCHAR(200),
    @p_StopDate    DATETIME2(3),
    @p_EnteredBy   INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.SCHOLARSHIP_MAIN
        SET SCH_LIVESTATUS    = 'S',
            SCH_STOPREASON    = @p_StopReason,
            SCH_STOPDATE      = @p_StopDate,
            SCH_STOPENTEREDON = GETDATE(),
            SCH_STOPENTEREDBY = @p_EnteredBy,
            SCH_UPDATEDON     = GETDATE(),
            SCH_UPDATEDBY     = @p_EnteredBy
        WHERE SCH_ID = @p_SCH_ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipStop created';
GO

-- =====================================================
-- 5. Triggers
-- =====================================================

CREATE OR ALTER TRIGGER dbo.trg_ScholarshipDetail_UpdateAudit
ON dbo.SCHOLARSHIP_DETAIL
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE SD
    SET SCHDET_UPDATEDON = GETDATE()
    FROM dbo.SCHOLARSHIP_DETAIL SD
    INNER JOIN inserted I ON SD.SCHDET_ID = I.SCHDET_ID;
END;
GO
PRINT '+ trg_ScholarshipDetail_UpdateAudit created';
GO

-- =====================================================
-- 6. EF Core Migration History
-- =====================================================

IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '+ Table __EFMigrationsHistory created';
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260310000000_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260310000000_InitialCreate', '10.0.3');
    PRINT '+ EF Core migration history seeded';
END
GO

-- Verify table creation
SELECT 'SCHOLARSHIP_AMOUNT' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_AMOUNT]
UNION ALL
SELECT 'SCHOLARSHIP_MAIN'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_MAIN]
UNION ALL
SELECT 'SCHOLARSHIP_DETAIL' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_DETAIL];
GO

PRINT '';
PRINT '======================================';
PRINT 'ADMINDB initialization complete';
PRINT '======================================';
GO

-- =====================================================
-- END OF init-database.sql
-- =====================================================
