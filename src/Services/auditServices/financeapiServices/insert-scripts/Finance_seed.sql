-- ============================================================
-- Finance Service Seed Data
-- Invoices + Financials matching current DB state:
--   Companies: 1=DNV AS, 2=Acme Corporation, 3=Global Industries
--   Contracts: 1-10 (see Contracts table)
--   Users: 1=admin
-- ============================================================

-- ============================================================
-- Invoices
-- ============================================================
SET IDENTITY_INSERT [dbo].[Invoices] ON;

INSERT INTO [dbo].[Invoices]
([InvoiceId],[InvoiceNumber],[CompanyId],[ContractId],[InvoiceDate],[DueDate],[PlannedPaymentDate],[PaidDate],
 [Amount],[TaxAmount],[TotalAmount],[Currency],[Status],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[Description],[PaymentMethod])
VALUES
-- Acme Corporation (CompanyId=2) - contracts 1,2,3
(1, 'INV-2024-001', 2, 1, '2024-03-20', '2024-04-19', '2024-04-19', '2024-04-15', 15000.00, 2250.00, 17250.00, 'USD', 'Paid',    1, '2024-03-20', '2024-04-15', 1, 1, 'ISO 9001:2015 Initial Certification Audit', 'Bank Transfer'),
(2, 'INV-2024-002', 2, 2, '2024-05-25', '2024-06-24', '2024-06-24', '2024-06-20', 16000.00, 2400.00, 18400.00, 'USD', 'Paid',    1, '2024-05-25', '2024-06-20', 1, 1, 'ISO 14001:2015 Initial Certification Audit', 'Bank Transfer'),
(3, 'INV-2025-001', 2, 1, '2025-03-13', '2025-04-12', '2025-04-12', '2025-04-08',  8000.00, 1200.00,  9200.00, 'USD', 'Paid',    1, '2025-03-13', '2025-04-08', 1, 1, 'ISO 9001:2015 Surveillance Year 1',          'Credit Card'),
(4, 'INV-2025-002', 2, 2, '2025-05-18', '2025-06-17', '2025-06-17', NULL,           8500.00, 1275.00,  9775.00, 'USD', 'Pending', 1, '2025-05-18', '2025-05-18', 1, 1, 'ISO 14001:2015 Surveillance Year 1',          NULL),
(5, 'INV-2025-003', 2, 3, '2025-07-12', '2025-08-11', '2025-08-11', NULL,           7000.00, 1050.00,  8050.00, 'USD', 'Pending', 1, '2025-07-12', '2025-07-12', 1, 1, 'Safety Management Consulting - Phase 1',      NULL),

-- Global Industries (CompanyId=3) - contracts 4,5,6
(6,  'INV-2024-003', 3, 4, '2024-02-17', '2024-03-19', '2024-03-19', '2024-03-15', 17000.00, 3230.00, 20230.00, 'EUR', 'Paid',    1, '2024-02-17', '2024-03-15', 1, 1, 'ISO 9001 & 14001 Integrated Certification',   'Bank Transfer'),
(7,  'INV-2024-004', 3, 5, '2024-06-08', '2024-07-08', '2024-07-08', '2024-07-02', 35000.00, 6650.00, 41650.00, 'EUR', 'Paid',    1, '2024-06-08', '2024-07-02', 1, 1, 'Energy Audit Services - Initial Assessment',  'Bank Transfer'),
(8,  'INV-2024-005', 3, 6, '2024-08-24', '2024-09-23', '2024-09-23', '2024-09-18', 25000.00, 4750.00, 29750.00, 'EUR', 'Paid',    1, '2024-08-24', '2024-09-18', 1, 1, 'ISO 50001 Initial Certification',             'Bank Transfer'),
(9,  'INV-2025-004', 3, 4, '2025-06-05', '2025-07-05', '2025-07-05', NULL,          15000.00, 2850.00, 17850.00, 'EUR', 'Overdue', 1, '2025-06-05', '2025-06-05', 1, 1, 'ISO 9001 & 14001 Surveillance Audit',         NULL),
(10, 'INV-2025-005', 3, 5, '2025-10-16', '2025-11-15', '2025-11-15', NULL,           8000.00, 1520.00,  9520.00, 'EUR', 'Pending', 1, '2025-10-16', '2025-10-16', 1, 1, 'Energy Audit Services - Annual Review',       NULL),

-- DNV AS (CompanyId=1) - contracts 7,8
(11, 'INV-2023-001', 1, 7, '2023-11-11', '2023-12-11', '2023-12-11', '2023-12-05', 22000.00, 5500.00, 27500.00, 'USD', 'Paid',    1, '2023-11-11', '2023-12-05', 1, 1, 'Training Services Agreement - Initial',       'Bank Transfer'),
(12, 'INV-2024-006', 1, 8, '2024-04-13', '2024-05-13', '2024-05-13', '2024-05-08', 45000.00, 11250.00,56250.00, 'USD', 'Paid',    1, '2024-04-13', '2024-05-08', 1, 1, 'Quality Assurance Framework - Setup',         'Bank Transfer'),
(13, 'INV-2024-007', 1, 7, '2024-09-21', '2024-10-21', '2024-10-21', '2024-10-15', 19000.00, 4750.00, 23750.00, 'USD', 'Paid',    1, '2024-09-21', '2024-10-15', 1, 1, 'Training Services - Year 2',                  'Credit Card'),
(14, 'INV-2025-006', 1, 8, '2025-04-10', '2025-05-10', '2025-05-10', NULL,          18000.00, 4500.00, 22500.00, 'USD', 'Pending', 1, '2025-04-10', '2025-04-10', 1, 1, 'Quality Assurance Framework - Annual Review', NULL),
(15, 'INV-2025-007', 2, 9, '2025-05-15', '2025-06-14', '2025-06-14', NULL,          12000.00, 2400.00, 14400.00, 'USD', 'Overdue', 1, '2025-05-15', '2025-05-15', 1, 1, 'Expired Pilot Contract - Final Invoice',      NULL);

SET IDENTITY_INSERT [dbo].[Invoices] OFF;
GO

-- ============================================================
-- Financials
-- ============================================================
SET IDENTITY_INSERT [dbo].[Financials] ON;

INSERT INTO [dbo].[Financials]
([FinancialId],[CompanyId],[Year],[Quarter],[Month],[Revenue],[Expenses],[Profit],[OutstandingAmount],[PaidAmount],[OverdueAmount],[Currency],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy])
VALUES
-- Acme Corporation (CompanyId=2)
(1,  2, 2024, 1, NULL,  17250.00,  5000.00, 12250.00,     0.00,  17250.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(2,  2, 2024, 2, NULL,  18400.00,  5500.00, 12900.00,     0.00,  18400.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(3,  2, 2025, 1, NULL,   9200.00,  2500.00,  6700.00,     0.00,   9200.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(4,  2, 2025, 2, NULL,  26225.00,  6500.00, 19725.00, 26225.00,      0.00,  14400.00,'USD', 1, GETDATE(), GETDATE(), 1, 1),
-- Global Industries (CompanyId=3)
(5,  3, 2024, 1, NULL,  20230.00,  6000.00, 14230.00,     0.00,  20230.00,     0.00, 'EUR', 1, GETDATE(), GETDATE(), 1, 1),
(6,  3, 2024, 2, NULL,  71400.00, 20000.00, 51400.00,     0.00,  71400.00,     0.00, 'EUR', 1, GETDATE(), GETDATE(), 1, 1),
(7,  3, 2025, 2, NULL,  17850.00,  5000.00, 12850.00, 17850.00,      0.00,  17850.00,'EUR', 1, GETDATE(), GETDATE(), 1, 1),
(8,  3, 2025, 4, NULL,   9520.00,  2800.00,  6720.00,  9520.00,      0.00,     0.00, 'EUR', 1, GETDATE(), GETDATE(), 1, 1),
-- DNV AS (CompanyId=1)
(9,  1, 2023, 4, NULL,  27500.00,  8000.00, 19500.00,     0.00,  27500.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(10, 1, 2024, 2, NULL,  56250.00, 15000.00, 41250.00,     0.00,  56250.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(11, 1, 2024, 3, NULL,  23750.00,  7000.00, 16750.00,     0.00,  23750.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1),
(12, 1, 2025, 2, NULL,  22500.00,  6500.00, 16000.00, 22500.00,      0.00,     0.00, 'USD', 1, GETDATE(), GETDATE(), 1, 1);

SET IDENTITY_INSERT [dbo].[Financials] OFF;
GO
