-- Seed CompanySettings for all companies
-- Provides default JSON configuration for each company

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 1)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (1, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":25},"audit":{"requireLeadAuditorSign":true,"autoCloseFindings":false},"system":{"maintenanceMode":false,"maxFileUploadMB":10}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 2)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (2, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":20},"audit":{"requireLeadAuditorSign":false,"autoCloseFindings":false}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 3)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (3, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":20},"audit":{"requireLeadAuditorSign":false,"autoCloseFindings":true}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 4)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (4, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":25},"audit":{"requireLeadAuditorSign":true,"autoCloseFindings":false}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 5)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (5, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":25},"audit":{"requireLeadAuditorSign":true,"autoCloseFindings":false}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 6)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (6, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":20},"audit":{"requireLeadAuditorSign":true,"autoCloseFindings":false}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 7)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (7, N'{"notifications":{"auditReminders":true,"certificateExpiry":false,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":20}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 8)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (8, N'{"notifications":{"auditReminders":false,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 9)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (9, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":false},"display":{"defaultView":"overview","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 10)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (10, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":20}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 11)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (11, N'{"notifications":{"auditReminders":false,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":20}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 12)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (12, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 13)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (13, N'{"notifications":{"auditReminders":true,"certificateExpiry":false,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 14)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (14, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":false},"display":{"defaultView":"dashboard","itemsPerPage":20}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 15)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (15, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":false,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 16)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (16, N'{"notifications":{"auditReminders":false,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":20}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 17)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (17, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"overview","itemsPerPage":50},"system":{"maxFileUploadMB":25}}', GETDATE(), GETDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanySettings] WHERE [CompanyId] = 18)
    INSERT INTO [dbo].[CompanySettings] ([CompanyId], [SettingsJson], [CreatedDate], [ModifiedDate], [UpdatedBy])
    VALUES (18, N'{"notifications":{"auditReminders":true,"certificateExpiry":true,"invoiceAlerts":true,"findingUpdates":true},"display":{"defaultView":"dashboard","itemsPerPage":25}}', GETDATE(), GETDATE(), 1);

SELECT COUNT(*) AS TotalCompanySettings FROM [dbo].[CompanySettings];
