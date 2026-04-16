-- Insert sample data for AuditTeamMembers table
-- DNV Staff: 2=John Smith, 3=Anna Johnson, 4=Michael Brown, 5=Sarah Wilson,
--            6=Luis Garcia, 7=Robert Anderson, 8=Elena Martinez,
--            9=Hiroshi Tanaka, 10=Kevin Lee
-- UNIQUE constraint: (AuditId, UserId) — each user appears once per audit

INSERT INTO [dbo].[AuditTeamMembers]
    ([AuditId], [UserId], [Role], [AssignedDate], [IsActive], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy])
VALUES
-- Audit 1: Acme ISO9001 ICA
(1,  4, 'Lead Auditor',       '2024-02-15', 1, '2024-02-15', '2024-03-19', 4, 4),
(1,  7, 'Auditor',            '2024-02-15', 1, '2024-02-15', '2024-03-19', 4, 7),
(1,  3, 'Audit Coordinator',  '2024-02-15', 1, '2024-02-15', '2024-03-19', 4, 3),

-- Audit 2: Acme ISO9001 Surveillance Y1
(2,  7, 'Lead Auditor',       '2025-02-10', 1, '2025-02-10', '2025-03-12', 7, 7),
(2,  2, 'Auditor',            '2025-02-10', 1, '2025-02-10', '2025-03-12', 7, 2),

-- Audit 3: Acme ISO14001 ICA
(3,  4, 'Lead Auditor',       '2024-04-20', 1, '2024-04-20', '2024-05-24', 4, 4),
(3,  8, 'Technical Specialist','2024-04-20', 1, '2024-04-20', '2024-05-24', 4, 8),

-- Audit 4: Acme ISO14001 Surveillance Y1
(4,  7, 'Lead Auditor',       '2025-04-15', 1, '2025-04-15', NULL, 7, NULL),
(4,  8, 'Technical Specialist','2025-04-15', 1, '2025-04-15', NULL, 7, NULL),

-- Audit 5: Acme ISO45001 Pre-Assessment
(5,  8, 'Lead Auditor',       '2025-06-10', 1, '2025-06-10', NULL, 8, NULL),

-- Audit 6: TechFlow ISO9001 ICA
(6,  5, 'Lead Auditor',       '2023-08-05', 1, '2023-08-05', '2023-09-09', 5, 5),
(6,  3, 'Auditor',            '2023-08-05', 1, '2023-08-05', '2023-09-09', 5, 3),
(6,  2, 'Audit Coordinator',  '2023-08-05', 1, '2023-08-05', '2023-09-09', 5, 2),

-- Audit 7: TechFlow ISO9001 Surveillance Y1
(7,  7, 'Lead Auditor',       '2024-08-02', 1, '2024-08-02', '2024-09-04', 7, 7),

-- Audit 8: TechFlow ISO9001 Surveillance Y2
(8,  7, 'Lead Auditor',       '2025-08-01', 1, '2025-08-01', NULL, 7, NULL),

-- Audit 9: TechFlow ISO27001 ICA
(9,  10, 'Lead Auditor',             '2023-12-15', 1, '2023-12-15', '2024-01-19', 10, 10),
(9,  3,  'IT Security Specialist',   '2023-12-15', 1, '2023-12-15', '2024-01-19', 10, 3),

-- Audit 10: TechFlow ISO27001 Surveillance
(10, 10, 'Lead Auditor',             '2024-12-13', 1, '2024-12-13', '2025-01-15', 10, 10),

-- Audit 11: Green Energy ISO14001 ICA
(11, 6,  'Lead Auditor',             '2024-01-12', 1, '2024-01-12', '2024-02-16', 6, 6),
(11, 8,  'Environmental Specialist', '2024-01-12', 1, '2024-01-12', '2024-02-16', 6, 8),

-- Audit 12: Green Energy ISO50001 ICA
(12, 6,  'Lead Auditor',             '2024-05-03', 1, '2024-05-03', '2024-06-07', 6, 6),
(12, 9,  'Energy Specialist',        '2024-05-03', 1, '2024-05-03', '2024-06-07', 6, 9),
(12, 8,  'Technical Auditor',        '2024-05-03', 1, '2024-05-03', '2024-06-07', 6, 8),

-- Audit 13: Green Energy ISO50001 Surveillance
(13, 9,  'Lead Auditor',             '2025-05-02', 1, '2025-05-02', NULL, 9, NULL),
(13, 6,  'Technical Review',         '2025-05-02', 1, '2025-05-02', NULL, 9, NULL),

-- Audit 14: Green Energy ISO50001 Pre-Assessment
(14, 8,  'Lead Auditor',             '2025-09-14', 1, '2025-09-14', NULL, 8, NULL),

-- Audit 15: Maritime ISO9001 ICA
(15, 6,  'Lead Auditor',             '2023-10-06', 1, '2023-10-06', '2023-11-10', 6, 6),
(15, 7,  'Maritime Specialist',      '2023-10-06', 1, '2023-10-06', '2023-11-10', 6, 7),
(15, 2,  'Quality Auditor',          '2023-10-06', 1, '2023-10-06', '2023-11-10', 6, 2),

-- Audit 16: Maritime ISM ICA
(16, 6,  'Lead Auditor',                    '2024-03-08', 1, '2024-03-08', '2024-04-12', 6, 6),
(16, 7,  'Ship Classification Specialist',  '2024-03-08', 1, '2024-03-08', '2024-04-12', 6, 7),
(16, 3,  'Marine Engineering Auditor',      '2024-03-08', 1, '2024-03-08', '2024-04-12', 6, 3),

-- Audit 17: Maritime ISM Surveillance
(17, 7,  'Lead Auditor',             '2025-03-07', 1, '2025-03-07', NULL, 7, NULL),
(17, 6,  'Technical Review',         '2025-03-07', 1, '2025-03-07', NULL, 7, NULL),

-- Audit 18: Maritime ISO45001 ICA
(18, 8,  'Lead Auditor',             '2024-08-16', 1, '2024-08-16', '2024-08-22', 8, 8),
(18, 3,  'Safety Specialist',        '2024-08-16', 1, '2024-08-16', '2024-08-22', 8, 3),

-- Audit 19: Food Excellence ISO22000 ICA
(19, 9,  'Lead Auditor',             '2024-01-10', 1, '2024-01-10', '2024-01-26', 9, 9),
(19, 5,  'Food Safety Specialist',   '2024-01-10', 1, '2024-01-10', '2024-01-26', 9, 5),

-- Audit 20: Food Excellence HACCP ICA
(20, 9,  'Lead Auditor',             '2024-02-20', 1, '2024-02-20', '2024-03-15', 9, 9),
(20, 5,  'HACCP Specialist',         '2024-02-20', 1, '2024-02-20', '2024-03-15', 9, 5);
