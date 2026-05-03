USE ERPNotificationDB;
GO

DELETE FROM dbo.Notifications;
DELETE FROM dbo.NotificationCategories;
GO

SET IDENTITY_INSERT dbo.NotificationCategories ON;
INSERT INTO dbo.NotificationCategories (CategoryId, CategoryName, CategoryCode, Description, IsActive, Color, Icon, Priority, DisplayOrder)
VALUES
 (1, 'System',      'SYS',  'System-level alerts',    1, '#FF0000', 'system',   1, 1),
 (2, 'Audit',       'AUD',  'Audit lifecycle events', 1, '#0066CC', 'audit',    2, 2),
 (3, 'Finding',     'FND',  'Finding-related events', 1, '#FFA500', 'finding',  3, 3),
 (4, 'Certificate', 'CERT', 'Certificate events',     1, '#008000', 'cert',     4, 4),
 (5, 'Training',    'TRN',  'Training events',        1, '#9933CC', 'training', 5, 5);
SET IDENTITY_INSERT dbo.NotificationCategories OFF;
GO

INSERT INTO dbo.Notifications (Title, Message, CategoryId, CompanyId, SiteId, ServiceId, Priority, Status, CreatedBy, ModifiedBy, ExpiryDate, IsActive, TargetAudience, ActionRequired, ActionUrl, RelatedEntityType, RelatedEntityId)
VALUES
 ('System maintenance window',     'Scheduled maintenance on 2026-05-10 02:00 UTC.',         1, NULL, NULL, NULL, 'High',   'Active', 1, 1, DATEADD(day, 14, GETDATE()), 1, 'All',      0, NULL,                  NULL,          NULL),
 ('Audit kickoff: ACME-2026-01',   'Audit ACME-2026-01 has been scheduled for next week.',   2, 1,    1,    1,    'Medium', 'Active', 1, 1, DATEADD(day, 7,  GETDATE()), 1, 'Auditors', 1, '/audits/1001',        'Audit',       1001),
 ('Finding overdue: F-2026-014',   'Finding F-2026-014 is past its due date.',               3, 1,    1,    1,    'High',   'Active', 1, 1, NULL,                        1, 'Owners',   1, '/findings/14',        'Finding',     14),
 ('Certificate expiring',          'Certificate CERT-2026-008 expires in 30 days.',          4, 2,    2,    NULL, 'Medium', 'Active', 1, 1, DATEADD(day, 30, GETDATE()), 1, 'Managers', 1, '/certificates/8',     'Certificate', 8),
 ('Training reminder',             'Mandatory annual training due by month end.',            5, NULL, NULL, NULL, 'Low',    'Active', 1, 1, DATEADD(day, 30, GETDATE()), 1, 'All',      0, NULL,                  NULL,          NULL),
 ('Finding closed',                'Finding F-2025-098 has been closed.',                    3, 3,    3,    1,    'Low',    'Closed', 1, 1, NULL,                        1, 'Owners',   0, NULL,                  'Finding',     98);
GO

SELECT 'Categories' AS Tbl, COUNT(*) AS Cnt FROM dbo.NotificationCategories
UNION ALL
SELECT 'Notifications',     COUNT(*) FROM dbo.Notifications;
GO
