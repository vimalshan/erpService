import os

base = r"e:\ERPMicroservice\src\Services\auditServices\scheduleapiServices\Stored-procedure\Audits"

sp1 = r"""-- =============================================
-- Author: Generated for Customer Portal Backend
-- Create date: 2025-09-19 (Revised)
-- Description: Get audit schedules with filtering capabilities
-- Schema: AuditSiteAudits, Sites, Companies, AuditTypes, Users,
--         AuditSiteServices, AuditSiteRepresentatives, UserSiteAccess
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[Sp_GetAuditSchedules]
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF ISJSON(@Parameters) = 0
    BEGIN
        SELECT '{"isSuccess":false,"message":"Invalid JSON parameters","errorCode":"INVALID_JSON","data":null}' AS JsonResponse;
        RETURN;
    END

    DECLARE @UserId       INT           = TRY_CAST(JSON_VALUE(@Parameters, '$.userId') AS INT);
    DECLARE @Filter       NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Parameters, '$.calendarScheduleFilter'), '{}');
    IF ISJSON(@Filter) = 0 SET @Filter = '{}';

    -- C# CalendarScheduleFilterInput serializes property names as PascalCase (default System.Text.Json)
    DECLARE @FromDate       DATETIME2     = TRY_CAST(JSON_VALUE(@Filter, '$.FromDate') AS DATETIME2);
    DECLARE @ToDate         DATETIME2     = TRY_CAST(JSON_VALUE(@Filter, '$.ToDate')   AS DATETIME2);
    DECLARE @CompanyIdsJson NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Filter, '$.CompanyIds'), '[]');
    DECLARE @SiteIdsJson    NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Filter, '$.SiteIds'),    '[]');
    DECLARE @ServiceIdsJson NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Filter, '$.ServiceIds'), '[]');
    DECLARE @StatusesJson   NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Filter, '$.Statuses'),   '[]');

    IF ISJSON(@CompanyIdsJson) = 0 SET @CompanyIdsJson = '[]';
    IF ISJSON(@SiteIdsJson)    = 0 SET @SiteIdsJson    = '[]';
    IF ISJSON(@ServiceIdsJson) = 0 SET @ServiceIdsJson = '[]';
    IF ISJSON(@StatusesJson)   = 0 SET @StatusesJson   = '[]';

    DECLARE @Results NVARCHAR(MAX);

    BEGIN TRY
        ;WITH ServiceAgg AS (
            -- Flat JSON integer array of ServiceIds per audit: [1,2,3]
            SELECT
                ass.AuditSiteAuditId,
                '[' + STRING_AGG(CAST(ass.ServiceId AS NVARCHAR(10)), ',') + ']' AS ServiceIdsJson
            FROM AuditSiteServices ass
            WHERE ass.IsActive = 1
            GROUP BY ass.AuditSiteAuditId
        ),
        RepAgg AS (
            -- Flat JSON string array of representative usernames per audit
            SELECT
                asr.AuditSiteAuditId,
                '["' + STRING_AGG(
                    REPLACE(REPLACE(u.Username, CHAR(92), CHAR(92)+CHAR(92)), CHAR(34), CHAR(92)+CHAR(34)),
                    '","'
                ) + '"]' AS ReprJson
            FROM AuditSiteRepresentatives asr
            INNER JOIN Users u ON asr.UserId = u.UserId
            WHERE asr.IsActive = 1
            GROUP BY asr.AuditSiteAuditId
        )
        SELECT @Results = (
            SELECT
                asa.AuditSiteAuditId                          AS siteAuditId,
                asa.StartDate                                 AS startDate,
                asa.EndDate                                   AS endDate,
                ISNULL(asa.Status, 'Scheduled')               AS status,
                JSON_QUERY(ISNULL(svc.ServiceIdsJson, '[]'))  AS serviceIds,
                asa.SiteId                                    AS siteId,
                ISNULL(atype.AuditTypeName, 'Standard Audit') AS auditType,
                ISNULL(usr.Username, '')                      AS leadAuditor,
                JSON_QUERY(ISNULL(rep.ReprJson, '[]'))        AS siteRepresentatives,
                s.CompanyId                                   AS companyId,
                asa.AuditId                                   AS auditId,
                CAST(NULL AS NVARCHAR(100))                   AS reportingCountry,
                CAST(NULL AS NVARCHAR(100))                   AS projectNumber,
                ISNULL(c.AccountDNVId, '')                    AS accountDNVId
            FROM AuditSiteAudits asa
            INNER JOIN Sites s          ON asa.SiteId        = s.SiteId
            LEFT JOIN  Companies c      ON s.CompanyId       = c.CompanyId
            LEFT JOIN  AuditTypes atype ON asa.AuditTypeId   = atype.AuditTypeId
            LEFT JOIN  Users usr        ON asa.LeadAuditorId = usr.UserId
            LEFT JOIN  ServiceAgg svc   ON svc.AuditSiteAuditId = asa.AuditSiteAuditId
            LEFT JOIN  RepAgg rep       ON rep.AuditSiteAuditId = asa.AuditSiteAuditId
            WHERE asa.IsActive = 1
              AND s.IsActive   = 1
              AND (
                    @UserId IS NULL
                    OR EXISTS (
                        SELECT 1 FROM UserSiteAccess usa
                        WHERE usa.UserId = @UserId AND usa.SiteId = asa.SiteId AND usa.IsActive = 1
                    )
              )
              AND (
                    NOT EXISTS (SELECT 1 FROM OPENJSON(@CompanyIdsJson))
                    OR s.CompanyId IN (SELECT CAST(j.value AS INT) FROM OPENJSON(@CompanyIdsJson) j)
              )
              AND (
                    NOT EXISTS (SELECT 1 FROM OPENJSON(@SiteIdsJson))
                    OR asa.SiteId IN (SELECT CAST(j.value AS INT) FROM OPENJSON(@SiteIdsJson) j)
              )
              AND (
                    NOT EXISTS (SELECT 1 FROM OPENJSON(@ServiceIdsJson))
                    OR EXISTS (
                        SELECT 1 FROM AuditSiteServices ass2
                        WHERE ass2.AuditSiteAuditId = asa.AuditSiteAuditId
                          AND ass2.IsActive = 1
                          AND ass2.ServiceId IN (SELECT CAST(j.value AS INT) FROM OPENJSON(@ServiceIdsJson) j)
                    )
              )
              AND (
                    NOT EXISTS (SELECT 1 FROM OPENJSON(@StatusesJson))
                    OR asa.Status IN (SELECT j.value FROM OPENJSON(@StatusesJson) j)
              )
              AND (@FromDate IS NULL OR asa.StartDate IS NULL OR asa.StartDate >= @FromDate)
              AND (@ToDate   IS NULL OR asa.EndDate   IS NULL OR asa.EndDate   <= @ToDate)
            ORDER BY asa.AuditSiteAuditId DESC
            FOR JSON PATH
        );

        IF @Results IS NULL SET @Results = '[]';
        SELECT '{"isSuccess":true,"message":"Success","errorCode":null,"data":' + @Results + '}' AS JsonResponse;
    END TRY
    BEGIN CATCH
        INSERT INTO ErrorLogs (UserId, ErrorMessage, StackTrace, CreatedAt)
        VALUES (@UserId, ERROR_MESSAGE(), CONCAT('Procedure: Sp_GetAuditSchedules, Line: ', ERROR_LINE()), GETDATE());
        SELECT '{"isSuccess":false,"message":"An error occurred retrieving audit schedules.","errorCode":"SERVER_ERROR","data":null}' AS JsonResponse;
    END CATCH
END
"""

sp2 = r"""-- =============================================
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
"""

with open(os.path.join(base, "Sp_GetAuditSchedules.sql"), "w", encoding="utf-8") as f:
    f.write(sp1)
print("Wrote Sp_GetAuditSchedules.sql:", len(sp1), "chars")

with open(os.path.join(base, "Sp_GetScheduleCalendarInvite.sql"), "w", encoding="utf-8") as f:
    f.write(sp2)
print("Wrote Sp_GetScheduleCalendarInvite.sql:", len(sp2), "chars")
