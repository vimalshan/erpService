USE [StationeryDB];
GO

CREATE OR ALTER PROCEDURE [dbo].[GetLowStockItems]
    @Threshold BIGINT
AS
BEGIN
    SELECT 
        SM_STATIONARYID as Id, 
        SM_DESC as Description, 
        SM_OPENINGSTOCK as Stock, 
        SM_REORDER_LEVEL as ReorderLevel
    FROM [STATIONARY_MASTER]
    WHERE SM_OPENINGSTOCK <= SM_REORDER_LEVEL 
       OR SM_OPENINGSTOCK <= @Threshold;
END;
GO
