-- Insert sample data for CertificateServices table
-- Maps certificates to the services they cover (many-to-many)

SET IDENTITY_INSERT [dbo].[CertificateServices] ON;

INSERT INTO [dbo].[CertificateServices]
([CertificateServiceId], [CertificateId], [ServiceId], [IsActive], [Scope], [Notes])
VALUES
-- Certificate 1 (Acme - ISO 9001) covers Quality Management service
(1,  1,  1, 1, 'Design, manufacture and supply of industrial equipment and components', NULL),

-- Certificate 2 (Acme - ISO 14001) covers Environmental Management service
(2,  2,  2, 1, 'Environmental management for industrial manufacturing operations', NULL),

-- Certificate 3 (TechFlow - ISO 9001) covers Quality Management service
(3,  3,  1, 1, 'Software development and IT service delivery', NULL),

-- Certificate 4 (TechFlow - ISO 27001) covers Information Security service
(4,  4,  4, 1, 'Information security management for software development and data center operations', NULL),

-- Certificate 5 (Green Energy - ISO 14001) covers Environmental Management service
(5,  5,  2, 1, 'Environmental management for renewable energy operations', NULL),

-- Certificate 6 (Green Energy - Wind Turbine) covers Wind Turbine service
(6,  6, 23, 1, 'Type certification for offshore wind turbine model GES-5000', NULL),

-- Certificate 7 (Green Energy - Solar) covers Solar Panel service
(7,  7, 24, 1, 'Solar panel installation certification for utility-scale facility', NULL),

-- Certificate 8 (Maritime - ISO 9001) covers Quality Management service
(8,  8,  1, 1, 'Shipbuilding and marine engineering services', NULL),

-- Certificate 9 (Maritime - DNV Ships) covers Ship Classification service
(9,  9, 22, 1, 'Ship classification and maritime safety certification', NULL),

-- Certificate 10 (Maritime - ISO 45001) covers OH&S service
(10, 10,  3, 1, 'Occupational health and safety management for maritime operations', NULL),

-- Certificate 11 (Food Excellence - ISO 22000) covers Food Safety service
(11, 11,  5, 1, 'Food safety management for food production and distribution', NULL),

-- Certificate 12 (Food Excellence - HACCP)
(12, 12, 28, 1, 'Hazard Analysis and Critical Control Points system', NULL),

-- Certificate 13 (Food Excellence - BRC)
(13, 13, 29, 1, 'BRC Global Standard for Food Safety Grade A', NULL),

-- Certificate 14 (AutoTech - ISO 9001) covers Quality Management service
(14, 14,  1, 1, 'Design and manufacture of automotive components and systems', NULL),

-- Certificate 15 (AutoTech - IATF 16949) covers Automotive Quality service
(15, 15,  8, 1, 'Automotive quality management system for component manufacturing', NULL),

-- Certificate 16 (Global Mfg - ISO 9001) covers Quality Management service
(16, 16,  1, 1, 'Global manufacturing operations for industrial equipment across multiple sites', NULL),

-- Certificate 17 (Global Mfg - ISO 14001) covers Environmental Management service
(17, 17,  2, 1, 'Global environmental management system for manufacturing operations', NULL),

-- Certificate 18 (Small Tech - ISO 27001) covers Information Security service
(18, 18,  4, 1, 'Information security management for software development services', NULL),

-- Certificate 19 (Family Food - ISO 22000) covers Food Safety service
(19, 19,  5, 1, 'Food safety management for artisanal food production', NULL),

-- Certificate 20 (TechFlow - ISO 20000) - Pending
(20, 20, 10, 1, 'IT service management and service delivery', NULL),

-- Certificate 21 (Green Energy - Carbon) - Pending
(21, 21, 26, 1, 'Carbon footprint verification for renewable energy operations', NULL),

-- Certificate 22 (Expired ISO 9001)
(22, 22,  1, 0, 'Precision manufacturing and engineering services', 'Expired - last surveillance completed 2022'),

-- Certificate 23 (Acme - Suspended ISO 45001)
(23, 23,  3, 1, 'Occupational health and safety management system', 'Suspended pending corrective action');

SET IDENTITY_INSERT [dbo].[CertificateServices] OFF;

-- Verify
SELECT COUNT(*) AS TotalCertificateServices FROM [dbo].[CertificateServices];
