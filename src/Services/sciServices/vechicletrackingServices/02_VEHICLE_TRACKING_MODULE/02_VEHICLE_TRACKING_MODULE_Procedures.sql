-- ==========================================
-- VEHICLE TRACKING MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Vehicle Management & Tracking
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterVehicle', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterVehicle;
GO
CREATE PROCEDURE dbo.usp_RegisterVehicle
    @p_RegNum1 VARCHAR(3),
    @p_RegNum2 VARCHAR(2),
    @p_RegNum3 VARCHAR(2),
    @p_RegNum4 VARCHAR(4),
    @p_RegDate DATETIME2,
    @p_UpdatedBy VARCHAR(25),
    @p_UpdatedByNum BIGINT,
    @p_VehicleID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.VEHICLE_MAST (VH_REG_NUM1, VH_REG_NUM2, VH_REG_NUM3, VH_REG_NUM4, VH_REG_DAT, VH_UPD_DAT, VH_UPD_USR, VH_UPD_NUM)
            VALUES (@p_RegNum1, @p_RegNum2, @p_RegNum3, @p_RegNum4, @p_RegDate, GETDATE(), @p_UpdatedBy, @p_UpdatedByNum);
            SET @p_VehicleID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_UpdateVehicleStage', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_UpdateVehicleStage;
GO
CREATE PROCEDURE dbo.usp_UpdateVehicleStage
    @p_TrackingNumber BIGINT,
    @p_VehicleTracker BIGINT,
    @p_StageCode BIGINT,
    @p_StageDecision CHAR(1),
    @p_EnteredBy VARCHAR(25),
    @p_EnteredByNum BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.VEHICLE_STAGE (ST_TRN_NUM, ST_TRK_NUM, ST_STG_SRL, ST_ENT_DAT, ST_ENT_USR, ST_ENT_NUM, ST_LEV_DAT, ST_ROL_COD, ST_DEC_FLG, ST_CAN_STS, VT_STG_COD, VT_DEL_DAT, VT_DEL_USR, VT_DEL_NUM)
            VALUES (@p_TrackingNumber, @p_VehicleTracker, 1, GETDATE(), @p_EnteredBy, @p_EnteredByNum, GETDATE(), 0, @p_StageDecision, 'N', @p_StageCode, GETDATE(), @p_EnteredBy, @p_EnteredByNum);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetVehicleStages', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetVehicleStages;
GO
CREATE PROCEDURE dbo.usp_GetVehicleStages
    @p_TrackingNumber BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT st.ST_TRK_NUM, st.ST_ENT_DAT, SM.ST_OPT_NAM, st.ST_DEC_FLG, st.ST_CAN_STS
        FROM dbo.VEHICLE_STAGE st
        INNER JOIN dbo.STAGE_MAST SM ON st.VT_STG_COD = SM.ST_STG_COD
        WHERE st.ST_TRN_NUM = @p_TrackingNumber
        ORDER BY st.ST_ENT_DAT DESC;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'VEHICLE_TRACKING_MODULE Procedures created successfully.';
GO
