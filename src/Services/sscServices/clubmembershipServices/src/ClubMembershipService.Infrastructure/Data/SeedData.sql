-- ==========================================
-- Seed Data: CLUB MEMBERSHIP MODULE
-- Database: SSCDB
-- Run after EF migrations have created the schema
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- CLUB_MASTER seed data
-- ==========================================
SET IDENTITY_INSERT [CLUB_MASTER] ON;

MERGE INTO [CLUB_MASTER] AS target
USING (VALUES
    (1, 'Chess Club',         'A', 1, GETDATE(), NULL, NULL),
    (2, 'Photography Club',   'A', 1, GETDATE(), NULL, NULL),
    (3, 'Coding & Robotics',  'A', 1, GETDATE(), NULL, NULL),
    (4, 'Book Reading Club',  'A', 1, GETDATE(), NULL, NULL),
    (5, 'Music & Arts Club',  'I', 1, GETDATE(), NULL, NULL)
) AS source (CLUB_ID, CLUB_NAME, CLUB_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
ON target.CLUB_ID = source.CLUB_ID
WHEN NOT MATCHED THEN
    INSERT (CLUB_ID, CLUB_NAME, CLUB_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
    VALUES (source.CLUB_ID, source.CLUB_NAME, source.CLUB_STATUS, source.CREATED_BY, source.CREATED_ON, source.MODIFIED_BY, source.MODIFIED_ON);

SET IDENTITY_INSERT [CLUB_MASTER] OFF;
GO

-- ==========================================
-- CLUB_MEMBERSHIP seed data
-- ==========================================
SET IDENTITY_INSERT [CLUB_MEMBERSHIP] ON;

MERGE INTO [CLUB_MEMBERSHIP] AS target
USING (VALUES
    (1001, 1, 101, '2024-01-10', 500.00, 'A', 1, GETDATE(), NULL, NULL),
    (1002, 1, 102, '2024-02-15', 500.00, 'A', 1, GETDATE(), NULL, NULL),
    (1003, 2, 103, '2024-01-20', 750.00, 'A', 1, GETDATE(), NULL, NULL),
    (1004, 2, 104, '2024-03-05', 750.00, 'I', 1, GETDATE(), NULL, NULL),
    (1005, 3, 105, '2024-02-01', 600.00, 'A', 1, GETDATE(), NULL, NULL),
    (1006, 3, 106, '2024-04-10', 600.00, 'A', 1, GETDATE(), NULL, NULL),
    (1007, 4, 107, '2024-01-25', 300.00, 'A', 1, GETDATE(), NULL, NULL),
    (1008, 4, 101, '2024-05-01', 300.00, 'A', 1, GETDATE(), NULL, NULL),
    (1009, 1, 108, '2024-06-12', 500.00, 'A', 1, GETDATE(), NULL, NULL),
    (1010, 3, 109, '2024-07-18', 600.00, 'I', 1, GETDATE(), NULL, NULL)
) AS source (MEMBERSHIP_ID, CLUB_ID, MEMBER_ID, JOIN_DATE, MEMBERSHIP_FEE, MEMBERSHIP_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
ON target.MEMBERSHIP_ID = source.MEMBERSHIP_ID
WHEN NOT MATCHED THEN
    INSERT (MEMBERSHIP_ID, CLUB_ID, MEMBER_ID, JOIN_DATE, MEMBERSHIP_FEE, MEMBERSHIP_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
    VALUES (source.MEMBERSHIP_ID, source.CLUB_ID, source.MEMBER_ID, source.JOIN_DATE, source.MEMBERSHIP_FEE, source.MEMBERSHIP_STATUS, source.CREATED_BY, source.CREATED_ON, source.MODIFIED_BY, source.MODIFIED_ON);

SET IDENTITY_INSERT [CLUB_MEMBERSHIP] OFF;
GO

-- ==========================================
-- CLUB_ACTIVITY seed data
-- ==========================================
SET IDENTITY_INSERT [CLUB_ACTIVITY] ON;

MERGE INTO [CLUB_ACTIVITY] AS target
USING (VALUES
    (2001, 1, 'Annual Chess Tournament',        '2024-03-15', 2000.00, 101, 'C', 1, GETDATE(), NULL, NULL),
    (2002, 1, 'Blitz Chess Championship',       '2024-08-20', 1500.00, 102, 'P', 1, GETDATE(), NULL, NULL),
    (2003, 2, 'Nature Photography Walk',        '2024-04-10', 800.00,  103, 'C', 1, GETDATE(), NULL, NULL),
    (2004, 2, 'Portrait Workshop',              '2024-09-05', 1200.00, 103, 'O', 1, GETDATE(), NULL, NULL),
    (2005, 3, 'Hackathon 2024',                 '2024-05-18', 5000.00, 105, 'C', 1, GETDATE(), NULL, NULL),
    (2006, 3, 'Robot Building Contest',         '2024-10-12', 3500.00, 106, 'P', 1, GETDATE(), NULL, NULL),
    (2007, 4, 'Monthly Book Review - July',     '2024-07-28', 200.00,  107, 'C', 1, GETDATE(), NULL, NULL),
    (2008, 4, 'Author Meet & Greet',            '2024-11-15', 600.00,  107, 'P', 1, GETDATE(), NULL, NULL),
    (2009, 1, 'Chess Strategy Seminar',         '2024-06-30', 300.00,  101, 'C', 1, GETDATE(), NULL, NULL),
    (2010, 3, 'AI & Machine Learning Talk',     '2024-12-01', 1000.00, 105, 'P', 1, GETDATE(), NULL, NULL)
) AS source (ACTIVITY_ID, CLUB_ID, ACTIVITY_NAME, ACTIVITY_DATE, ACTIVITY_BUDGET, ORGANIZER_ID, ACTIVITY_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
ON target.ACTIVITY_ID = source.ACTIVITY_ID
WHEN NOT MATCHED THEN
    INSERT (ACTIVITY_ID, CLUB_ID, ACTIVITY_NAME, ACTIVITY_DATE, ACTIVITY_BUDGET, ORGANIZER_ID, ACTIVITY_STATUS, CREATED_BY, CREATED_ON, MODIFIED_BY, MODIFIED_ON)
    VALUES (source.ACTIVITY_ID, source.CLUB_ID, source.ACTIVITY_NAME, source.ACTIVITY_DATE, source.ACTIVITY_BUDGET, source.ORGANIZER_ID, source.ACTIVITY_STATUS, source.CREATED_BY, source.CREATED_ON, source.MODIFIED_BY, source.MODIFIED_ON);

SET IDENTITY_INSERT [CLUB_ACTIVITY] OFF;
GO

PRINT 'Seed data inserted successfully into CLUB_MEMBERSHIP_MODULE tables.';
GO
