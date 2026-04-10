-- ============================================================================
-- SPARSH Microservices - Complete Database Initialization Script
-- Run against SQL Server to create all databases, tables, sequences, and procs
-- ============================================================================

-- ============================================================================
-- 1. CREATE DATABASES
-- ============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SPARSHDB')
    CREATE DATABASE [SPARSHDB];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProblemManagementDb')
    CREATE DATABASE [ProblemManagementDb];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SparshTransactionalDb')
    CREATE DATABASE [SparshTransactionalDb];
GO

PRINT '=== Databases created ===';
GO

-- ============================================================================
-- 2. SPARSHDB - Shared Tables (EmployeePride, MobileApp, MobileExpense)
-- ============================================================================
USE [SPARSHDB];
GO

-- Mobile App Management Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOB_APPDEVICE_DETAILS')
CREATE TABLE [MOB_APPDEVICE_DETAILS] (
    [MD_EMPSYSID] DECIMAL(38) NOT NULL,
    [MD_DEVICEID] VARCHAR(200) NULL,
    [MD_ACTIVE] CHAR(1) NOT NULL,
    [MD_DEVICETYPE] CHAR(1) NULL,
    [MD_IMEINO] VARCHAR(200) NULL,
    [MD_CREATEDON] DATETIME2(3) NOT NULL,
    [MD_UPDATEDBY] DECIMAL(38) NOT NULL,
    [MD_UPDATEDON] DATETIME2(3) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOB_LOGINDET')
CREATE TABLE [MOB_LOGINDET] (
    [LD_LOGINID] DECIMAL(38) NOT NULL,
    [LD_USERSYSID] DECIMAL(38) NOT NULL,
    [LD_DEVICEID] VARCHAR(200) NULL,
    [LD_LOGON] DATETIME2(3) NOT NULL,
    [LD_GUID] VARCHAR(255) NOT NULL,
    [LD_IMEINO] VARCHAR(200) NULL,
    [LD_DEVICETYPE] CHAR(1) NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOBAPP_REGISTER')
CREATE TABLE [MOBAPP_REGISTER] (
    [REGISTER_ID] BIGINT NOT NULL,
    [REGISTER_EMPSYSID] BIGINT NULL,
    [REGISTER_USERID] VARCHAR(255) NULL,
    [REGISTER_USERSYSID] BIGINT NULL,
    [REGISTER_USERTYPE] CHAR(1) NULL,
    [REGISTER_PINNO] BIGINT NULL,
    [REGISTER_PINGENERATEDON] DATETIME2(3) NULL,
    [REGISTER_UPDATEDON] DATETIME2(3) NULL,
    [REGISTER_STATUS] CHAR(1) NULL,
    [REGISTER_MOBILENO] VARCHAR(255) NULL,
    [REGISTER_IMEINO] VARCHAR(255) NULL,
    [REGISTER_GUID] CHAR(1) NULL,
    [REGISTER_DEVICEID] VARCHAR(255) NULL,
    [REGISTER_DTYPE] CHAR(1) NULL,
    CONSTRAINT [PK_MOBAPP_REGISTER] PRIMARY KEY ([REGISTER_ID])
);
GO

-- Mobile Expense Management Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOBEXP_DET')
CREATE TABLE [MOBEXP_DET] (
    [MOBEXP_ID] DECIMAL(38) NOT NULL,
    [MOBEXP_TPID] DECIMAL(38) NOT NULL,
    [MOBEXP_CATID] DECIMAL(38) NOT NULL,
    [MOBEXP_DATE] DATETIME2(3) NULL,
    [MOBEXP_COMMENT] VARCHAR(500) NOT NULL,
    [MOBEXP_AMOUNT] DECIMAL(38) NULL,
    [MOBEXP_CURRID] DECIMAL(38) NULL,
    [MOBEXP_ENTEREDBY] DECIMAL(38) NOT NULL,
    [MOBEXP_ENTEREDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_MOBEXP_DET] PRIMARY KEY ([MOBEXP_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOBEXP_FILE')
CREATE TABLE [MOBEXP_FILE] (
    [MOBEXPPHT_ID] DECIMAL(38) NOT NULL,
    [MOBEXPPHT_EXPID] DECIMAL(38) NOT NULL,
    [MOBEXPPHT_FILENAME] VARCHAR(500) NOT NULL,
    [MOBEXPPHT_FILEDATA] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_MOBEXP_FILE] PRIMARY KEY ([MOBEXPPHT_ID])
);
GO

-- Employee Pride Management Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MOMENT_PRIDE')
CREATE TABLE [MOMENT_PRIDE] (
    [MOMENTPRIDE_ID] DECIMAL(38) NOT NULL,
    [MOMENTPRIDE_TITLE] VARCHAR(50) NOT NULL,
    [MOMENTPRIDE_BODY] NVARCHAR(MAX) NULL,
    [MOMENTPRIDE_EMPSYSID] DECIMAL(38) NOT NULL,
    [MOMENTPRIDE_FOOTER] VARCHAR(500) NOT NULL,
    [MOMENTPRIDE_LOCATION] VARCHAR(100) NOT NULL,
    [MOMENTPRIDE_IMAGE] VARCHAR(200) NOT NULL,
    [MOMENTPRIDE_MODIFIEDBY] BIGINT NOT NULL,
    [MOMENTPRIDE_MODIFIEDON] DATETIME2(3) NULL
);
GO

-- ============================================================================
-- 3. SPARSHDB - Sequences
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOB_LoginId')
    CREATE SEQUENCE dbo.seq_MOB_LoginId AS DECIMAL(38) START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_Id')
    CREATE SEQUENCE dbo.seq_MOBEXP_Id AS DECIMAL(38) START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_File_Id')
    CREATE SEQUENCE dbo.seq_MOBEXP_File_Id AS DECIMAL(38) START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOMENT_PRIDE_Id')
    CREATE SEQUENCE dbo.seq_MOMENT_PRIDE_Id AS DECIMAL(38) START WITH 1 INCREMENT BY 1 CACHE 100;
GO

PRINT '=== SPARSHDB tables and sequences created ===';
GO

-- ============================================================================
-- 4. ProblemManagementDb - Tables
-- ============================================================================
USE [ProblemManagementDb];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_FUNCTION')
CREATE TABLE [PROBLEM_FUNCTION] (
    [FUNCID] BIGINT NOT NULL,
    [FUNCNAME] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_PROBLEM_FUNCTION] PRIMARY KEY ([FUNCID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_IMPACT')
CREATE TABLE [PROBLEM_IMPACT] (
    [IMPACT_ID] BIGINT NOT NULL,
    [IMPACT_DESC] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_PROBLEM_IMPACT] PRIMARY KEY ([IMPACT_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_MAIN')
CREATE TABLE [PROBLEM_MAIN] (
    [PR_ID] BIGINT NOT NULL,
    [PR_OWNER] BIGINT NOT NULL,
    [PR_ENTEREDBY] BIGINT NOT NULL,
    [PR_DESCRIPTION] VARCHAR(255) NOT NULL,
    [PR_RESPEXPBY] DATETIME2(3) NULL,
    [PR_CATEGORY] CHAR(1) NULL,
    [PR_SPECIALIZATION] BIGINT NULL,
    [PR_IMPACT] VARCHAR(255) NULL,
    [PR_EXPRESULT] VARCHAR(255) NULL,
    [PR_ENTEREDON] DATETIME2(3) NULL,
    [PR_STATUS] CHAR(1) NOT NULL,
    [PR_APPID] BIGINT NULL,
    [PR_STATEMENT] VARCHAR(255) NULL,
    [PR_TYPE] CHAR(1) NULL,
    [PR_ATTACH] VARCHAR(255) NULL,
    [PR_PRBFLAG] CHAR(1) NULL,
    [PR_PRBDESCRIPTION] VARCHAR(255) NULL,
    [PR_POSTFLAG] CHAR(1) NULL,
    [PR_QUESTION] VARCHAR(255) NULL,
    [PR_UNITID] BIGINT NOT NULL,
    [PR_SITEID] BIGINT NOT NULL,
    [PR_SOURCEID] BIGINT NULL,
    [PR_MODBY] BIGINT NOT NULL,
    [PR_MODON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_PROBLEM_MAIN] PRIMARY KEY ([PR_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_ATTACHMENT')
CREATE TABLE [PROBLEM_ATTACHMENT] (
    [PRAT_ID] BIGINT NOT NULL,
    [PRAT_PRID] BIGINT NULL,
    [PRAT_FILENAME] VARCHAR(2000) NULL,
    [PRAT_ENTEREDON] DATETIME2(3) NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_SOLUTION')
CREATE TABLE [PROBLEM_SOLUTION] (
    [SOL_ID] BIGINT NOT NULL,
    [SOL_PRID] BIGINT NOT NULL,
    [SOL_DESCRIPTION] VARCHAR(255) NULL,
    [SOL_IMPLEMENTATION] CHAR(1) NULL,
    [SOL_ENTEREDBY] BIGINT NOT NULL,
    [SOL_ENTEREDON] DATETIME2(3) NOT NULL,
    [SOL_ATTACH] VARCHAR(255) NULL,
    CONSTRAINT [PK_PROBLEM_SOLUTION] PRIMARY KEY ([SOL_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_APP')
CREATE TABLE [PROBLEM_APP] (
    [PRAPP_ID] BIGINT NOT NULL,
    [PRAPP_PRID] BIGINT NOT NULL,
    [PRAPP_BY] BIGINT NOT NULL,
    [PRAPP_ON] DATETIME2(3) NOT NULL,
    [PRAPP_STATUS] CHAR(1) NOT NULL,
    [PRAPP_REASON] VARCHAR(255) NULL,
    [PRAPP_AUDFLAG] CHAR(1) NOT NULL,
    CONSTRAINT [PK_PROBLEM_APP] PRIMARY KEY ([PRAPP_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PROBLEM_APPAUDIENCE')
CREATE TABLE [PROBLEM_APPAUDIENCE] (
    [PRAUD_ID] BIGINT NOT NULL,
    [PRAUD_PRID] BIGINT NOT NULL,
    [PRAUD_UNITID] INT NOT NULL,
    CONSTRAINT [PK_PROBLEM_APPAUDIENCE] PRIMARY KEY ([PRAUD_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SOLUTION_APP')
CREATE TABLE [SOLUTION_APP] (
    [SOLAPP_ID] BIGINT NOT NULL,
    [SOLAPP_SOLID] BIGINT NOT NULL,
    [SOLAPP_BY] BIGINT NOT NULL,
    [SOLAPP_ON] DATETIME2(3) NOT NULL,
    [SOLAPP_STATUS] CHAR(1) NOT NULL,
    [SOLAPP_REASON] VARCHAR(255) NULL,
    [SOLAPP_AUDFLAG] CHAR(1) NULL,
    CONSTRAINT [PK_SOLUTION_APP] PRIMARY KEY ([SOLAPP_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SOLUTION_COMMENT')
CREATE TABLE [SOLUTION_COMMENT] (
    [COM_ID] DECIMAL(38) NOT NULL,
    [COM_SOLID] DECIMAL(38) NOT NULL,
    [COM_DESCRIPTION] VARCHAR(500) NOT NULL,
    [COM_LOGID] DECIMAL(38) NOT NULL,
    [COM_LOGDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SOLUTION_COMMENT] PRIMARY KEY ([COM_ID])
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SOLUTION_LIKES')
CREATE TABLE [SOLUTION_LIKES] (
    [LIKE_ID] DECIMAL(38) NOT NULL,
    [LIKE_SOLID] DECIMAL(38) NOT NULL,
    [LIKE_LOGID] DECIMAL(38) NOT NULL,
    [LIKE_LOGON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SOLUTION_LIKES] PRIMARY KEY ([LIKE_ID])
);
GO

-- Problem Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_MAIN_Id')
    CREATE SEQUENCE dbo.seq_PROBLEM_MAIN_Id AS BIGINT START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_SOLUTION_Id')
    CREATE SEQUENCE dbo.seq_PROBLEM_SOLUTION_Id AS BIGINT START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_APP_Id')
    CREATE SEQUENCE dbo.seq_PROBLEM_APP_Id AS BIGINT START WITH 1 INCREMENT BY 1 CACHE 100;
GO

PRINT '=== ProblemManagementDb tables and sequences created ===';
GO

-- ============================================================================
-- 5. SparshTransactionalDb - Scholarship Tables (created by EF migrations)
-- ============================================================================
USE [SparshTransactionalDb];
GO

-- EF Core will handle schema creation via migrations.
-- Sequences for reference:
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_SCHOLARSHIP_APPLICATION_Id')
    CREATE SEQUENCE dbo.seq_SCHOLARSHIP_APPLICATION_Id AS BIGINT START WITH 1 INCREMENT BY 1 CACHE 100;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_SCHOLARSHIP_DISBURSEMENT_Id')
    CREATE SEQUENCE dbo.seq_SCHOLARSHIP_DISBURSEMENT_Id AS BIGINT START WITH 1 INCREMENT BY 1 CACHE 100;
GO

PRINT '=== SparshTransactionalDb sequences created ===';
GO

-- ============================================================================
-- 6. Stored Procedures - SPARSHDB
-- ============================================================================
USE [SPARSHDB];
GO

IF OBJECT_ID('dbo.fn_GetStudentEligibility', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetStudentEligibility;
GO
CREATE FUNCTION dbo.fn_GetStudentEligibility (@p_StudentID BIGINT, @p_SchemeID BIGINT)
RETURNS VARCHAR(50)
AS BEGIN
    DECLARE @Status VARCHAR(50) = 'INELIGIBLE';
    BEGIN TRY
        SELECT @Status = 'ELIGIBLE'
        WHERE EXISTS (
            SELECT 1 FROM dbo.SCHOLARSHIP_ELIGIBILITY_CRITERIA
            WHERE SCHOLARSHIP_ID = @p_SchemeID AND ELIGIBILITY_STATUS = 'A'
        );
    END TRY BEGIN CATCH SET @Status = 'ERROR'; END CATCH
    RETURN @Status;
END;
GO

IF OBJECT_ID('dbo.fn_CalculateScholarshipAmount', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateScholarshipAmount;
GO
CREATE FUNCTION dbo.fn_CalculateScholarshipAmount (@p_SchemeID BIGINT, @p_StudentAnnualFees DECIMAL(19,0))
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @Amount DECIMAL(19,0) = 0, @CoveragePercentage DECIMAL(5,2);
    BEGIN TRY
        SELECT @CoveragePercentage = SCHOLARSHIP_COVERAGE_PERCENT FROM dbo.SCHOLARSHIP_MASTER WHERE SCHOLARSHIP_ID = @p_SchemeID;
        SET @Amount = CAST(@p_StudentAnnualFees * (ISNULL(@CoveragePercentage, 100) / 100) AS DECIMAL(19,0));
    END TRY BEGIN CATCH SET @Amount = 0; END CATCH
    RETURN @Amount;
END;
GO

IF OBJECT_ID('dbo.usp_ApplyForScholarship', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ApplyForScholarship;
GO
CREATE PROCEDURE dbo.usp_ApplyForScholarship
    @p_StudentID BIGINT, @p_ScholarshipID BIGINT, @p_ApplicationDate DATE,
    @p_FamilyIncome DECIMAL(19,0), @p_ApplicantID BIGINT, @p_ApplicationID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF dbo.fn_GetStudentEligibility(@p_StudentID, @p_ScholarshipID) <> 'ELIGIBLE'
            THROW 50001, 'Student not eligible for scholarship', 1;
        INSERT INTO dbo.SCHOLARSHIP_APPLICATION (EMP_STUDENT_ID, SCHOLARSHIP_ID, APPLICATION_DATE,
            FAMILY_INCOME, APPLICATION_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_StudentID, @p_ScholarshipID, @p_ApplicationDate, @p_FamilyIncome, 'S', @p_ApplicantID, GETDATE());
        SET @p_ApplicationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_ApproveScholarship', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ApproveScholarship;
GO
CREATE PROCEDURE dbo.usp_ApproveScholarship
    @p_ApplicationID BIGINT, @p_ApprovedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.SCHOLARSHIP_APPLICATION
        SET APPLICATION_STATUS = 'A', UPDATED_BY = @p_ApprovedBy, UPDATED_ON = GETDATE()
        WHERE APPLICATION_ID = @p_ApplicationID;
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT '=== All stored procedures created ===';
GO

-- ============================================================================
-- 7. Seed Data - Problem Management Reference Data
-- ============================================================================
USE [ProblemManagementDb];
GO

IF NOT EXISTS (SELECT 1 FROM PROBLEM_FUNCTION)
BEGIN
    INSERT INTO PROBLEM_FUNCTION (FUNCID, FUNCNAME) VALUES
    (1, 'Human Resources'),
    (2, 'Information Technology'),
    (3, 'Finance'),
    (4, 'Operations'),
    (5, 'Quality Assurance');
END
GO

IF NOT EXISTS (SELECT 1 FROM PROBLEM_IMPACT)
BEGIN
    INSERT INTO PROBLEM_IMPACT (IMPACT_ID, IMPACT_DESC) VALUES
    (1, 'Critical - Business Stoppage'),
    (2, 'High - Major Process Impact'),
    (3, 'Medium - Moderate Impact'),
    (4, 'Low - Minor Impact');
END
GO

PRINT '=== Seed data inserted ===';
PRINT '=== Database initialization complete! ===';
GO
