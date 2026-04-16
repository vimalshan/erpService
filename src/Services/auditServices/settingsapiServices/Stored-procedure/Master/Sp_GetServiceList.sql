CREATE OR ALTER PROCEDURE [dbo].[Sp_GetServiceList]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ServiceListJson NVARCHAR(MAX) = (
        SELECT
            s.ServiceId                              AS id,
            COALESCE(s.ServiceName, 'Unknown Service') AS serviceName,
            COALESCE(s.ServiceCode, '')               AS serviceCode,
            COALESCE(s.Description, '')               AS description,
            CAST(ISNULL(s.IsActive, 1) AS BIT)        AS isActive
        FROM [dbo].[Services] s
        WHERE s.IsActive = 1
        ORDER BY s.ServiceName
        FOR JSON PATH
    );

    IF @ServiceListJson IS NULL
        SET @ServiceListJson = '[]';

    SELECT (
        SELECT
            JSON_QUERY(@ServiceListJson) AS data,
            CAST(1 AS BIT)              AS isSuccess,
            ''                          AS message,
            ''                          AS errorCode
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonResponse;
END


