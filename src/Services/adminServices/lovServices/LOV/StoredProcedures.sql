-- ==========================================
-- LOV SERVICE - STORED PROCEDURES
-- Database: LOVDB
-- ==========================================

USE [LOVDB];
GO

-- ==========================================
-- SP: usp_GetLovMastersByType
-- Gets all LOV masters for a given type
-- ==========================================
IF OBJECT_ID('usp_GetLovMastersByType', 'P') IS NOT NULL DROP PROCEDURE usp_GetLovMastersByType;
GO
CREATE PROCEDURE usp_GetLovMastersByType
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        LOV_ID          AS LovId,
        LOV_TYPE_ID     AS LovTypeId,
        LOV_NAME        AS LovName,
        LOV_UPDATED_BY  AS LovUpdatedBy,
        LOV_UPDATED_ON  AS LovUpdatedOn
    FROM LOV_MASTER WITH (NOLOCK)
    WHERE LOV_TYPE_ID = @LovTypeId
    ORDER BY LOV_NAME;
END
GO

-- ==========================================
-- SP: usp_GetAllLovTypes
-- ==========================================
IF OBJECT_ID('usp_GetAllLovTypes', 'P') IS NOT NULL DROP PROCEDURE usp_GetAllLovTypes;
GO
CREATE PROCEDURE usp_GetAllLovTypes
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName
    FROM LOV_TYPE WITH (NOLOCK)
    ORDER BY LOV_TYPE_NAME;
END
GO

-- ==========================================
-- SP: usp_UpsertLovType
-- Insert or update a LOV type
-- ==========================================
IF OBJECT_ID('usp_UpsertLovType', 'P') IS NOT NULL DROP PROCEDURE usp_UpsertLovType;
GO
CREATE PROCEDURE usp_UpsertLovType
    @LovTypeId   BIGINT,
    @LovTypeName VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM LOV_TYPE WHERE LOV_TYPE_ID = @LovTypeId)
        UPDATE LOV_TYPE SET LOV_TYPE_NAME = @LovTypeName WHERE LOV_TYPE_ID = @LovTypeId;
    ELSE
        INSERT INTO LOV_TYPE (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (@LovTypeId, @LovTypeName);
END
GO

-- ==========================================
-- SP: usp_UpsertLovMaster
-- ==========================================
IF OBJECT_ID('usp_UpsertLovMaster', 'P') IS NOT NULL DROP PROCEDURE usp_UpsertLovMaster;
GO
CREATE PROCEDURE usp_UpsertLovMaster
    @LovId        BIGINT,
    @LovTypeId    BIGINT,
    @LovName      VARCHAR(30),
    @LovUpdatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME2(3) = SYSDATETIME();
    IF EXISTS (SELECT 1 FROM LOV_MASTER WHERE LOV_ID = @LovId)
        UPDATE LOV_MASTER
        SET LOV_NAME = @LovName, LOV_UPDATED_BY = @LovUpdatedBy, LOV_UPDATED_ON = @Now
        WHERE LOV_ID = @LovId;
    ELSE
        INSERT INTO LOV_MASTER (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
        VALUES (@LovId, @LovTypeId, @LovName, @LovUpdatedBy, @Now);
END
GO

-- ==========================================
-- SP: usp_SearchItemData
-- ==========================================
IF OBJECT_ID('usp_SearchItemData', 'P') IS NOT NULL DROP PROCEDURE usp_SearchItemData;
GO
CREATE PROCEDURE usp_SearchItemData
    @CatName  VARCHAR(40) = NULL,
    @ItemName VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, CATNAME, ITEMNAME, MAKE, UOM, PRICE
    FROM ITEMDATA WITH (NOLOCK)
    WHERE (@CatName  IS NULL OR CATNAME  LIKE '%' + @CatName  + '%')
      AND (@ItemName IS NULL OR ITEMNAME LIKE '%' + @ItemName + '%')
    ORDER BY CATNAME, ITEMNAME;
END
GO

-- ==========================================
-- SP: usp_DeleteLovType
-- ==========================================
IF OBJECT_ID('usp_DeleteLovType', 'P') IS NOT NULL DROP PROCEDURE usp_DeleteLovType;
GO
CREATE PROCEDURE usp_DeleteLovType
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM LOV_MASTER WHERE LOV_TYPE_ID = @LovTypeId;
    DELETE FROM LOV_TYPE  WHERE LOV_TYPE_ID  = @LovTypeId;
END
GO

PRINT 'All stored procedures created successfully.';
GO
