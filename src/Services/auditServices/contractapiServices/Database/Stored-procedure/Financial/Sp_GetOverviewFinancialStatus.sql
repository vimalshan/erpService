CREATE PROCEDURE [dbo].[Sp_GetOverviewFinancialStatus]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT COALESCE(i.Status, 'Unknown')                                      AS financialStatus,
               COUNT(*)                                                           AS financialCount,
               ROUND(CAST(COUNT(*) AS FLOAT)
                   / NULLIF(CAST(SUM(COUNT(*)) OVER () AS FLOAT), 0) * 100, 1)   AS financialpercentage
        FROM   Invoices i
        WHERE  i.IsActive = 1
        GROUP  BY i.Status
        ORDER  BY financialCount DESC;
    END TRY
    BEGIN CATCH
        SELECT '' AS financialStatus, 0 AS financialCount, 0.0 AS financialpercentage;
    END CATCH
END