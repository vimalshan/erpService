-- Seed Companies 4-18 (companies 1-3 already exist)
-- Available columns: CompanyId, CompanyName, CompanyCode, ParentCompanyId, AccountDNVId,
--                    Address (new), CityId (new), CountryId (new), ZipCode, PostalCode (new),
--                    VATNumber, PONumberRequired, IsServiceRequestOpen, IsActive
-- Note: CreatedAt has a default (getdate()), no need to specify

SET IDENTITY_INSERT [dbo].[Companies] ON;

INSERT INTO [dbo].[Companies]
    ([CompanyId], [CompanyName], [CompanyCode], [ParentCompanyId], [AccountDNVId],
     [Address], [CityId], [CountryId], [ZipCode], [PostalCode],
     [VATNumber], [PONumberRequired], [IsServiceRequestOpen], [IsActive])
VALUES
(4,  'Maritime Solutions AS',          'MARISOL-NO',     NULL, 'DNV-MAR-004',
     'Strandveien 22',         36, 11, '0001',     '0001',     'NO123456789', 0, 1, 1),
(5,  'Food Excellence Corp',           'FOODEX-FR',      NULL, 'DNV-FOODEX-005',
     '78 Avenue de la Gastronomie', 16,  4, '75001',    '75001',    'FR123456789', 1, 1, 1),
(6,  'AutoTech Manufacturing SpA',     'AUTOTECH-IT',    NULL, 'DNV-AUTOTECH-006',
     'Via dell''Industria 42', 21,  5, '00118',    '00118',    'IT123456789', 1, 1, 1),
(7,  'Iberica Construcciones SA',      'IBERICA-ES',     NULL, 'DNV-IBERICA-007',
     'Calle de la Construccion 89', 26, 6, '28001', '28001',    'ES123456789', 1, 0, 1),
(8,  'Dutch Innovation BV',            'DUTCHINNOV-NL',  NULL, 'DNV-DUTCHINNOV-008',
     'Innovatielaan 156',      31,  7, '1011',     '1011',     'NL123456789', 0, 0, 1),
(9,  'EuroLogistics BVBA',             'EUROLOG-BE',     NULL, 'DNV-EUROLOG-009',
     'Logistiekstraat 67',     32,  8, '2000',     '2000',     'BE123456789', 1, 1, 1),
(10, 'Alpine Precision AG',            'ALPINE-CH',      NULL, 'DNV-ALPINE-010',
     'Prazisionsweg 34',        9,  9, '8001',     '8001',     'CH123456789', 1, 0, 1),
(11, 'Nordic Forest Products AB',      'NORFOR-SE',      NULL, 'DNV-NORFOR-011',
     'Skogsvägen 12',          12, 12, '111 22',   '111 22',   'SE123456789', 0, 0, 1),
(12, 'Danish Wind Energy A/S',         'DANWIND-DK',     NULL, 'DNV-DANWIND-012',
     'Vindenergivej 88',       13, 13, '2100',     '2100',     'DK123456789', 1, 1, 1),
(13, 'Tokyo Precision Industries KK',  'TOKPREC-JP',     NULL, 'DNV-TOKPREC-013',
     '3-4-5 Shibuya, Shibuya-ku', 41, 15, '150-0002', '150-0002', 'JP123456789', 0, 1, 1),
(14, 'Southeast Asia Marine Pte Ltd',  'SEAMARINE-SG',   NULL, 'DNV-SEAMARINE-014',
     '10 Marina Boulevard',    44, 18, '018983',   '018983',   'SG123456789', 1, 1, 1),
(15, 'Aussie Mining Solutions Pty Ltd','AUSMIN-AU',      NULL, 'DNV-AUSMIN-015',
     '123 Mining Street',      45, 19, '2000',     '2000',     'AU123456789', 1, 1, 1),
(16, 'Canadian Energy Systems Inc',    'CANENERGY-CA',   NULL, 'DNV-CANENERGY-016',
     '456 Energy Drive',       46, 21, 'M5H 2N2',  'M5H 2N2',  'CA123456789', 0, 1, 1),
(17, 'Global Manufacturing Inc',       'GLOBMAN-MULTI',  NULL, 'DNV-GLOBMAN-017',
     '789 Global Plaza',        1,  1, '10002',    '10002',    'US987654321', 1, 1, 1),
(18, 'European Technical Services Ltd','EUROTECH-MULTI', NULL, 'DNV-EUROTECH-018',
     '321 European Way',        6,  2, 'EC1A 1BB', 'EC1A 1BB', 'GB987654321', 0, 1, 1);

SET IDENTITY_INSERT [dbo].[Companies] OFF;

-- Update existing companies 1-3 to add AccountDNVId and location info
UPDATE [dbo].[Companies] SET [AccountDNVId] = 'DNV-CORP-001',   [CityId] = 1, [CountryId] = 11, [Address] = 'Veritasveien 1, Hovik' WHERE [CompanyId] = 1 AND [AccountDNVId] IS NULL;
UPDATE [dbo].[Companies] SET [AccountDNVId] = 'DNV-ACME-002',   [CityId] = 2, [CountryId] = 1,  [Address] = '123 Industrial Blvd'    WHERE [CompanyId] = 2 AND [AccountDNVId] IS NULL;
UPDATE [dbo].[Companies] SET [AccountDNVId] = 'DNV-GLOBAL-003', [CityId] = 3, [CountryId] = 1,  [Address] = '789 Global Plaza'       WHERE [CompanyId] = 3 AND [AccountDNVId] IS NULL;

SELECT COUNT(*) AS TotalCompanies FROM [dbo].[Companies];
