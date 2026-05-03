USE ERPOverviewDB;
GO

SET IDENTITY_INSERT dbo.WidgetConfigs ON;
INSERT INTO dbo.WidgetConfigs (Id, WidgetKey, DisplayName, IsEnabled, DisplayOrder, Configuration)
VALUES
 (1, 'certification_quicklink', 'Certification Quicklink Card', 1, 1, NULL),
 (2, 'financial_status',        'Financial Status Widget',      1, 2, NULL),
 (3, 'upcoming_audit',          'Upcoming Audit Widget',        1, 3, NULL),
 (4, 'training_status',         'Training Status Widget',       1, 4, NULL);
SET IDENTITY_INSERT dbo.WidgetConfigs OFF;
GO

SELECT COUNT(*) AS WidgetConfigCount FROM dbo.WidgetConfigs;
GO
