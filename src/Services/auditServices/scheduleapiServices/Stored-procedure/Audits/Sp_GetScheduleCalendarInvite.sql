-- =============================================
-- Author: Generated for Customer Portal Backend
-- Create date: 2025-09-19 (Revised)
-- Description: Generate calendar invite for an audit schedule
-- Schema: AuditSiteAudits (AuditSiteAuditId), Sites, AuditTypes, Users,
--         AuditSiteServices, Services, AuditSiteRepresentatives
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[Sp_GetScheduleCalendarInvite]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF ISJSON(@Parameters) = 0
    BEGIN
        SELECT '{"isSuccess":false,"message":"Invalid JSON parameters","errorCode":"INVALID_JSON","data":null}' AS JsonResponse;
        RETURN;
    END

    DECLARE @UserId         INT = TRY_CAST(JSON_VALUE(@Parameters, '$.userId') AS INT);
    DECLARE @IsAddToCalender BIT = CASE WHEN JSON_VALUE(@Parameters, '$.isAddToCalender') = 'true' THEN 1 ELSE 0 END;
    DECLARE @SiteAuditId    INT = TRY_CAST(JSON_VALUE(@Parameters, '$.siteAuditId') AS INT);

    IF @SiteAuditId IS NULL
    BEGIN
        SELECT '{"isSuccess":false,"message":"SiteAuditId is required","errorCode":"MISSING_PARAMETERS","data":null}' AS JsonResponse;
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM AuditSiteAudits WHERE AuditSiteAuditId = @SiteAuditId AND IsActive = 1)
    BEGIN
        SELECT '{"isSuccess":false,"message":"Audit schedule not found","errorCode":"AUDIT_NOT_FOUND","data":null}' AS JsonResponse;
        RETURN;
    END

    BEGIN TRY
        -- Get core audit details
        DECLARE @AuditTypeName    NVARCHAR(200);
        DECLARE @StartDate        DATETIME2;
        DECLARE @EndDate          DATETIME2;
        DECLARE @LeadAuditorName  NVARCHAR(200);
        DECLARE @SiteName         NVARCHAR(500);
        DECLARE @SiteLocation     NVARCHAR(1000);

        SELECT
            @AuditTypeName   = ISNULL(atype.AuditTypeName, 'Standard Audit'),
            @StartDate       = asa.StartDate,
            @EndDate         = asa.EndDate,
            @LeadAuditorName = ISNULL(usr.Username, ''),
            @SiteName        = ISNULL(s.SiteName, ''),
            @SiteLocation    = ISNULL(s.Address, '')
        FROM AuditSiteAudits asa
        INNER JOIN Sites s          ON asa.SiteId        = s.SiteId
        LEFT JOIN  AuditTypes atype ON asa.AuditTypeId   = atype.AuditTypeId
        LEFT JOIN  Users usr        ON asa.LeadAuditorId = usr.UserId
        WHERE asa.AuditSiteAuditId = @SiteAuditId AND asa.IsActive = 1;

        -- Get primary service name
        DECLARE @ServiceName NVARCHAR(500) = '';
        SELECT TOP 1 @ServiceName = ISNULL(svc.ServiceName, '')
        FROM AuditSiteServices ass
        INNER JOIN Services svc ON ass.ServiceId = svc.ServiceId
        WHERE ass.AuditSiteAuditId = @SiteAuditId AND ass.IsActive = 1 AND svc.IsActive = 1
        ORDER BY ass.CreatedDate;

        -- Get primary site representative username
        DECLARE @SiteRepresentative NVARCHAR(500) = '';
        SELECT TOP 1 @SiteRepresentative = ISNULL(u2.Username, '')
        FROM AuditSiteRepresentatives asr
        INNER JOIN Users u2 ON asr.UserId = u2.UserId
        WHERE asr.AuditSiteAuditId = @SiteAuditId AND asr.IsActive = 1
        ORDER BY asr.CreatedDate;

        -- Build ICS content
        DECLARE @ICSContent NVARCHAR(MAX) = CONCAT(
            'BEGIN:VCALENDAR', CHAR(13), CHAR(10),
            'PRODID:-//Audit Schedule//EN', CHAR(13), CHAR(10),
            'VERSION:2.0', CHAR(13), CHAR(10),
            'METHOD:REQUEST', CHAR(13), CHAR(10),
            'BEGIN:VEVENT', CHAR(13), CHAR(10),
            'SUMMARY:Audit Schedule: ', @AuditTypeName, ' - ', @SiteName, CHAR(13), CHAR(10),
            'UID:audit-', CAST(@SiteAuditId AS NVARCHAR(10)), '@auditportal', CHAR(13), CHAR(10),
            CASE WHEN @StartDate IS NOT NULL THEN CONCAT('DTSTART:', FORMAT(@StartDate, 'yyyyMMddTHHmmssZ'), CHAR(13), CHAR(10)) ELSE '' END,
            CASE WHEN @EndDate   IS NOT NULL THEN CONCAT('DTEND:',   FORMAT(@EndDate,   'yyyyMMddTHHmmssZ'), CHAR(13), CHAR(10)) ELSE '' END,
            'LOCATION:', @SiteLocation, CHAR(13), CHAR(10),
            'STATUS:CONFIRMED', CHAR(13), CHAR(10),
            'END:VEVENT', CHAR(13), CHAR(10),
            'END:VCALENDAR'
        );

        -- Convert ICS string to byte array (ASCII codes as JSON int array)
        DECLARE @ByteArray NVARCHAR(MAX) = '';
        DECLARE @i    INT = 1;
        DECLARE @len  INT = LEN(@ICSContent);
        WHILE @i <= @len
        BEGIN
            IF @i > 1 SET @ByteArray = @ByteArray + ',';
            SET @ByteArray = @ByteArray + CAST(UNICODE(SUBSTRING(@ICSContent, @i, 1)) AS NVARCHAR(10));
            SET @i = @i + 1;
        END

        -- Build calendarAttributes JSON via FOR JSON
        DECLARE @CalAttrJson NVARCHAR(MAX) = (
            SELECT
                @AuditTypeName    AS auditType,
                @EndDate          AS endDate,
                @LeadAuditorName  AS leadAuditor,
                @ServiceName      AS service,
                @SiteName         AS site,
                @SiteLocation     AS siteAddress,
                @SiteRepresentative AS siteRepresentative,
                @StartDate        AS startDate
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        DECLARE @Data NVARCHAR(MAX) =
            '{"icsResponse":[' + @ByteArray + '],"calendarAttributes":' + @CalAttrJson + '}';

        SELECT '{"isSuccess":true,"message":"Success","errorCode":null,"data":' + @Data + '}' AS JsonResponse;
    END TRY
    BEGIN CATCH
        INSERT INTO ErrorLogs (UserId, ErrorMessage, StackTrace, CreatedDate)
        VALUES (@UserId, ERROR_MESSAGE(), CONCAT('Procedure: Sp_GetScheduleCalendarInvite, Line: ', ERROR_LINE()), GETDATE());
        SELECT '{"isSuccess":false,"message":"An error occurred retrieving calendar invite.","errorCode":"SERVER_ERROR","data":null}' AS JsonResponse;
    END CATCH
END
