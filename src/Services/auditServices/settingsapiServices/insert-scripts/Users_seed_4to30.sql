-- Seed Users 4-30 (users 1-3 already exist)
-- Columns available in DB: UserId, Username, Email, PasswordHash, IsActive, FirstName, LastName
-- CompanyId nullable, Role defaults to 'User'

SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users] ([UserId], [Username], [Email], [PasswordHash], [IsActive], [FirstName], [LastName])
VALUES
-- DNV Staff - Lead Auditors / Technical Experts
(4,  'm.brown',      'michael.brown@dnv.com',              '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Michael',    'Brown'),
(5,  's.wilson',     'sarah.wilson@dnv.com',               '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Sarah',      'Wilson'),
(6,  'l.garcia',     'luis.garcia@dnv.com',                '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Luis',       'Garcia'),
(7,  'r.anderson',   'robert.anderson@dnv.com',            '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Robert',     'Anderson'),
(8,  'e.martinez',   'elena.martinez@dnv.com',             '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Elena',      'Martinez'),
(9,  'h.tanaka',     'hiroshi.tanaka@dnv.com',             '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Hiroshi',    'Tanaka'),
(10, 'k.lee',        'kevin.lee@dnv.com',                  '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Kevin',      'Lee'),
-- Acme Corporation users
(11, 'j.doe',        'john.doe@acmecorp.com',              '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'John',       'Doe'),
(12, 'm.davis',      'mary.davis@acmecorp.com',            '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Mary',       'Davis'),
(13, 'r.miller',     'richard.miller@acmecorp.com',        '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Richard',    'Miller'),
(14, 'l.taylor',     'lisa.taylor@acmecorp.com',           '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Lisa',       'Taylor'),
-- TechFlow users
(15, 'p.white',      'peter.white@techflow.com',           '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Peter',      'White'),
(16, 's.clark',      'susan.clark@techflow.com',           '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Susan',      'Clark'),
(17, 'd.lewis',      'david.lewis@techflow.com',           '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'David',      'Lewis'),
-- Green Energy users
(18, 'a.mueller',    'andreas.mueller@greenenergy.de',     '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Andreas',    'Mueller'),
(19, 'i.schmidt',    'ingrid.schmidt@greenenergy.de',      '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Ingrid',     'Schmidt'),
(20, 'm.weber',      'michael.weber@greenenergy.de',       '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Michael',    'Weber'),
-- Maritime Solutions users
(21, 't.hansen',     'tor.hansen@maritime-solutions.no',   '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Tor',        'Hansen'),
(22, 'k.olsen',      'kari.olsen@maritime-solutions.no',   '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Kari',       'Olsen'),
(23, 'o.berg',       'ola.berg@maritime-solutions.no',     '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Ola',        'Berg'),
-- Food Excellence users
(24, 'c.dubois',     'catherine.dubois@foodexcellence.fr', '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Catherine',  'Dubois'),
(25, 'p.martin',     'pierre.martin@foodexcellence.fr',    '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Pierre',     'Martin'),
(26, 'n.bernard',    'nicole.bernard@foodexcellence.fr',   '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Nicole',     'Bernard'),
-- AutoTech Manufacturing users
(27, 'g.rossi',      'giuseppe.rossi@autotech.it',         '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Giuseppe',   'Rossi'),
(28, 'f.ferrari',    'francesca.ferrari@autotech.it',      '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Francesca',  'Ferrari'),
(29, 'l.bianchi',    'lorenzo.bianchi@autotech.it',        '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Lorenzo',    'Bianchi'),
(30, 'a.conti',      'alessandro.conti@autotech.it',       '$2a$10$N9qo8uLOickgx2ZMRZoMye1kW7.QN6j1h9g3qBL.WJuJ8a4KqAQuO', 1, 'Alessandro', 'Conti');

SET IDENTITY_INSERT [dbo].[Users] OFF;

-- Update existing users 1-3 to set FirstName/LastName
UPDATE [dbo].[Users] SET [FirstName] = 'System',    [LastName] = 'Administrator' WHERE [UserId] = 1 AND ([FirstName] IS NULL OR [FirstName] = '');
UPDATE [dbo].[Users] SET [FirstName] = 'John',      [LastName] = 'Auditor'       WHERE [UserId] = 2 AND ([FirstName] IS NULL OR [FirstName] = '');
UPDATE [dbo].[Users] SET [FirstName] = 'Client',    [LastName] = 'One'           WHERE [UserId] = 3 AND ([FirstName] IS NULL OR [FirstName] = '');

SELECT COUNT(*) AS TotalUsers FROM [dbo].[Users];
