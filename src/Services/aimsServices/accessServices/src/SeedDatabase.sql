-- ===========================================
-- Seed Database Script for ACCESSDB
-- Adds indexes and views from deployment script
-- ===========================================

USE ACCESSDB;
GO

-- Create Indexes if they don't exist
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AIMS_USERROLE_EMPSYSID')
BEGIN
    CREATE INDEX IX_AIMS_USERROLE_EMPSYSID ON AIMS_USERROLE (ROLE_EMPSYSID);
    PRINT 'Created index: IX_AIMS_USERROLE_EMPSYSID';
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MENU_MASTER_PARENT')
BEGIN
    CREATE INDEX IX_MENU_MASTER_PARENT ON MENU_MASTER (MENU_PARENTID);
    PRINT 'Created index: IX_MENU_MASTER_PARENT';
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SPARSHMENU_ACCESS_UNIT')
BEGIN
    CREATE INDEX IX_SPARSHMENU_ACCESS_UNIT ON SPARSHMENU_ACCESS (ACCESS_UNIT);
    PRINT 'Created index: IX_SPARSHMENU_ACCESS_UNIT';
END;
GO

-- Create Status View
DROP VIEW IF EXISTS vw_AccessDB_Status;
GO

CREATE VIEW vw_AccessDB_Status AS
SELECT 
    'ACCESSDB' AS DatabaseName,
    'Access Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM AIMS_USERMAP) AS UserMappings,
    (SELECT COUNT(*) FROM AIMS_USERROLE) AS UserRoles,
    (SELECT COUNT(*) FROM MENU_MASTER) AS MenuItems,
    (SELECT COUNT(*) FROM SPARSHMENU_MASTER) AS SPARSHMenuItems,
    GETDATE() AS LastChecked;
GO

-- ===========================================
-- Test Data: Optional Seed Data
-- ===========================================

-- Insert test user mapping (if not exists)
IF NOT EXISTS (SELECT 1 FROM AIMS_USERMAP WHERE USER_EMPSYSID = 1001)
BEGIN
    INSERT INTO AIMS_USERMAP (USER_EMPSYSID, USER_EFFDATE, USER_MODIFIEDBY, USER_MODIFIEDON)
    VALUES (1001, GETDATE(), 0, GETDATE());
    PRINT 'Inserted test user mapping: 1001';
END;
GO

-- Insert sample menu items (let SQL Server generate MENU_ID)
IF NOT EXISTS (SELECT 1 FROM MENU_MASTER WHERE Menu_NAME = 'User Management')
BEGIN
    INSERT INTO MENU_MASTER (Menu_NAME, Menu_PATH, MENU_DISPLAYORDER, MENU_MODIFIEDBY, MENU_MODIFIEDON)
    VALUES 
        ('User Management', '/admin/users', 1, 0, GETDATE()),
        ('Role Management', '/admin/roles', 2, 0, GETDATE()),
        ('Menu Management', '/admin/menus', 3, 0, GETDATE()),
        ('Reports', '/reports', 4, 0, GETDATE());
    PRINT 'Inserted sample menu items';
END;
GO

-- Insert sample SPARSH menu
IF NOT EXISTS (SELECT 1 FROM SPARSHMENU_MASTER WHERE SPARSHMENU_ID = 1)
BEGIN
    INSERT INTO SPARSHMENU_MASTER (SPARSHMENU_ID, SPARSHMENU_NAME, SPARSHMENU_PAGENAME, SPARSHMENU_LASTMODIFIEDBY, SPARSHMENU_LASTMODIFIEDON)
    VALUES 
        (1, 'Dashboard', '/sparsh/dashboard', 0, GETDATE()),
        (2, 'Reports', '/sparsh/reports', 0, GETDATE()),
        (3, 'Settings', '/sparsh/settings', 0, GETDATE());
    PRINT 'Inserted sample SPARSH menu items';
END;
GO

-- ===========================================
-- Verification Report
-- ===========================================
PRINT '======================================';
PRINT 'ACCESSDB SEED COMPLETE';
PRINT '======================================';

SELECT 'Database Status:' AS Section;
SELECT * FROM vw_AccessDB_Status;

PRINT '';
PRINT 'Table Row Counts:';
SELECT COUNT(*) AS [AIMS_USERMAP Count] FROM AIMS_USERMAP;
SELECT COUNT(*) AS [AIMS_USERROLE Count] FROM AIMS_USERROLE;
SELECT COUNT(*) AS [MENU_MASTER Count] FROM MENU_MASTER;
SELECT COUNT(*) AS [AIMS_USERMENUMAP Count] FROM AIMS_USERMENUMAP;
SELECT COUNT(*) AS [SPARSHMENU_MASTER Count] FROM SPARSHMENU_MASTER;
SELECT COUNT(*) AS [SPARSHMENU_ACCESS Count] FROM SPARSHMENU_ACCESS;

PRINT '';
PRINT 'Database Ready for Application!';
GO
