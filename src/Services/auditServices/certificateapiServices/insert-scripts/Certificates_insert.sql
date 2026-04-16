-- Insert sample data for Certificates table
-- This script creates certificate records for completed audits

SET IDENTITY_INSERT [dbo].[Certificates] ON;

INSERT INTO [dbo].[Certificates] 
([CertificateId], [CertificateNumber], [CertificateName], [CompanyId], [SiteId], [ServiceId], [IssueDate], [ExpiryDate], [Status], [CertificateType], [Scope], [IsActive], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [IssuedBy], [RevisionNumber], [PreviousCertificateId], [CertificatePath], [AuditId], [Notes])
VALUES
-- Acme Corporation Certificates
(1, 'DNV-QMS-001-2024', 'ISO 9001:2015 Quality Management System Certificate', 1, NULL, 1, '2024-03-20', '2027-03-20', 'Valid', 'Initial', 'Design, manufacture and supply of industrial equipment and components', 1, '2024-03-20', '2024-03-20', 17, 17, 17, 1, NULL, NULL, 1, NULL),
(2, 'DNV-EMS-001-2024', 'ISO 14001:2015 Environmental Management System Certificate', 1, NULL, 2, '2024-05-25', '2027-05-25', 'Valid', 'Initial', 'Environmental management for industrial manufacturing operations', 1, '2024-05-25', '2024-05-25', 17, 17, 17, 1, NULL, NULL, 3, NULL),

-- TechFlow Industries Certificates
(3, 'DNV-QMS-002-2023', 'ISO 9001:2015 Quality Management System Certificate', 2, NULL, 1, '2023-09-10', '2026-09-10', 'Valid', 'Initial', 'Software development and IT service delivery', 1, '2023-09-10', '2023-09-10', 17, 17, 17, 1, NULL, NULL, 6, NULL),
(4, 'DNV-ISMS-001-2024', 'ISO 27001:2013 Information Security Management Certificate', 2, NULL, 4, '2024-01-20', '2027-01-20', 'Valid', 'Initial', 'Information security management for software development and data center operations', 1, '2024-01-20', '2024-01-20', 17, 17, 17, 1, NULL, NULL, 9, NULL),

-- Green Energy Solutions Certificates
(5, 'DNV-EMS-002-2024', 'ISO 14001:2015 Environmental Management System Certificate', 3, NULL, 2, '2024-02-17', '2027-02-17', 'Valid', 'Initial', 'Environmental management for renewable energy operations', 1, '2024-02-17', '2024-02-17', 17, 17, 17, 1, NULL, NULL, 11, NULL),
(6, 'DNV-WTC-001-2024', 'Wind Turbine Type Certificate', 3, NULL, 23, '2024-06-08', '2029-06-08', 'Valid', 'Initial', 'Type certification for offshore wind turbine model GES-5000', 1, '2024-06-08', '2024-06-08', 17, 17, 17, 1, NULL, NULL, 12, NULL),
(7, 'DNV-SPC-001-2024', 'Solar Panel Installation Certificate', 3, NULL, 24, '2024-08-24', '2027-08-24', 'Valid', 'Initial', 'Solar panel installation certification for utility-scale facility', 1, '2024-08-24', '2024-08-24', 17, 17, 17, 1, NULL, NULL, 14, NULL),

-- Maritime Solutions Certificates
(8, 'DNV-QMS-003-2023', 'ISO 9001:2015 Quality Management System Certificate', 4, NULL, 1, '2023-11-11', '2026-11-11', 'Valid', 'Initial', 'Shipbuilding and marine engineering services', 1, '2023-11-11', '2023-11-11', 17, 17, 17, 1, NULL, NULL, 16, NULL),
(9, 'DNV-SHIP-001-2024', 'DNV Rules for Ships Classification Certificate', 4, NULL, 22, '2024-04-13', '2029-04-13', 'Valid', 'Initial', 'Ship classification and maritime safety certification', 1, '2024-04-13', '2024-04-13', 17, 17, 17, 1, NULL, NULL, 17, NULL),
(10, 'DNV-OHSMS-001-2024', 'ISO 45001:2018 Occupational Health and Safety Certificate', 4, NULL, 3, '2024-09-21', '2027-09-21', 'Valid', 'Initial', 'Occupational health and safety management for maritime operations', 1, '2024-09-21', '2024-09-21', 17, 17, 17, 1, NULL, NULL, 19, NULL),

-- Food Excellence Corp Certificates
(11, 'DNV-FSMS-001-2024', 'ISO 22000:2018 Food Safety Management System Certificate', 5, NULL, 5, '2024-01-27', '2027-01-27', 'Valid', 'Initial', 'Food safety management for food production and distribution', 1, '2024-01-27', '2024-01-27', 17, 17, 17, 1, NULL, NULL, 20, NULL),
(12, 'DNV-HACCP-001-2024', 'HACCP Certification', 5, NULL, 28, '2024-03-16', '2027-03-16', 'Valid', 'Initial', 'Hazard Analysis and Critical Control Points system', 1, '2024-03-16', '2024-03-16', 17, 17, 17, 1, NULL, NULL, 21, NULL),
(13, 'DNV-BRC-001-2024', 'BRC Global Standard for Food Safety Certificate', 5, NULL, 29, '2024-07-06', '2025-07-06', 'Valid', 'Initial', 'BRC Global Standard for Food Safety Grade A', 1, '2024-07-06', '2024-07-06', 17, 17, 17, 1, NULL, NULL, 22, NULL),

-- AutoTech Manufacturing Certificates
(14, 'DNV-QMS-004-2023', 'ISO 9001:2015 Quality Management System Certificate', 6, NULL, 1, '2023-06-17', '2026-06-17', 'Valid', 'Initial', 'Design and manufacture of automotive components and systems', 1, '2023-06-17', '2023-06-17', 17, 17, 17, 1, NULL, NULL, 24, NULL),
(15, 'DNV-IATF-001-2023', 'IATF 16949:2016 Automotive Quality Management Certificate', 6, NULL, 8, '2023-10-28', '2026-10-28', 'Valid', 'Initial', 'Automotive quality management system for component manufacturing', 1, '2023-10-28', '2023-10-28', 17, 17, 17, 1, NULL, NULL, 25, NULL),

-- Global Manufacturing Inc Certificates (Multi-site/Multi-national)
(16, 'DNV-QMS-005-2024', 'ISO 9001:2015 Multi-Site Quality Management System Certificate', 17, NULL, 1, '2024-05-18', '2027-05-18', 'Valid', 'Initial', 'Global manufacturing operations for industrial equipment across multiple sites', 1, '2024-05-18', '2024-05-18', 17, 17, 17, 1, NULL, NULL, 28, NULL),
(17, 'DNV-EMS-003-2024', 'ISO 14001:2015 Multi-Site Environmental Management Certificate', 17, NULL, 2, '2024-08-24', '2027-08-24', 'Valid', 'Initial', 'Global environmental management system for manufacturing operations', 1, '2024-08-24', '2024-08-24', 17, 17, 17, 1, NULL, NULL, 29, NULL),

-- Small Company Certificates
(18, 'DNV-ISMS-002-2024', 'ISO 27001:2013 Information Security Management Certificate', 19, NULL, 4, '2024-11-23', '2027-11-23', 'Valid', 'Initial', 'Information security management for software development services', 1, '2024-11-23', '2024-11-23', 17, 17, 17, 1, NULL, NULL, 31, NULL),
(19, 'DNV-FSMS-002-2025', 'ISO 22000:2018 Food Safety Management System Certificate', 20, NULL, 5, '2025-02-22', '2028-02-22', 'Valid', 'Initial', 'Food safety management for artisanal food production', 1, '2025-02-22', '2025-02-22', 17, 17, 17, 1, NULL, NULL, 32, NULL),

-- Certificates with different statuses for variety
(20, 'DNV-ITSM-001-2025', 'ISO 20000-1:2018 IT Service Management Certificate', 2, NULL, 10, '2025-11-15', '2028-11-15', 'Pending', NULL, 'IT service management and service delivery', 1, '2025-09-15', '2025-09-15', 17, 17, NULL, 1, NULL, NULL, NULL, NULL),
(21, 'DNV-CARBON-001-2025', 'Carbon Footprint Verification Certificate', 3, NULL, 26, '2025-12-06', '2026-12-06', 'Pending', NULL, 'Carbon footprint verification for renewable energy operations', 1, '2025-10-06', '2025-10-06', 17, 17, NULL, 1, NULL, NULL, NULL, NULL),

-- Expired certificate example
(22, 'DNV-QMS-006-2020', 'ISO 9001:2015 Quality Management System Certificate', 13, NULL, 1, '2020-05-15', '2023-05-15', 'Expired', 'Initial', 'Precision manufacturing and engineering services', 0, '2020-05-15', '2023-05-15', 17, 17, 17, 1, NULL, NULL, NULL, NULL),

-- Suspended certificate example
(23, 'DNV-OHSMS-002-2024', 'ISO 45001:2018 Occupational Health and Safety Certificate', 1, NULL, 3, '2024-08-01', '2027-08-01', 'Suspended', 'Initial', 'Occupational health and safety management system', 1, '2024-08-01', '2025-01-15', 17, 17, 17, 1, NULL, NULL, NULL, 'Certificate suspended pending corrective action review');

SET IDENTITY_INSERT [dbo].[Certificates] OFF;

-- Verify the insert
SELECT COUNT(*) as TotalCertificates FROM [dbo].[Certificates];

-- Show certificate status distribution
SELECT 
    Status,
    COUNT(*) as Count
FROM [dbo].[Certificates] 
GROUP BY Status
ORDER BY Count DESC;

-- Show certificates by company
SELECT 
    c.CompanyName,
    COUNT(cert.CertificateId) as CertificateCount,
    COUNT(CASE WHEN cert.Status = 'Valid' THEN 1 END) as ValidCertificates,
    COUNT(CASE WHEN cert.ExpiryDate < GETDATE() THEN 1 END) as ExpiredCertificates
FROM [dbo].[Companies] c
LEFT JOIN [dbo].[Certificates] cert ON c.CompanyId = cert.CompanyId
GROUP BY c.CompanyId, c.CompanyName
HAVING COUNT(cert.CertificateId) > 0
ORDER BY CertificateCount DESC;

-- Show certificates expiring soon (next 6 months)
SELECT 
    cert.CertificateNumber,
    cert.CertificateTitle,
    c.CompanyName,
    s.ServiceName,
    cert.ExpiryDate,
    DATEDIFF(day, GETDATE(), cert.ExpiryDate) as DaysUntilExpiry
FROM [dbo].[Certificates] cert
INNER JOIN [dbo].[Companies] c ON cert.CompanyId = c.CompanyId
INNER JOIN [dbo].[Services] s ON cert.ServiceId = s.ServiceId
WHERE cert.Status = 'Valid' 
AND cert.ExpiryDate BETWEEN GETDATE() AND DATEADD(month, 6, GETDATE())
ORDER BY cert.ExpiryDate;

-- Show certificates by service type
SELECT 
    s.ServiceName,
    COUNT(cert.CertificateId) as CertificateCount,
    COUNT(CASE WHEN cert.Status = 'Valid' THEN 1 END) as ValidCertificates
FROM [dbo].[Services] s
LEFT JOIN [dbo].[Certificates] cert ON s.ServiceId = cert.ServiceId
GROUP BY s.ServiceId, s.ServiceName
HAVING COUNT(cert.CertificateId) > 0
ORDER BY CertificateCount DESC;