-- Insert sample data for Audits table
-- This script creates 20 sample audits covering 5 customer companies

INSERT INTO [dbo].[Audits]
    ([auditId], [sites], [services], [companyId], [status], [startDate], [endDate], [leadAuditor], [type])
VALUES
-- Acme Corporation (companyId=1) — Sites 1,2,3
(1,  '1,2,3', 'ISO9001',  1, 'Completed', '2024-03-10', '2024-03-19', 'Michael Brown',    'Initial Certification'),
(2,  '1,2',   'ISO9001',  1, 'Completed', '2025-03-08', '2025-03-12', 'Robert Anderson',  'Surveillance'),
(3,  '1,2',   'ISO14001', 1, 'Completed', '2024-05-18', '2024-05-24', 'Michael Brown',    'Initial Certification'),
(4,  '1,2',   'ISO14001', 1, 'Scheduled', '2025-05-12', '2025-05-17', 'Robert Anderson',  'Surveillance'),
(5,  '1',     'ISO45001', 1, 'Scheduled', '2025-07-07', '2025-07-10', 'Elena Martinez',   'Pre-Assessment'),

-- TechFlow Industries (companyId=2) — Sites 4,5,6
(6,  '4,5,6', 'ISO9001',  2, 'Completed', '2023-09-01', '2023-09-09', 'Sarah Wilson',     'Initial Certification'),
(7,  '4,5',   'ISO9001',  2, 'Completed', '2024-08-31', '2024-09-04', 'Robert Anderson',  'Surveillance'),
(8,  '4,5',   'ISO9001',  2, 'Scheduled', '2025-08-30', '2025-09-03', 'Robert Anderson',  'Surveillance'),
(9,  '4,5',   'ISO27001', 2, 'Completed', '2024-01-12', '2024-01-19', 'Kevin Lee',        'Initial Certification'),
(10, '4,5',   'ISO27001', 2, 'Completed', '2025-01-10', '2025-01-15', 'Kevin Lee',        'Surveillance'),

-- Green Energy Solutions (companyId=3) — Sites 7,8,9
(11, '7,8,9', 'ISO14001', 3, 'Completed', '2024-02-10', '2024-02-16', 'Luis Garcia',      'Initial Certification'),
(12, '7,8',   'ISO50001', 3, 'Completed', '2024-06-01', '2024-06-07', 'Luis Garcia',      'Initial Certification'),
(13, '7,8',   'ISO50001', 3, 'Scheduled', '2025-06-01', '2025-06-03', 'Robert Anderson',  'Surveillance'),
(14, '7',     'ISO50001', 3, 'Scheduled', '2025-10-12', '2025-10-16', 'Elena Martinez',   'Pre-Assessment'),

-- Maritime Solutions (companyId=4) — Sites 10,11,12
(15, '10,11,12', 'ISO9001',  4, 'Completed', '2023-11-04', '2023-11-10', 'Luis Garcia',   'Initial Certification'),
(16, '10,11,12', 'ISM',      4, 'Completed', '2024-04-06', '2024-04-12', 'Luis Garcia',   'Initial Certification'),
(17, '10,11',    'ISM',      4, 'Scheduled', '2025-04-05', '2025-04-08', 'Robert Anderson','Surveillance'),
(18, '10,11',    'ISO45001', 4, 'Completed', '2024-08-18', '2024-08-22', 'Elena Martinez', 'Initial Certification'),

-- Food Excellence Corp (companyId=5) — Sites 13,14,15
(19, '13,14,15', 'ISO22000', 5, 'Completed', '2024-01-22', '2024-01-26', 'Hiroshi Tanaka', 'Initial Certification'),
(20, '13,14',    'HACCP',    5, 'Completed', '2024-03-11', '2024-03-15', 'Hiroshi Tanaka', 'Initial Certification');