-- Insert sample data for Contracts table

SET IDENTITY_INSERT [dbo].[Contracts] ON;

INSERT INTO [dbo].[Contracts]
([ContractId], [ContractNumber], [ContractName], [CompanyId], [ContractType], [StartDate], [EndDate],
 [Status], [TotalValue], [Currency], [IsActive], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy],
 [SignedDate], [SignedByClient], [SignedByDNV], [Terms], [Notes], [AutoRenewal])
VALUES
-- Acme Corporation (CompanyId=2) Contracts
(1,  'CNT-2024-001', 'ISO 9001 Certification Agreement',      2, 'Certification', '2024-01-15', '2027-01-14', 'Active',    45000.00, 'USD', 1, '2024-01-10', '2024-01-15', 1, 1, '2024-01-15', 'John Smith',    'DNV Certifier', 'Standard certification terms apply', 'Annual surveillance required', 0),
(2,  'CNT-2024-002', 'ISO 14001 Environmental Contract',      2, 'Certification', '2024-04-01', '2027-03-31', 'Active',    38000.00, 'USD', 1, '2024-03-20', '2024-04-01', 1, 1, '2024-04-01', 'John Smith',    'DNV Certifier', 'Environmental standard terms',          NULL, 0),
(3,  'CNT-2025-001', 'Safety Management Consulting',          2, 'Consulting',    '2025-01-01', '2025-12-31', 'Active',    22000.00, 'USD', 1, '2024-12-15', '2025-01-01', 1, 1, '2025-01-01', 'John Smith',    'DNV Advisor',   'Consulting agreement terms',            'Quarterly reviews', 0),

-- Global Industries (CompanyId=3) Contracts
(4,  'CNT-2023-001', 'ISO 9001 & 14001 Integrated Cert',      3, 'Certification', '2023-06-01', '2026-05-31', 'Active',    75000.00, 'EUR', 1, '2023-05-15', '2023-06-01', 1, 1, '2023-06-01', 'Maria Garcia',  'DNV Certifier', 'Integrated management system terms',    NULL, 1),
(5,  'CNT-2024-003', 'Energy Audit Services',                 3, 'Assessment',    '2024-07-01', '2025-06-30', 'Completed', 18000.00, 'EUR', 1, '2024-06-20', '2025-07-01', 1, 1, '2024-07-01', 'Maria Garcia',  'DNV Assessor',  'Energy audit agreement terms',          'Completed on schedule', 0),
(6,  'CNT-2025-002', 'ISO 50001 Certification',               3, 'Certification', '2025-03-01', '2028-02-28', 'Active',    42000.00, 'EUR', 1, '2025-02-15', '2025-03-01', 1, 1, '2025-03-01', 'Maria Garcia',  'DNV Certifier', 'Energy management certification terms', NULL, 0),

-- DNV AS (CompanyId=1) Internal Contracts
(7,  'CNT-2024-004', 'Training Services Agreement',           1, 'Training',      '2024-09-01', '2025-08-31', 'Active',    30000.00, 'USD', 1, '2024-08-20', '2024-09-01', 1, 1, '2024-09-01', 'Admin User',    'DNV Trainer',   'Training services terms',               NULL, 1),
(8,  'CNT-2025-003', 'Quality Assurance Framework Contract',  1, 'Assessment',    '2025-02-01', '2026-01-31', 'Active',    55000.00, 'USD', 1, '2025-01-20', '2025-02-01', 1, 1, '2025-02-01', 'Admin User',    'DNV Assessor',  'Quality framework assessment terms',    'Bi-annual reviews', 0),
(9,  'CNT-2024-005', 'Expired Pilot Contract',                2, 'Consulting',    '2023-01-01', '2023-12-31', 'Completed', 12000.00, 'USD', 1, '2022-12-10', '2024-01-05', 1, 1, '2023-01-01', 'John Smith',    'DNV Advisor',   'Pilot consulting agreement',            'Completed successfully', 0),
(10, 'CNT-2025-004', 'Draft Renewal Agreement',               3, 'Certification', '2026-06-01', '2029-05-31', 'Draft',     80000.00, 'EUR', 1, '2025-04-01', '2025-04-01', 1, 1, NULL,         NULL,            NULL,            'Renewal subject to review',             'Pending signature', 1);

SET IDENTITY_INSERT [dbo].[Contracts] OFF;
