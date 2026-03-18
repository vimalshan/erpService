-- ==========================================
-- MASTER MODULE - Stored Procedures
-- Database: LETDB
-- Purpose: Master data management procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- STORED PROCEDURES - MASTER DATA MANAGEMENT
-- ==========================================

-- Procedure: usp_Master_InsertSkill
-- Purpose: Insert a new skill into SKILL_MAST
IF OBJECT_ID('dbo.usp_Master_InsertSkill', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_InsertSkill;
GO
CREATE PROCEDURE dbo.usp_Master_InsertSkill
    @p_SkillCode BIGINT,
    @p_SkillName VARCHAR(255),
    @p_SkillType CHAR(1),
    @p_WeightNum DECIMAL(19,0) = NULL,
    @p_EffectiveDate DATETIME2(3) = NULL
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.SKILL_MAST (
            SK_SKL_COD,
            SK_SKL_NAM,
            SK_SKL_TYP,
            SK_WGT_NUM,
            SK_EFF_DAT
        )
        VALUES (
            @p_SkillCode,
            @p_SkillName,
            @p_SkillType,
            @p_WeightNum,
            ISNULL(@p_EffectiveDate, GETDATE())
        );
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Master_InsertTraining
-- Purpose: Insert a new training provider
IF OBJECT_ID('dbo.usp_Master_InsertTraining', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_InsertTraining;
GO
CREATE PROCEDURE dbo.usp_Master_InsertTraining
    @p_TrainingCode BIGINT,
    @p_TrainingName VARCHAR(255),
    @p_Address1 VARCHAR(255) = NULL,
    @p_ContactName VARCHAR(255) = NULL,
    @p_PhoneNum VARCHAR(255) = NULL
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.TRAIN_MAST (
            TR_TRN_COD,
            TR_TRN_NAM,
            TR_TRN_ADD1,
            TR_CNT_NAM1,
            TR_PHN_NUM1
        )
        VALUES (
            @p_TrainingCode,
            @p_TrainingName,
            @p_Address1,
            @p_ContactName,
            @p_PhoneNum
        );
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Master_InsertJobMaster
-- Purpose: Insert a new job master record
IF OBJECT_ID('dbo.usp_Master_InsertJobMaster', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_InsertJobMaster;
GO
CREATE PROCEDURE dbo.usp_Master_InsertJobMaster
    @p_JobCode BIGINT,
    @p_JobName VARCHAR(65),
    @p_CategoryCode CHAR(3)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.JOB_MAST (
            JB_JOB_COD,
            JB_JOB_NAM,
            JB_CAT_COD
        )
        VALUES (
            @p_JobCode,
            @p_JobName,
            @p_CategoryCode
        );
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Master_GetSkills
-- Purpose: Retrieve all skills
IF OBJECT_ID('dbo.usp_Master_GetSkills', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_GetSkills;
GO
CREATE PROCEDURE dbo.usp_Master_GetSkills
    @p_SkillType CHAR(1) = NULL
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        SK_SKL_COD,
        SK_SKL_NAM,
        SK_SKL_TYP,
        SK_WGT_NUM,
        SK_EFF_DAT,
        SK_CLS_DAT
    FROM dbo.SKILL_MAST
    WHERE (@p_SkillType IS NULL OR SK_SKL_TYP = @p_SkillType)
    ORDER BY SK_SKL_NAM;
END;
GO

-- Procedure: usp_Master_GetTrainings
-- Purpose: Retrieve all training master records
IF OBJECT_ID('dbo.usp_Master_GetTrainings', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_GetTrainings;
GO
CREATE PROCEDURE dbo.usp_Master_GetTrainings
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        TR_TRN_COD,
        TR_TRN_NAM,
        TR_TRN_ADD1,
        TR_CNT_NAM1,
        TR_PHN_NUM1,
        TR_EFF_DAT
    FROM dbo.TRAIN_MAST
    WHERE TR_CAN_DAT IS NULL
    ORDER BY TR_TRN_NAM;
END;
GO

-- Procedure: usp_Master_GetJobs
-- Purpose: Retrieve jobs by category
IF OBJECT_ID('dbo.usp_Master_GetJobs', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_GetJobs;
GO
CREATE PROCEDURE dbo.usp_Master_GetJobs
    @p_CategoryCode CHAR(3) = NULL
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        JB_JOB_COD,
        JB_JOB_NAM,
        JB_CAT_COD
    FROM dbo.JOB_MAST
    WHERE (@p_CategoryCode IS NULL OR JB_CAT_COD = @p_CategoryCode)
    ORDER BY JB_JOB_NAM;
END;
GO

-- Procedure: usp_Master_GetCategories
-- Purpose: Retrieve all categories
IF OBJECT_ID('dbo.usp_Master_GetCategories', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_GetCategories;
GO
CREATE PROCEDURE dbo.usp_Master_GetCategories
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CT_CAT_COD,
        CT_CAT_NAM,
        CT_SRL_NUM
    FROM dbo.CAT_MAST
    ORDER BY CT_CAT_NAM;
END;
GO

-- Procedure: usp_Master_GetActiveFiscalYears
-- Purpose: Retrieve active financial years
IF OBJECT_ID('dbo.usp_Master_GetActiveFiscalYears', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Master_GetActiveFiscalYears;
GO
CREATE PROCEDURE dbo.usp_Master_GetActiveFiscalYears
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        AC_SRL_NUM,
        AC_STR_DAT,
        AC_END_DAT,
        AC_CLS_FLG
    FROM dbo.COMP_FINYEAR
    WHERE AC_CLS_FLG = 'N'
    ORDER BY AC_STR_DAT DESC;
END;
GO

PRINT 'Master Module Procedures created successfully.';
GO
