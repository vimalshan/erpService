-- Insert sample data for CertificateSites table
-- Maps certificates to the sites they cover (many-to-many)

SET IDENTITY_INSERT [dbo].[CertificateSites] ON;

INSERT INTO [dbo].[CertificateSites]
([CertificateSiteId], [CertificateId], [SiteId], [IsActive], [Scope], [Notes])
VALUES
-- Certificate 1 (Acme - ISO 9001): covers 3 Acme sites
(1,  1,  1, 1, 'Quality management for headquarters operations', NULL),
(2,  1,  2, 1, 'Quality management for manufacturing plant', NULL),
(3,  1,  3, 1, 'Quality management for warehouse operations', NULL),

-- Certificate 2 (Acme - ISO 14001): covers 2 Acme sites
(4,  2,  1, 1, 'Environmental management for headquarters', NULL),
(5,  2,  2, 1, 'Environmental management for manufacturing plant', NULL),

-- Certificate 3 (TechFlow - ISO 9001): covers 3 TechFlow sites
(6,  3,  4, 1, 'Quality management for main office', NULL),
(7,  3,  5, 1, 'Quality management for data center', NULL),
(8,  3,  6, 1, 'Quality management for development lab', NULL),

-- Certificate 4 (TechFlow - ISO 27001): covers 2 TechFlow sites
(9,  4,  4, 1, 'Information security for main office', NULL),
(10, 4,  5, 1, 'Information security for data center', NULL),

-- Certificate 5 (Green Energy - ISO 14001): covers 3 Green Energy sites
(11, 5,  7, 1, 'Environmental management for Berlin HQ', NULL),
(12, 5,  8, 1, 'Environmental management for wind farm', NULL),
(13, 5,  9, 1, 'Environmental management for solar installation', NULL),

-- Certificate 6 (Green Energy - Wind Turbine): covers wind farm site
(14, 6,  8, 1, 'Type certification scope for offshore wind turbine model GES-5000', NULL),

-- Certificate 7 (Green Energy - Solar): covers solar site
(15, 7,  9, 1, 'Solar panel installation certification scope', NULL),

-- Certificate 8 (Maritime - ISO 9001): covers 3 Maritime sites
(16, 8, 10, 1, 'Quality management for Oslo office', NULL),
(17, 8, 11, 1, 'Quality management for Bergen shipyard', NULL),
(18, 8, 12, 1, 'Quality management for port facility', NULL),

-- Certificate 9 (Maritime - DNV Ships): covers 2 Maritime sites
(19, 9, 11, 1, 'Ship classification scope for Bergen shipyard', NULL),
(20, 9, 12, 1, 'Ship classification scope for port facility', NULL),

-- Certificate 10 (Maritime - ISO 45001): covers 2 Maritime sites
(21, 10, 10, 1, 'OH&S management for Oslo office', NULL),
(22, 10, 11, 1, 'OH&S management for Bergen shipyard', NULL),

-- Certificate 11 (Food Excellence - ISO 22000): covers 3 Food sites
(23, 11, 13, 1, 'Food safety management for Paris HQ', NULL),
(24, 11, 14, 1, 'Food safety management for Lyon production facility', NULL),
(25, 11, 15, 1, 'Food safety management for distribution center', NULL),

-- Certificate 12 (Food Excellence - HACCP): covers 2 Food sites
(26, 12, 14, 1, 'HACCP scope for Lyon production facility', NULL),
(27, 12, 15, 1, 'HACCP scope for distribution center', NULL),

-- Certificate 13 (Food Excellence - BRC): covers production site only
(28, 13, 14, 1, 'BRC Grade A scope for Lyon production facility', NULL),

-- Certificate 14 (AutoTech - ISO 9001): covers 3 AutoTech sites
(29, 14, 16, 1, 'Quality management for Rome headquarters', NULL),
(30, 14, 17, 1, 'Quality management for Milan factory', NULL),
(31, 14, 18, 1, 'Quality management for Turin R&D center', NULL),

-- Certificate 15 (AutoTech - IATF 16949): covers 3 AutoTech sites
(32, 15, 16, 1, 'Automotive QMS for Rome headquarters', NULL),
(33, 15, 17, 1, 'Automotive QMS for Milan factory', NULL),
(34, 15, 18, 1, 'Automotive QMS for Turin R&D center', NULL),

-- Certificate 16 (Global Mfg - ISO 9001): covers 4 global sites
(35, 16, 19, 1, 'QMS scope for US headquarters', NULL),
(36, 16, 20, 1, 'QMS scope for UK operations', NULL),
(37, 16, 21, 1, 'QMS scope for Germany plant', NULL),
(38, 16, 22, 1, 'QMS scope for Asia Pacific facility', NULL),

-- Certificate 17 (Global Mfg - ISO 14001): covers 4 global sites
(39, 17, 19, 1, 'EMS scope for US headquarters', NULL),
(40, 17, 20, 1, 'EMS scope for UK operations', NULL),
(41, 17, 21, 1, 'EMS scope for Germany plant', NULL),
(42, 17, 22, 1, 'EMS scope for Asia Pacific facility', NULL),

-- Certificate 18 (Small Tech - ISO 27001): single site
(43, 18, 23, 1, 'Information security management for Small Tech office', NULL),

-- Certificate 19 (Family Food - ISO 22000): single site
(44, 19, 24, 1, 'Food safety management for kitchen facility', NULL),

-- Certificate 20 (TechFlow - ISO 20000 Pending): 2 TechFlow sites
(45, 20, 4, 1, 'IT service management for main office', NULL),
(46, 20, 5, 1, 'IT service management for data center', NULL),

-- Certificate 21 (Green Energy - Carbon Pending): 3 Green Energy sites
(47, 21, 7, 1, 'Carbon footprint scope for Berlin HQ', NULL),
(48, 21, 8, 1, 'Carbon footprint scope for wind farm', NULL),
(49, 21, 9, 1, 'Carbon footprint scope for solar installation', NULL),

-- Certificate 22 (Expired - Tokyo): single site
(50, 22, 25, 0, 'Precision manufacturing scope for main plant', 'Site scope inactive - certificate expired'),

-- Certificate 23 (Acme - Suspended ISO 45001): 2 Acme sites
(51, 23, 1, 1, 'OH&S management for Acme headquarters', 'Under review'),
(52, 23, 2, 1, 'OH&S management for Acme manufacturing plant', 'Under review');

SET IDENTITY_INSERT [dbo].[CertificateSites] OFF;

-- Verify
SELECT COUNT(*) AS TotalCertificateSites FROM [dbo].[CertificateSites];
