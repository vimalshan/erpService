-- Insert sample data for AuditSites table
-- Maps which sites participate in each audit (AuditId refs Audits.auditId 1-20)

INSERT INTO [dbo].[AuditSites]
    ([AuditId], [SiteId], [IsActive], [Status], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy])
VALUES
-- Audit 1: Acme ISO9001 ICA — Sites 1,2,3
(1, 1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(1, 2, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(1, 3, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 2: Acme ISO9001 Surveillance — Sites 1,2
(2, 1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(2, 2, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 3: Acme ISO14001 ICA — Sites 1,2
(3, 1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(3, 2, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 4: Acme ISO14001 Surveillance — Sites 1,2
(4, 1, 1, 'active', GETDATE(), GETDATE(), 1, 1),
(4, 2, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 5: Acme ISO45001 Pre-Assessment — Site 1
(5, 1, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 6: TechFlow ISO9001 ICA — Sites 4,5,6
(6, 4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(6, 5, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(6, 6, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 7: TechFlow ISO9001 Surveillance Y1 — Sites 4,5
(7, 4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(7, 5, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 8: TechFlow ISO9001 Surveillance Y2 — Sites 4,5
(8, 4, 1, 'active', GETDATE(), GETDATE(), 1, 1),
(8, 5, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 9: TechFlow ISO27001 ICA — Sites 4,5
(9, 4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(9, 5, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 10: TechFlow ISO27001 Surveillance — Sites 4,5
(10, 4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(10, 5, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 11: Green Energy ISO14001 ICA — Sites 7,8,9
(11, 7, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(11, 8, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(11, 9, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 12: Green Energy ISO50001 ICA — Sites 7,8
(12, 7, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(12, 8, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 13: Green Energy ISO50001 Surveillance — Sites 7,8
(13, 7, 1, 'active', GETDATE(), GETDATE(), 1, 1),
(13, 8, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 14: Green Energy ISO50001 Pre-Assessment — Site 7
(14, 7, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 15: Maritime ISO9001 ICA — Sites 10,11,12
(15, 10, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(15, 11, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(15, 12, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 16: Maritime ISM ICA — Sites 10,11,12
(16, 10, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(16, 11, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(16, 12, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 17: Maritime ISM Surveillance — Sites 10,11
(17, 10, 1, 'active', GETDATE(), GETDATE(), 1, 1),
(17, 11, 1, 'active', GETDATE(), GETDATE(), 1, 1),

-- Audit 18: Maritime ISO45001 ICA — Sites 10,11
(18, 10, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(18, 11, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 19: Food Excellence ISO22000 ICA — Sites 13,14,15
(19, 13, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(19, 14, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(19, 15, 1, 'completed', GETDATE(), GETDATE(), 1, 1),

-- Audit 20: Food Excellence HACCP ICA — Sites 13,14
(20, 13, 1, 'completed', GETDATE(), GETDATE(), 1, 1),
(20, 14, 1, 'completed', GETDATE(), GETDATE(), 1, 1);
(NEWID(), 19, 3, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Global US HQ - ISO 45001 (main site)
(NEWID(), 20, 3, 1, 0, GETDATE(), GETDATE(), 1, 1), -- Global UK Ops - ISO 45001
(NEWID(), 21, 3, 1, 0, GETDATE(), GETDATE(), 1, 1), -- Global DE Plant - ISO 45001
(NEWID(), 22, 3, 1, 0, GETDATE(), GETDATE(), 1, 1), -- Global AP Hub - ISO 45001

-- Additional specialized sites
-- Small Tech Solutions
(NEWID(), 23, 4, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Small Tech Office - ISO 27001 (main site)

-- Family Food Co
(NEWID(), 24, 5, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Family Food Kitchen - ISO 22000 (main site)
(NEWID(), 24, 28, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Family Food Kitchen - HACCP (main site)

-- Dutch Innovation Lab
(NEWID(), 25, 1, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Dutch Innovation Lab - ISO 9001 (main site)
(NEWID(), 25, 4, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Dutch Innovation Lab - ISO 27001 (main site)

-- EuroLogistics Main Hub
(NEWID(), 26, 1, 1, 1, GETDATE(), GETDATE(), 1, 1), -- EuroLogistics Hub - ISO 9001 (main site)
(NEWID(), 26, 2, 1, 1, GETDATE(), GETDATE(), 1, 1), -- EuroLogistics Hub - ISO 14001 (main site)

-- Alpine Precision Factory
(NEWID(), 27, 1, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Alpine Precision - ISO 9001 (main site)
(NEWID(), 27, 16, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Alpine Precision - ISO 17025 (main site)

-- Nordic Forest Mill
(NEWID(), 28, 2, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Nordic Forest - ISO 14001 (main site)
(NEWID(), 28, 3, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Nordic Forest - ISO 45001 (main site)

-- Danish Wind Offshore Base
(NEWID(), 29, 23, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Danish Wind - Wind Turbine Certification (main site)
(NEWID(), 29, 2, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Danish Wind - ISO 14001 (main site)

-- Tokyo Precision Main Plant
(NEWID(), 30, 1, 1, 1, GETDATE(), GETDATE(), 1, 1), -- Tokyo Precision - ISO 9001 (main site)
(NEWID(), 30, 8, 1, 1, GETDATE(), GETDATE(), 1, 1); -- Tokyo Precision - IATF 16949 (main site)

-- Verify the insert
SELECT COUNT(*) as TotalAuditSiteAssignments FROM [dbo].[AuditSites];

-- Show audit sites by service
SELECT 
    s.ServiceName,
    COUNT(aus.SiteId) as SiteCount,
    COUNT(CASE WHEN aus.IsMainSite = 1 THEN 1 END) as MainSites
FROM [dbo].[Services] s
LEFT JOIN [dbo].[AuditSites] aus ON s.ServiceId = aus.ServiceId AND aus.IsActive = 1
GROUP BY s.ServiceId, s.ServiceName
HAVING COUNT(aus.SiteId) > 0
ORDER BY SiteCount DESC;

-- Show sites with their services
SELECT 
    c.CompanyName,
    st.SiteName,
    COUNT(aus.ServiceId) as ServiceCount,
    STRING_AGG(s.ServiceName, ', ') as Services
FROM [dbo].[AuditSites] aus
INNER JOIN [dbo].[Sites] st ON aus.SiteId = st.SiteId
INNER JOIN [dbo].[Companies] c ON st.CompanyId = c.CompanyId
INNER JOIN [dbo].[Services] s ON aus.ServiceId = s.ServiceId
WHERE aus.IsActive = 1
GROUP BY c.CompanyId, c.CompanyName, st.SiteId, st.SiteName
ORDER BY c.CompanyName, st.SiteName;

-- Show main sites vs additional sites
SELECT 
    'Main Sites' as SiteType,
    COUNT(*) as Count
FROM [dbo].[AuditSites] 
WHERE IsActive = 1 AND IsMainSite = 1
UNION ALL
SELECT 
    'Additional Sites' as SiteType,
    COUNT(*) as Count
FROM [dbo].[AuditSites] 
WHERE IsActive = 1 AND IsMainSite = 0;