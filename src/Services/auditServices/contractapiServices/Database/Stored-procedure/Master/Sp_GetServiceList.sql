CREATE PROCEDURE [dbo].[Sp_GetServiceList]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT s.ServiceId AS id,
               COALESCE(s.ServiceName, 'Unknown Service') AS serviceName
        FROM   Services s
        WHERE  s.IsActive = 1
        ORDER  BY s.ServiceName;
    END TRY
    BEGIN CATCH
        SELECT NULL AS id, ERROR_MESSAGE() AS serviceName;
    END CATCH
END


