USE ERPScheduleDB;
GO

SET IDENTITY_INSERT dbo.AuditSiteAudits ON;
INSERT INTO dbo.AuditSiteAudits
  (AuditSiteAuditId, AuditId, SiteId, AuditTypeId, AuditNumber, ScheduledDate, StartDate, EndDate, CompletedDate, Status, LeadAuditorId, IsActive, CreatedBy, ModifiedBy, Notes, ReportPath, CertificateIssued, CertificateNumber)
VALUES
  (1, 1001, 1, 1, 'AUD-2026-001', '2026-06-01', NULL,        NULL,        NULL,        'scheduled',  1, 1, 1, 1, 'Annual surveillance',    NULL,                  0, NULL),
  (2, 1001, 2, 1, 'AUD-2026-002', '2026-06-15', NULL,        NULL,        NULL,        'scheduled',  1, 1, 1, 1, 'Surveillance site 2',    NULL,                  0, NULL),
  (3, 1002, 3, 2, 'AUD-2026-003', '2026-05-10', '2026-05-10',NULL,        NULL,        'in_progress',2, 1, 1, 1, 'Initial certification',  NULL,                  0, NULL),
  (4, 1002, 1, 2, 'AUD-2026-004', '2026-04-01', '2026-04-01','2026-04-03','2026-04-05','completed',  2, 1, 1, 1, 'Recertification done',   '/reports/r004.pdf',   1, 'CERT-2026-004'),
  (5, 1003, 4, 3, 'AUD-2026-005', '2026-07-01', NULL,        NULL,        NULL,        'scheduled',  3, 1, 1, 1, 'Transfer audit',         NULL,                  0, NULL);
SET IDENTITY_INSERT dbo.AuditSiteAudits OFF;
GO

SELECT COUNT(*) AS AuditSiteAuditCount FROM dbo.AuditSiteAudits;
GO
