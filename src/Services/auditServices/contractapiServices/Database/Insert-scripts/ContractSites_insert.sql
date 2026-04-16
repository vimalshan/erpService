-- Insert sample data for ContractSites table

SET IDENTITY_INSERT [dbo].[ContractSites] ON;

INSERT INTO [dbo].[ContractSites]
([ContractSiteId], [ContractId], [SiteId], [IsActive], [CreatedDate], [ModifiedDate],
 [CreatedBy], [ModifiedBy], [StartDate], [EndDate], [Status])
VALUES
-- Contract 1 (ISO 9001 - Acme, CompanyId=2) → Manufacturing Plant (SiteId=2)
(1,  1, 2, 1, '2024-01-10', '2024-01-10', 1, 1, '2024-01-15', '2027-01-14', 'Active'),

-- Contract 2 (ISO 14001 - Acme) → Manufacturing Plant (SiteId=2)
(2,  2, 2, 1, '2024-03-20', '2024-03-20', 1, 1, '2024-04-01', '2027-03-31', 'Active'),

-- Contract 3 (Safety Consulting - Acme) → Manufacturing Plant (SiteId=2)
(3,  3, 2, 1, '2024-12-15', '2024-12-15', 1, 1, '2025-01-01', '2025-12-31', 'Active'),

-- Contract 4 (Integrated Cert - Global, CompanyId=3) → Research Center (SiteId=3)
(4,  4, 3, 1, '2023-05-15', '2023-05-15', 1, 1, '2023-06-01', '2026-05-31', 'Active'),

-- Contract 5 (Energy Audit - Global) → Research Center (SiteId=3)
(5,  5, 3, 1, '2024-06-20', '2025-07-01', 1, 1, '2024-07-01', '2025-06-30', 'Completed'),

-- Contract 6 (ISO 50001 - Global) → Research Center (SiteId=3)
(6,  6, 3, 1, '2025-02-15', '2025-02-15', 1, 1, '2025-03-01', '2028-02-28', 'Active'),

-- Contract 7 (Training - DNV, CompanyId=1) → Headquarters (SiteId=1)
(7,  7, 1, 1, '2024-08-20', '2024-08-20', 1, 1, '2024-09-01', '2025-08-31', 'Active'),

-- Contract 8 (QA Framework - DNV) → Headquarters (SiteId=1)
(8,  8, 1, 1, '2025-01-20', '2025-01-20', 1, 1, '2025-02-01', '2026-01-31', 'Active'),

-- Contract 9 (Expired Pilot - Acme) → Manufacturing Plant (SiteId=2)
(9,  9, 2, 1, '2022-12-10', '2024-01-05', 1, 1, '2023-01-01', '2023-12-31', 'Completed'),

-- Contract 10 (Draft Renewal - Global) → Research Center (SiteId=3)
(10, 10, 3, 1, '2025-04-01', '2025-04-01', 1, 1, '2026-06-01', '2029-05-31', 'Active');

SET IDENTITY_INSERT [dbo].[ContractSites] OFF;
