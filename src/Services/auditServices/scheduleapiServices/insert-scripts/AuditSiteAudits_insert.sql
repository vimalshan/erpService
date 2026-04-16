-- Insert sample data for AuditSiteAudits table
-- Columns: AuditId, SiteId, AuditTypeId, AuditNumber (UNIQUE),
--          ScheduledDate, StartDate, EndDate, CompletedDate,
--          Status, LeadAuditorId, IsActive, CreatedDate, ModifiedDate,
--          CreatedBy, ModifiedBy, Notes, CertificateIssued
-- AuditTypeId: 1=ICA, 2=Surveillance, 6=Pre-Assessment
-- LeadAuditorId refs DNV staff: 4=M.Brown, 5=S.Wilson, 6=L.Garcia,
--   7=R.Anderson, 8=E.Martinez, 9=H.Tanaka, 10=K.Lee

INSERT INTO [dbo].[AuditSiteAudits]
    ([AuditId], [SiteId], [AuditTypeId], [AuditNumber],
     [ScheduledDate], [StartDate], [EndDate], [CompletedDate],
     [Status], [LeadAuditorId], [IsActive],
     [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy],
     [Notes], [CertificateIssued])
VALUES
-- Audit 1: Acme ISO9001 ICA — Sites 1,2,3
(1, 1, 1, 'AUD-2024-001', '2024-03-10', '2024-03-15', '2024-03-19', '2024-03-19', 'completed', 4, 1, GETDATE(), GETDATE(), 1, 4, 'ISO 9001 ICA - Acme HQ',           1),
(1, 2, 1, 'AUD-2024-002', '2024-03-10', '2024-03-17', '2024-03-19', '2024-03-19', 'completed', 4, 1, GETDATE(), GETDATE(), 1, 4, 'ISO 9001 ICA - Acme Manufacturing', 1),
(1, 3, 1, 'AUD-2024-003', '2024-03-10', '2024-03-18', '2024-03-19', '2024-03-19', 'completed', 7, 1, GETDATE(), GETDATE(), 1, 7, 'ISO 9001 ICA - Acme Warehouse',    1),

-- Audit 2: Acme ISO9001 Surveillance Y1 — Sites 1,2
(2, 1, 2, 'AUD-2025-001', '2025-03-08', '2025-03-10', '2025-03-12', '2025-03-12', 'completed', 7, 1, GETDATE(), GETDATE(), 1, 7, 'ISO 9001 Surv Y1 - Acme HQ',  0),
(2, 2, 2, 'AUD-2025-002', '2025-03-08', '2025-03-11', '2025-03-12', '2025-03-12', 'completed', 7, 1, GETDATE(), GETDATE(), 1, 7, 'ISO 9001 Surv Y1 - Acme Mfg', 0),

-- Audit 3: Acme ISO14001 ICA — Sites 1,2
(3, 1, 1, 'AUD-2024-004', '2024-05-18', '2024-05-20', '2024-05-24', '2024-05-24', 'completed', 4, 1, GETDATE(), GETDATE(), 1, 4, 'ISO 14001 ICA - Acme HQ',  1),
(3, 2, 1, 'AUD-2024-005', '2024-05-18', '2024-05-22', '2024-05-24', '2024-05-24', 'completed', 8, 1, GETDATE(), GETDATE(), 1, 8, 'ISO 14001 ICA - Acme Mfg', 1),

-- Audit 4: Acme ISO14001 Surveillance Y1 — Sites 1,2 (scheduled)
(4, 1, 2, 'AUD-2025-003', '2025-05-15', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 14001 Surv Y1 - Acme HQ',  0),
(4, 2, 2, 'AUD-2025-004', '2025-05-16', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 14001 Surv Y1 - Acme Mfg', 0),

-- Audit 5: Acme ISO45001 Pre-Assessment — Site 1 (scheduled)
(5, 1, 6, 'AUD-2025-005', '2025-07-07', NULL, NULL, NULL, 'scheduled', 8, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 45001 Pre-Assessment - Acme HQ', 0),

-- Audit 6: TechFlow ISO9001 ICA — Sites 4,5,6
(6, 4, 1, 'AUD-2023-001', '2023-09-01', '2023-09-05', '2023-09-09', '2023-09-09', 'completed', 5, 1, GETDATE(), GETDATE(), 1, 5, 'ISO 9001 ICA - TechFlow Main Office', 1),
(6, 5, 1, 'AUD-2023-002', '2023-09-01', '2023-09-07', '2023-09-09', '2023-09-09', 'completed', 5, 1, GETDATE(), GETDATE(), 1, 5, 'ISO 9001 ICA - TechFlow Data Center', 1),
(6, 6, 1, 'AUD-2023-003', '2023-09-01', '2023-09-08', '2023-09-09', '2023-09-09', 'completed', 5, 1, GETDATE(), GETDATE(), 1, 5, 'ISO 9001 ICA - TechFlow Dev Lab',    1),

-- Audit 7: TechFlow ISO9001 Surveillance Y1 — Sites 4,5
(7, 4, 2, 'AUD-2024-006', '2024-09-02', '2024-09-02', '2024-09-04', '2024-09-04', 'completed', 7, 1, GETDATE(), GETDATE(), 1, 7, 'ISO 9001 Surv Y1 - TechFlow Main', 0),
(7, 5, 2, 'AUD-2024-007', '2024-09-03', '2024-09-03', '2024-09-04', '2024-09-04', 'completed', 7, 1, GETDATE(), GETDATE(), 1, 7, 'ISO 9001 Surv Y1 - TechFlow DC',   0),

-- Audit 8: TechFlow ISO9001 Surveillance Y2 — Sites 4,5 (scheduled)
(8, 4, 2, 'AUD-2025-006', '2025-09-01', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 9001 Surv Y2 - TechFlow Main', 0),
(8, 5, 2, 'AUD-2025-007', '2025-09-02', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 9001 Surv Y2 - TechFlow DC',   0),

-- Audit 9: TechFlow ISO27001 ICA — Sites 4,5
(9, 4, 1, 'AUD-2024-008', '2024-01-12', '2024-01-15', '2024-01-19', '2024-01-19', 'completed', 10, 1, GETDATE(), GETDATE(), 1, 10, 'ISO 27001 ICA - TechFlow Main', 1),
(9, 5, 1, 'AUD-2024-009', '2024-01-12', '2024-01-17', '2024-01-19', '2024-01-19', 'completed', 10, 1, GETDATE(), GETDATE(), 1, 10, 'ISO 27001 ICA - TechFlow DC',   1),

-- Audit 10: TechFlow ISO27001 Surveillance — Sites 4,5
(10, 4, 2, 'AUD-2025-008', '2025-01-10', '2025-01-13', '2025-01-15', '2025-01-15', 'completed', 10, 1, GETDATE(), GETDATE(), 1, 10, 'ISO 27001 Surv - TechFlow Main', 0),
(10, 5, 2, 'AUD-2025-009', '2025-01-10', '2025-01-14', '2025-01-15', '2025-01-15', 'completed', 10, 1, GETDATE(), GETDATE(), 1, 10, 'ISO 27001 Surv - TechFlow DC',   0),

-- Audit 11: Green Energy ISO14001 ICA — Sites 7,8,9
(11, 7, 1, 'AUD-2024-010', '2024-02-10', '2024-02-12', '2024-02-16', '2024-02-16', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 14001 ICA - Green Energy HQ',    1),
(11, 8, 1, 'AUD-2024-011', '2024-02-10', '2024-02-14', '2024-02-16', '2024-02-16', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 14001 ICA - Green Energy WF1',   1),
(11, 9, 1, 'AUD-2024-012', '2024-02-10', '2024-02-15', '2024-02-16', '2024-02-16', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 14001 ICA - Green Energy Solar', 1),

-- Audit 12: Green Energy ISO50001 ICA — Sites 7,8
(12, 7, 1, 'AUD-2024-013', '2024-06-01', '2024-06-03', '2024-06-07', '2024-06-07', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 50001 ICA - Green Energy HQ',  1),
(12, 8, 1, 'AUD-2024-014', '2024-06-01', '2024-06-05', '2024-06-07', '2024-06-07', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 50001 ICA - Green Energy WF1', 1),

-- Audit 13: Green Energy ISO50001 Surveillance — Sites 7,8 (scheduled)
(13, 7, 2, 'AUD-2025-010', '2025-06-01', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 50001 Surv - Green Energy HQ',  0),
(13, 8, 2, 'AUD-2025-011', '2025-06-02', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 50001 Surv - Green Energy WF1', 0),

-- Audit 14: Green Energy ISO50001 Pre-Assessment — Site 7 (scheduled)
(14, 7, 6, 'AUD-2025-012', '2025-10-12', NULL, NULL, NULL, 'scheduled', 8, 1, GETDATE(), GETDATE(), 1, 1, 'ISO 50001 Pre-Assessment - Green Energy HQ', 0),

-- Audit 15: Maritime ISO9001 ICA — Sites 10,11,12
(15, 10, 1, 'AUD-2023-004', '2023-11-04', '2023-11-06', '2023-11-10', '2023-11-10', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 9001 ICA - Maritime Oslo',    1),
(15, 11, 1, 'AUD-2023-005', '2023-11-04', '2023-11-08', '2023-11-10', '2023-11-10', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 9001 ICA - Maritime Shipyard', 1),
(15, 12, 1, 'AUD-2023-006', '2023-11-04', '2023-11-09', '2023-11-10', '2023-11-10', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISO 9001 ICA - Maritime Port',     1),

-- Audit 16: Maritime ISM ICA — Sites 10,11,12
(16, 10, 1, 'AUD-2024-015', '2024-04-06', '2024-04-08', '2024-04-12', '2024-04-12', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISM ICA - Maritime Oslo',    1),
(16, 11, 1, 'AUD-2024-016', '2024-04-06', '2024-04-10', '2024-04-12', '2024-04-12', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISM ICA - Maritime Shipyard', 1),
(16, 12, 1, 'AUD-2024-017', '2024-04-06', '2024-04-11', '2024-04-12', '2024-04-12', 'completed', 6, 1, GETDATE(), GETDATE(), 1, 6, 'ISM ICA - Maritime Port',     1),

-- Audit 17: Maritime ISM Surveillance — Sites 10,11 (scheduled)
(17, 10, 2, 'AUD-2025-013', '2025-04-05', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISM Surv - Maritime Oslo',    0),
(17, 11, 2, 'AUD-2025-014', '2025-04-07', NULL, NULL, NULL, 'scheduled', 7, 1, GETDATE(), GETDATE(), 1, 1, 'ISM Surv - Maritime Shipyard', 0),

-- Audit 18: Maritime ISO45001 ICA — Sites 10,11
(18, 10, 1, 'AUD-2024-018', '2024-08-18', '2024-08-18', '2024-08-22', '2024-08-22', 'completed', 8, 1, GETDATE(), GETDATE(), 1, 8, 'ISO 45001 ICA - Maritime Oslo',    1),
(18, 11, 1, 'AUD-2024-019', '2024-08-18', '2024-08-20', '2024-08-22', '2024-08-22', 'completed', 8, 1, GETDATE(), GETDATE(), 1, 8, 'ISO 45001 ICA - Maritime Shipyard', 1),

-- Audit 19: Food Excellence ISO22000 ICA — Sites 13,14,15
(19, 13, 1, 'AUD-2024-020', '2024-01-22', '2024-01-22', '2024-01-26', '2024-01-26', 'completed', 9, 1, GETDATE(), GETDATE(), 1, 9, 'ISO 22000 ICA - Food Paris HQ',    1),
(19, 14, 1, 'AUD-2024-021', '2024-01-22', '2024-01-24', '2024-01-26', '2024-01-26', 'completed', 9, 1, GETDATE(), GETDATE(), 1, 9, 'ISO 22000 ICA - Food Lyon Prod',   1),
(19, 15, 1, 'AUD-2024-022', '2024-01-22', '2024-01-25', '2024-01-26', '2024-01-26', 'completed', 9, 1, GETDATE(), GETDATE(), 1, 9, 'ISO 22000 ICA - Food Distribution', 1),

-- Audit 20: Food Excellence HACCP ICA — Sites 13,14
(20, 13, 1, 'AUD-2024-023', '2024-03-11', '2024-03-11', '2024-03-15', '2024-03-15', 'completed', 9, 1, GETDATE(), GETDATE(), 1, 9, 'HACCP ICA - Food Paris HQ',  1),
(20, 14, 1, 'AUD-2024-024', '2024-03-11', '2024-03-13', '2024-03-15', '2024-03-15', 'completed', 9, 1, GETDATE(), GETDATE(), 1, 9, 'HACCP ICA - Food Lyon Prod', 1);
