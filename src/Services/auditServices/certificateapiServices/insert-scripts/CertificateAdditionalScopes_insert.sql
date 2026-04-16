-- Insert sample data for CertificateAdditionalScopes table
-- Additional scope entries that extend or clarify certificate scope

SET IDENTITY_INSERT [dbo].[CertificateAdditionalScopes] ON;

INSERT INTO [dbo].[CertificateAdditionalScopes]
([CertificateAdditionalScopeId], [CertificateId], [ScopeDescription], [ScopeType], [IsActive], [EffectiveDate], [ExpiryDate], [Notes])
VALUES
-- Certificate 1 (Acme - ISO 9001): extended scope inclusions
(1,  1, 'After-sales service and maintenance of industrial equipment', 'Inclusion', 1, '2024-03-20', NULL, NULL),
(2,  1, 'Supply chain management for raw materials procurement', 'Inclusion', 1, '2024-03-20', NULL, NULL),
(3,  1, 'Research and development activities are excluded from scope', 'Exclusion', 1, '2024-03-20', NULL, NULL),

-- Certificate 2 (Acme - ISO 14001): additional environmental scope
(4,  2, 'Management of hazardous waste arising from manufacturing processes', 'Inclusion', 1, '2024-05-25', NULL, NULL),
(5,  2, 'Energy consumption monitoring and reduction initiatives', 'Inclusion', 1, '2024-05-25', NULL, NULL),

-- Certificate 4 (TechFlow - ISO 27001): security scope extension
(6,  4, 'Cloud infrastructure hosted on third-party platforms', 'Inclusion', 1, '2024-01-20', NULL, NULL),
(7,  4, 'Mobile device management for employee-owned devices', 'Inclusion', 1, '2024-01-20', NULL, NULL),
(8,  4, 'Physical security of off-site backup facilities', 'Exclusion', 1, '2024-01-20', NULL, NULL),

-- Certificate 5 (Green Energy - ISO 14001): renewable energy scope
(9,  5, 'Carbon offset programs and emission trading activities', 'Inclusion', 1, '2024-02-17', NULL, NULL),
(10, 5, 'Decommissioning and disposal of end-of-life renewable equipment', 'Inclusion', 1, '2024-02-17', NULL, NULL),

-- Certificate 11 (Food Excellence - ISO 22000): food safety scope additions
(11, 11, 'Halal food preparation and certification compliance', 'Inclusion', 1, '2024-01-27', NULL, NULL),
(12, 11, 'Cold chain management for temperature-sensitive products', 'Inclusion', 1, '2024-01-27', NULL, NULL),
(13, 11, 'Organic certification activities are excluded', 'Exclusion', 1, '2024-01-27', NULL, NULL),

-- Certificate 12 (Food Excellence - HACCP): HACCP critical control points
(14, 12, 'Incoming raw materials inspection as Critical Control Point', 'Inclusion', 1, '2024-03-16', NULL, NULL),
(15, 12, 'Pasteurisation process as Critical Control Point', 'Inclusion', 1, '2024-03-16', NULL, NULL),
(16, 12, 'Final product microbiological testing as Critical Control Point', 'Inclusion', 1, '2024-03-16', NULL, NULL),

-- Certificate 14 (AutoTech - ISO 9001): automotive scope addenda
(17, 14, 'Customer-specific requirements for OEM clients', 'Inclusion', 1, '2023-06-17', NULL, NULL),
(18, 14, 'Prototype and pre-production development activities', 'Inclusion', 1, '2023-06-17', NULL, NULL),

-- Certificate 15 (AutoTech - IATF 16949): automotive specific requirements
(19, 15, 'IATF Customer Specific Requirements compliance for all OEM customers', 'Inclusion', 1, '2023-10-28', NULL, NULL),
(20, 15, 'Core tool requirements: APQP, PPAP, FMEA, MSA, SPC', 'Inclusion', 1, '2023-10-28', NULL, NULL),
(21, 15, 'Aftermarket parts and accessories are excluded', 'Exclusion', 1, '2023-10-28', NULL, NULL),

-- Certificate 16 (Global Mfg - Multi-site ISO 9001): global scope notes
(22, 16, 'Each site operates under a harmonised global QMS with local adaptations', 'Inclusion', 1, '2024-05-18', NULL, NULL),
(23, 16, 'Joint ventures and minority-owned subsidiaries are excluded', 'Exclusion', 1, '2024-05-18', NULL, NULL),

-- Certificate 18 (Small Tech - ISO 27001): startup-specific scope
(24, 18, 'Software-as-a-service (SaaS) product development and delivery', 'Inclusion', 1, '2024-11-23', NULL, NULL),
(25, 18, 'Customer data processing under applicable data protection regulations', 'Inclusion', 1, '2024-11-23', NULL, NULL),

-- Certificate 23 (Acme - Suspended ISO 45001): suspension notes
(26, 23, 'High-risk confined space entry works suspended pending re-assessment', 'Suspension', 1, '2025-01-15', '2025-04-15', 'Scope suspended following incident investigation'),
(27, 23, 'Working at height activities under additional controls during suspension period', 'Suspension', 1, '2025-01-15', '2025-04-15', 'Enhanced permit-to-work system in place');

SET IDENTITY_INSERT [dbo].[CertificateAdditionalScopes] OFF;

-- Verify
SELECT COUNT(*) AS TotalAdditionalScopes FROM [dbo].[CertificateAdditionalScopes];
SELECT ScopeType, COUNT(*) AS Count FROM [dbo].[CertificateAdditionalScopes] GROUP BY ScopeType ORDER BY Count DESC;
