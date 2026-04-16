-- Insert sample data for ContractServices table

SET IDENTITY_INSERT [dbo].[ContractServices] ON;

INSERT INTO [dbo].[ContractServices]
([ContractServiceId], [ContractId], [ServiceId], [Quantity], [UnitPrice], [TotalPrice], [Currency],
 [IsActive], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [StartDate], [EndDate], [Status])
VALUES
-- Contract 1 (ISO 9001 - Acme) → Quality Management (3)
(1,  1, 3, 1, 45000.00, 45000.00, 'USD', 1, '2024-01-10', '2024-01-10', 1, 1, '2024-01-15', '2027-01-14', 'Active'),

-- Contract 2 (ISO 14001 - Acme) → Environmental Management (1)
(2,  2, 1, 1, 38000.00, 38000.00, 'USD', 1, '2024-03-20', '2024-03-20', 1, 1, '2024-04-01', '2027-03-31', 'Active'),

-- Contract 3 (Safety Consulting - Acme) → Occupational Health & Safety (2)
(3,  3, 2, 1, 22000.00, 22000.00, 'USD', 1, '2024-12-15', '2024-12-15', 1, 1, '2025-01-01', '2025-12-31', 'Active'),

-- Contract 4 (Integrated Cert - Global) → Quality Management (3) + Environmental (1)
(4,  4, 3, 1, 40000.00, 40000.00, 'EUR', 1, '2023-05-15', '2023-05-15', 1, 1, '2023-06-01', '2026-05-31', 'Active'),
(5,  4, 1, 1, 35000.00, 35000.00, 'EUR', 1, '2023-05-15', '2023-05-15', 1, 1, '2023-06-01', '2026-05-31', 'Active'),

-- Contract 5 (Energy Audit - Global) → Energy Management (4)
(6,  5, 4, 1, 18000.00, 18000.00, 'EUR', 1, '2024-06-20', '2025-07-01', 1, 1, '2024-07-01', '2025-06-30', 'Completed'),

-- Contract 6 (ISO 50001 - Global) → Energy Management (4)
(7,  6, 4, 1, 42000.00, 42000.00, 'EUR', 1, '2025-02-15', '2025-02-15', 1, 1, '2025-03-01', '2028-02-28', 'Active'),

-- Contract 7 (Training - DNV) → Occupational Health & Safety (2)
(8,  7, 2, 3, 10000.00, 30000.00, 'USD', 1, '2024-08-20', '2024-08-20', 1, 1, '2024-09-01', '2025-08-31', 'Active'),

-- Contract 8 (QA Framework - DNV) → Quality Management (3)
(9,  8, 3, 1, 55000.00, 55000.00, 'USD', 1, '2025-01-20', '2025-01-20', 1, 1, '2025-02-01', '2026-01-31', 'Active'),

-- Contract 9 (Expired Pilot - Acme) → Energy Management (4)
(10, 9, 4, 1, 12000.00, 12000.00, 'USD', 1, '2022-12-10', '2024-01-05', 1, 1, '2023-01-01', '2023-12-31', 'Completed'),

-- Contract 10 (Draft Renewal - Global) → Quality Management (3) + Environmental (1)
(11, 10, 3, 1, 45000.00, 45000.00, 'EUR', 1, '2025-04-01', '2025-04-01', 1, 1, '2026-06-01', '2029-05-31', 'Active'),
(12, 10, 1, 1, 35000.00, 35000.00, 'EUR', 1, '2025-04-01', '2025-04-01', 1, 1, '2026-06-01', '2029-05-31', 'Active');

SET IDENTITY_INSERT [dbo].[ContractServices] OFF;
