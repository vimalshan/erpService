CREATE PROCEDURE [dbo].[Sp_GetWidgetForUpcomingAudit]
    @startDate DATE = NULL,
    @endDate   DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @startDate IS NULL SET @startDate = CAST(GETDATE() AS DATE);
        IF @endDate   IS NULL SET @endDate   = DATEADD(MONTH, 3, @startDate);

        SELECT COALESCE(a.Status, 'ToBeConfirmed')        AS confirmed,
               COALESCE(a.Status, 'ToBeConfirmed')        AS toBeConfirmed,
               COALESCE(a.Status, 'ToBeConfirmedByDNV')   AS toBeConfirmedByDNV
        FROM   Audits a
        WHERE  a.StartDate >= @startDate
          AND  a.StartDate <= @endDate
        ORDER  BY a.StartDate;
    END TRY
    BEGIN CATCH
        SELECT '' AS confirmed, '' AS toBeConfirmed, '' AS toBeConfirmedByDNV;
    END CATCH
END


