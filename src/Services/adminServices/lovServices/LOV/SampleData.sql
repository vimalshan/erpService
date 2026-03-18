-- ==========================================
-- LOV SERVICE - SAMPLE DATA
-- Database: LOVDB
-- ==========================================

USE [LOVDB];
GO

-- ── LOV_TYPE Sample Data ──────────────────────────────────────────────────
PRINT 'Inserting LOV_TYPE sample data...';
GO

MERGE INTO LOV_TYPE AS target
USING (VALUES
    (1, 'CATEGORY'),
    (2, 'STATUS'),
    (3, 'PRIORITY'),
    (4, 'DEPARTMENT'),
    (5, 'UOM'),
    (6, 'PAYMENT_MODE'),
    (7, 'TAX_TYPE'),
    (8, 'CURRENCY')
) AS source (LOV_TYPE_ID, LOV_TYPE_NAME)
ON target.LOV_TYPE_ID = source.LOV_TYPE_ID
WHEN MATCHED THEN
    UPDATE SET LOV_TYPE_NAME = source.LOV_TYPE_NAME
WHEN NOT MATCHED THEN
    INSERT (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (source.LOV_TYPE_ID, source.LOV_TYPE_NAME);
GO

-- ── LOV_MASTER Sample Data ────────────────────────────────────────────────
PRINT 'Inserting LOV_MASTER sample data...';
GO

DECLARE @Now DATETIME2(3) = SYSDATETIME();
DECLARE @UserId BIGINT = 1;

MERGE INTO LOV_MASTER AS target
USING (VALUES
    -- CATEGORY (Type 1)
    (101, 1, 'Electronics',   @UserId, @Now),
    (102, 1, 'Furniture',     @UserId, @Now),
    (103, 1, 'Stationery',    @UserId, @Now),
    (104, 1, 'Consumables',   @UserId, @Now),
    -- STATUS (Type 2)
    (201, 2, 'Active',        @UserId, @Now),
    (202, 2, 'Inactive',      @UserId, @Now),
    (203, 2, 'Pending',       @UserId, @Now),
    (204, 2, 'Cancelled',     @UserId, @Now),
    -- PRIORITY (Type 3)
    (301, 3, 'High',          @UserId, @Now),
    (302, 3, 'Medium',        @UserId, @Now),
    (303, 3, 'Low',           @UserId, @Now),
    -- DEPARTMENT (Type 4)
    (401, 4, 'IT',            @UserId, @Now),
    (402, 4, 'HR',            @UserId, @Now),
    (403, 4, 'Finance',       @UserId, @Now),
    (404, 4, 'Operations',    @UserId, @Now),
    -- UOM (Type 5)
    (501, 5, 'Nos',           @UserId, @Now),
    (502, 5, 'Kg',            @UserId, @Now),
    (503, 5, 'Ltr',           @UserId, @Now),
    (504, 5, 'Box',           @UserId, @Now),
    -- PAYMENT_MODE (Type 6)
    (601, 6, 'Cash',          @UserId, @Now),
    (602, 6, 'Credit Card',   @UserId, @Now),
    (603, 6, 'Bank Transfer', @UserId, @Now),
    -- TAX_TYPE (Type 7)
    (701, 7, 'GST',           @UserId, @Now),
    (702, 7, 'VAT',           @UserId, @Now),
    (703, 7, 'Exempt',        @UserId, @Now),
    -- CURRENCY (Type 8)
    (801, 8, 'INR',           @UserId, @Now),
    (802, 8, 'USD',           @UserId, @Now),
    (803, 8, 'EUR',           @UserId, @Now)
) AS source (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
ON target.LOV_ID = source.LOV_ID
WHEN MATCHED THEN
    UPDATE SET LOV_NAME = source.LOV_NAME, LOV_UPDATED_BY = source.LOV_UPDATED_BY, LOV_UPDATED_ON = source.LOV_UPDATED_ON
WHEN NOT MATCHED THEN
    INSERT (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
    VALUES (source.LOV_ID, source.LOV_TYPE_ID, source.LOV_NAME, source.LOV_UPDATED_BY, source.LOV_UPDATED_ON);
GO

-- ── ITEMDATA Sample Data ──────────────────────────────────────────────────
PRINT 'Inserting ITEMDATA sample data...';
GO

MERGE INTO ITEMDATA AS target
USING (VALUES
    ('Electronics', 'Laptop Dell XPS 15',        'Dell',    'Nos',  85000),
    ('Electronics', 'Monitor 27 inch',            'LG',      'Nos',  22000),
    ('Electronics', 'Wireless Keyboard',          'Logitech','Nos',   3500),
    ('Electronics', 'Wireless Mouse',             'Logitech','Nos',   1800),
    ('Electronics', 'USB-C Hub 7-in-1',           'Anker',   'Nos',   4200),
    ('Furniture',   'Office Chair Ergonomic',     'Herman',  'Nos',  45000),
    ('Furniture',   'Standing Desk 180cm',        'IKEA',    'Nos',  32000),
    ('Furniture',   'Bookshelf 5-tier',           'IKEA',    'Nos',   8500),
    ('Stationery',  'A4 Paper 500 sheets',        'ITC',     'Box',    550),
    ('Stationery',  'Ballpoint Pen Box',          'Cello',   'Box',    250),
    ('Stationery',  'Sticky Notes Pack',          '3M',      'Box',    320),
    ('Consumables', 'Hand Sanitizer 500ml',       'Dettol',  'Ltr',    250),
    ('Consumables', 'Printer Ink Cartridge',      'HP',      'Nos',   1200),
    ('Consumables', 'Coffee Beans 1kg',           'Lavazza', 'Kg',    1800)
) AS source (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
ON target.ITEMNAME = source.ITEMNAME AND target.CATNAME = source.CATNAME
WHEN MATCHED THEN
    UPDATE SET MAKE = source.MAKE, UOM = source.UOM, PRICE = source.PRICE
WHEN NOT MATCHED THEN
    INSERT (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
    VALUES (source.CATNAME, source.ITEMNAME, source.MAKE, source.UOM, source.PRICE);
GO

PRINT 'Sample data inserted successfully.';
GO

-- Verify counts
SELECT 'LOV_TYPE'   AS TableName, COUNT(*) AS RecordCount FROM LOV_TYPE
UNION ALL
SELECT 'LOV_MASTER' AS TableName, COUNT(*) AS RecordCount FROM LOV_MASTER
UNION ALL
SELECT 'ITEMDATA'   AS TableName, COUNT(*) AS RecordCount FROM ITEMDATA;
GO
