-- ============================================================================
-- Seed Data Script for GST Compliance Module
-- Includes sample suppliers and test GST registrations
-- ============================================================================

-- ============================================================================
-- SEED: GST_SUPPLIER (Reference Data)
-- ============================================================================
PRINT 'Seeding GST_SUPPLIER table...';

IF NOT EXISTS (SELECT 1 FROM GST_SUPPLIER WHERE SUPPLIER_NAME = 'TCS')
BEGIN
    INSERT INTO GST_SUPPLIER (SUPPLIER_NAME, EMAIL_ADDRESS, OU, PAN_NO)
    VALUES ('TCS', 'vendor@tcs.com', 'India Operations', 'AAACT5055K');
END

IF NOT EXISTS (SELECT 1 FROM GST_SUPPLIER WHERE SUPPLIER_NAME = 'Infosys')
BEGIN
    INSERT INTO GST_SUPPLIER (SUPPLIER_NAME, EMAIL_ADDRESS, OU, PAN_NO)
    VALUES ('Infosys', 'vendor@infosys.com', 'Global Delivery', 'AAACI5055K');
END

IF NOT EXISTS (SELECT 1 FROM GST_SUPPLIER WHERE SUPPLIER_NAME = 'Wipro')
BEGIN
    INSERT INTO GST_SUPPLIER (SUPPLIER_NAME, EMAIL_ADDRESS, OU, PAN_NO)
    VALUES ('Wipro', 'vendor@wipro.com', 'IT Services', 'AAACW5055K');
END

PRINT 'GST_SUPPLIER table seeded successfully.';
GO

-- ============================================================================
-- SEED: GST_MAIN (Main GST Registrations)
-- ============================================================================
PRINT 'Seeding GST_MAIN table with test GST registrations...';

-- Sample GST Registration 1: Pending Status
IF NOT EXISTS (SELECT 1 FROM GST_MAIN WHERE GST_PANNO = 'AAACG5055K')
BEGIN
    INSERT INTO GST_MAIN (
        GST_TYPE, GST_PANNO, GST_EMAILID, GST_MOBILENO, GST_CREATEDON, GST_STATUS,
        GST_VENDORNAMEFLAG, GST_VENDORNAME, GST_VENDADDLINE1, GST_VENDCITY, 
        GST_VENDSTATE, GST_VENDPINCODE, GST_REGISTRATIONTYPE, GST_CONTACTNAME,
        GST_CONTACTEMAILID, GST_DIGITALFLAG, GST_ENTEREDBY, GST_SCREENTYPE
    )
    VALUES (
        'R',  -- GST Type (Regular)
        'AAACG5055K',  -- PAN
        'gst.contact1@example.com',
        '9876543210',
        GETUTCDATE(),
        'P',  -- Status: Pending
        'Y',
        'Vendor Company A',
        '123 Business Street',
        'Bangalore',
        'Karnataka',
        '560001',
        1,  -- Registration Type: Regular
        'John Smith',
        'john.smith@example.com',
        'Y',
        1,
        'W'
    );
END

-- Sample GST Registration 2: Active Status
IF NOT EXISTS (SELECT 1 FROM GST_MAIN WHERE GST_PANNO = 'ACACD5055K')
BEGIN
    INSERT INTO GST_MAIN (
        GST_TYPE, GST_PANNO, GST_EMAILID, GST_MOBILENO, GST_CREATEDON, GST_MODIFIEDON, GST_STATUS,
        GST_VENDORNAMEFLAG, GST_VENDORNAME, GST_VENDADDLINE1, GST_VENDCITY,
        GST_VENDSTATE, GST_VENDPINCODE, GST_REGISTRATIONTYPE, GST_CONTACTNAME,
        GST_CONTACTEMAILID, GST_DIGITALFLAG, GST_ENTEREDBY, GST_SCREENTYPE
    )
    VALUES (
        'C',  -- GST Type (Composition)
        'ACACD5055K',  -- PAN
        'compliance@example.com',
        '9876543211',
        GETUTCDATE(),
        GETUTCDATE(),
        'A',  -- Status: Active
        'Y',
        'Vendor Company B',
        '456 Commerce Plaza',
        'Mumbai',
        'Maharashtra',
        '400001',
        1,
        'Jane Doe',
        'jane.doe@example.com',
        'Y',
        1,
        'W'
    );
END

-- Sample GST Registration 3: Inactive Status
IF NOT EXISTS (SELECT 1 FROM GST_MAIN WHERE GST_PANNO = 'AXCDE5055K')
BEGIN
    INSERT INTO GST_MAIN (
        GST_TYPE, GST_PANNO, GST_EMAILID, GST_MOBILENO, GST_CREATEDON, GST_MODIFIEDON, GST_STATUS,
        GST_VENDORNAMEFLAG, GST_VENDORNAME, GST_VENDADDLINE1, GST_VENDCITY,
        GST_VENDSTATE, GST_VENDPINCODE, GST_REGISTRATIONTYPE, GST_CONTACTNAME,
        GST_CONTACTEMAILID, GST_DIGITALFLAG, GST_ENTEREDBY, GST_SCREENTYPE
    )
    VALUES (
        'U',  -- GST Type (Unregistered)
        'AXCDE5055K',  -- PAN
        'old.vendor@example.com',
        '9876543212',
        DATEADD(MONTH, -6, GETUTCDATE()),
        DATEADD(MONTH, -1, GETUTCDATE()),
        'I',  -- Status: Inactive
        'Y',
        'Legacy Vendor Inc',
        '789 Old Business Ave',
        'Delhi',
        'Delhi',
        '110001',
        1,
        'Robert Johnson',
        'robert@example.com',
        'Y',
        1,
        'W'
    );
END

PRINT 'GST_MAIN table seeded with test registrations.';
GO

-- ============================================================================
-- SEED: GST_HSNDET (HSN Details for Active Registration)
-- ============================================================================
PRINT 'Seeding GST_HSNDET table with HSN product codes...';

DECLARE @GstId BIGINT;
SET @GstId = (SELECT TOP 1 GST_ID FROM GST_MAIN WHERE GST_PANNO = 'ACACD5055K');

IF @GstId IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM GST_HSNDET WHERE GSTHSN_GSTID = @GstId AND GSTHSN_HSNCODE = '8471'
)
BEGIN
    INSERT INTO GST_HSNDET (GSTHSN_GSTID, GSTHSN_PRODUCTNAME, GSTHSN_HSNCODE, GSTHSN_REMARKS)
    VALUES 
        (@GstId, 'Electronic Data Processing Machines', '8471', 'Computers and peripherals'),
        (@GstId, 'Electrical Machinery', '8504', 'Power supplies and converters'),
        (@GstId, 'Optical Instruments', '9015', 'Electronic measuring devices');
END

PRINT 'GST_HSNDET seeded successfully.';
GO

-- ============================================================================
-- SEED: GST_SERVDET (Service/SAC Details for Active Registration)
-- ============================================================================
PRINT 'Seeding GST_SERVDET table with SAC service codes...';

DECLARE @GstId2 BIGINT;
SET @GstId2 = (SELECT TOP 1 GST_ID FROM GST_MAIN WHERE GST_PANNO = 'ACACD5055K');

IF @GstId2 IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM GST_SERVDET WHERE GSTSAC_GSTID = @GstId2 AND GSTSAC_SACCODE = '998311'
)
BEGIN
    INSERT INTO GST_SERVDET (GSTSAC_GSTID, GSTSAC_SERVICENAME, GSTSAC_SACCODE, GSTSAC_REMARKS)
    VALUES 
        (@GstId2, 'Services related to management of hostels', '998311', 'Hostel management'),
        (@GstId2, 'Advertising and event management', '998361', 'Event organization'),
        (@GstId2, 'Other professional services', '998369', 'Consultancy services');
END

PRINT 'GST_SERVDET seeded successfully.';
GO

-- ============================================================================
-- SEED: GST_STATEREGDET (State Registration Details for Active Registration)
-- ============================================================================
PRINT 'Seeding GST_STATEREGDET with state-wise registrations...';

DECLARE @GstId3 BIGINT;
SET @GstId3 = (SELECT TOP 1 GST_ID FROM GST_MAIN WHERE GST_PANNO = 'ACACD5055K');

IF @GstId3 IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM GST_STATEREGDET WHERE GST_ID = @GstId3 AND GST_STATE = 'Maharashtra'
)
BEGIN
    INSERT INTO GST_STATEREGDET (
        GST_ID, GST_STATE, GST_ADDRESS, GST_VENDCITY, GST_VENDCITYNAME, GST_VENDPINCODE,
        GST_GSTINNO, GST_CONTACTPERSON, GST_EMAILID, GST_MOBILENO, GST_REMARKS
    )
    VALUES 
        (
            @GstId3, 'Maharashtra', '456 Commerce Building', 'Mumbai', 'MUMBAI', '400001',
            '27ACACD5055K1ZA', 'Jane Doe', 'jane@example.com', '9876543211', 'Head Office'
        ),
        (
            @GstId3, 'Karnataka', '123 Tech Park', 'Bangalore', 'BANGALORE', '560001', 
            '29ACACD5055K2ZA', 'Branch Manager', 'branch@example.com', '9876543213', 'Branch Office'
        );
END

PRINT 'GST_STATEREGDET seeded successfully.';
GO

-- ============================================================================
-- Summary Report
-- ============================================================================
PRINT '';
PRINT '============================================================================';
PRINT 'SEED DATA VALIDATION REPORT';
PRINT '============================================================================';
PRINT '';

DECLARE @SupplierCount INT = (SELECT COUNT(*) FROM GST_SUPPLIER);
DECLARE @GstMainCount INT = (SELECT COUNT(*) FROM GST_MAIN);
DECLARE @HsnCount INT = (SELECT COUNT(*) FROM GST_HSNDET);
DECLARE @SacCount INT = (SELECT COUNT(*) FROM GST_SERVDET);
DECLARE @StateRegCount INT = (SELECT COUNT(*) FROM GST_STATEREGDET);

PRINT 'Total Records Seeded:';
PRINT '  GST_SUPPLIER: ' + CAST(@SupplierCount AS VARCHAR(10)) + ' records';
PRINT '  GST_MAIN: ' + CAST(@GstMainCount AS VARCHAR(10)) + ' records';
PRINT '  GST_HSNDET: ' + CAST(@HsnCount AS VARCHAR(10)) + ' records';
PRINT '  GST_SERVDET: ' + CAST(@SacCount AS VARCHAR(10)) + ' records';
PRINT '  GST_STATEREGDET: ' + CAST(@StateRegCount AS VARCHAR(10)) + ' records';
PRINT '';

-- Display GST Status Distribution
PRINT 'GST Registration Status Distribution:';
SELECT 
    ISNULL(GST_STATUS, 'UNKNOWN') AS [Status],
    CASE 
        WHEN GST_STATUS = 'P' THEN 'Pending'
        WHEN GST_STATUS = 'A' THEN 'Active'
        WHEN GST_STATUS = 'I' THEN 'Inactive'
        WHEN GST_STATUS = 'S' THEN 'Suspended'
        ELSE 'Unknown'
    END AS [Description],
    COUNT(*) AS [Count]
FROM GST_MAIN
GROUP BY GST_STATUS;

PRINT '';
PRINT 'Seeding operation completed successfully!';
PRINT '============================================================================';
