-- ============================================================================
-- Entity Framework Core Migration Script: InitialCreate
-- Generated from GstDbContext model snapshot
-- Idempotent: IF NOT EXISTS checks ensure script can run multiple times
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'dbo')
BEGIN
    EXEC sp_executesql N'CREATE SCHEMA [dbo];'
END
GO

-- ============================================================================
-- CREATE TABLE: GST_SUPPLIER (Reference Data)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GST_SUPPLIER')
BEGIN
    CREATE TABLE [GST_SUPPLIER] (
        [SUPPLIER_NUMBER] bigint NOT NULL IDENTITY (1, 1),
        [SUPPLIER_NAME] nvarchar(200) NOT NULL,
        [EMAIL_ADDRESS] nvarchar(50) NULL,
        [OU] nvarchar(200) NULL,
        [PAN_NO] nvarchar(max) NULL,
        CONSTRAINT [PK_GST_SUPPLIER] PRIMARY KEY ([SUPPLIER_NUMBER])
    );
END
GO

-- ============================================================================
-- CREATE TABLE: GST_MAIN (Root Aggregate)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GST_MAIN')
BEGIN
    CREATE TABLE [GST_MAIN] (
        [GST_ID] bigint NOT NULL IDENTITY (1, 1),
        [GST_TYPE] nvarchar(1) NULL,
        [GST_PANNO] nvarchar(20) NOT NULL,
        [GST_EMAILID] nvarchar(200) NULL,
        [GST_MOBILENO] nvarchar(max) NULL,
        [GST_CREATEDON] datetime2 NOT NULL DEFAULT GETUTCDATE(),
        [GST_MODIFIEDON] datetime2 NULL,
        [GST_VENDORID] bigint NULL,
        [GST_VENDORNAMEFLAG] nvarchar(1) NULL,
        [GST_VENDORNAME] nvarchar(200) NULL,
        [GST_VENDCONST] int NULL,
        [GST_VENDADDFLAG] nvarchar(1) NULL,
        [GST_VENDADDLINE1] nvarchar(200) NULL,
        [GST_VENDADDLINE2] nvarchar(100) NULL,
        [GST_VENDADDLINE3] nvarchar(100) NULL,
        [GST_VENDADDLINE4] nvarchar(100) NULL,
        [GST_VENDCITY] nvarchar(100) NULL,
        [GST_VENDCITYNAME] nvarchar(100) NULL,
        [GST_VENDSTATE] nvarchar(100) NULL,
        [GST_VENDPINCODE] nvarchar(100) NULL,
        [GST_REGISTRATIONTYPE] int NOT NULL,
        [GST_CONTACTNAME] nvarchar(100) NULL,
        [GST_CONTACTEMAILID] nvarchar(100) NULL,
        [GST_CONTACTMOBILENO] nvarchar(max) NULL,
        [GST_REMARKS] nvarchar(200) NULL,
        [GST_STATUS] nvarchar(1) NULL,
        [GST_DIGITALFLAG] nvarchar(255) NOT NULL,
        [GST_GSTNCOPY] nvarchar(200) NULL,
        [GST_ENTEREDBYFLA] nvarchar(1) NULL,
        [GST_ENTEREDBY] bigint NULL,
        [GST_SCREENTYPE] nvarchar(1) NULL,
        CONSTRAINT [PK_GST_MAIN] PRIMARY KEY ([GST_ID]),
        CONSTRAINT [AK_GST_MAIN_GST_PANNO] UNIQUE ([GST_PANNO])
    );
END
GO

-- ============================================================================
-- CREATE TABLE: GST_HSNDET (HSN Details - Child of GST_MAIN)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GST_HSNDET')
BEGIN
    CREATE TABLE [GST_HSNDET] (
        [GSTHSN_ID] bigint NOT NULL IDENTITY (1, 1),
        [GSTHSN_GSTID] bigint NOT NULL,
        [GSTHSN_PRODUCTNAME] nvarchar(100) NULL,
        [GSTHSN_HSNCODE] nvarchar(50) NULL,
        [GSTHSN_REMARKS] nvarchar(200) NULL,
        CONSTRAINT [PK_GST_HSNDET] PRIMARY KEY ([GSTHSN_ID]),
        CONSTRAINT [FK_GST_HSNDET_GST_MAIN_GSTHSN_GSTID] FOREIGN KEY ([GSTHSN_GSTID]) 
            REFERENCES [GST_MAIN] ([GST_ID]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_GST_HSNDET_GSTHSN_GSTID] ON [GST_HSNDET] ([GSTHSN_GSTID]);
END
GO

-- ============================================================================
-- CREATE TABLE: GST_SERVDET (Service Details / SAC - Child of GST_MAIN)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GST_SERVDET')
BEGIN
    CREATE TABLE [GST_SERVDET] (
        [GSTSAC_ID] bigint NOT NULL IDENTITY (1, 1),
        [GSTSAC_GSTID] bigint NOT NULL,
        [GSTSAC_SERVICENAME] nvarchar(100) NULL,
        [GSTSAC_SACCODE] nvarchar(50) NULL,
        [GSTSAC_REMARKS] nvarchar(200) NULL,
        CONSTRAINT [PK_GST_SERVDET] PRIMARY KEY ([GSTSAC_ID]),
        CONSTRAINT [FK_GST_SERVDET_GST_MAIN_GSTSAC_GSTID] FOREIGN KEY ([GSTSAC_GSTID]) 
            REFERENCES [GST_MAIN] ([GST_ID]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_GST_SERVDET_GSTSAC_GSTID] ON [GST_SERVDET] ([GSTSAC_GSTID]);
END
GO

-- ============================================================================
-- CREATE TABLE: GST_STATEREGDET (State Registration Details - Child of GST_MAIN)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GST_STATEREGDET')
BEGIN
    CREATE TABLE [GST_STATEREGDET] (
        [GST_TINID] bigint NOT NULL IDENTITY (1, 1),
        [GST_ID] bigint NOT NULL,
        [GST_STATE] nvarchar(20) NULL,
        [GST_ADDRESS] nvarchar(200) NULL,
        [GST_VENDCITY] nvarchar(100) NULL,
        [GST_VENDCITYNAME] nvarchar(100) NULL,
        [GST_VENDPINCODE] nvarchar(6) NULL,
        [GST_TINNO] nvarchar(50) NULL,
        [GST_EXCNO] nvarchar(50) NULL,
        [GST_SERNO] nvarchar(50) NULL,
        [GST_GSTINNO] nvarchar(50) NULL,
        [GST_ARNNO] nvarchar(50) NULL,
        [GST_ARNCOPY] nvarchar(200) NULL,
        [GST_ARNTEMPFILE] nvarchar(200) NULL,
        [GST_CONTACTPERSON] nvarchar(100) NULL,
        [GST_EMAILID] nvarchar(100) NULL,
        [GST_MOBILENO] nvarchar(10) NULL,
        [GST_REMARKS] nvarchar(200) NULL,
        CONSTRAINT [PK_GST_STATEREGDET] PRIMARY KEY ([GST_TINID]),
        CONSTRAINT [FK_GST_STATEREGDET_GST_MAIN_GST_ID] FOREIGN KEY ([GST_ID]) 
            REFERENCES [GST_MAIN] ([GST_ID]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_GST_STATEREGDET_GST_ID] ON [GST_STATEREGDET] ([GST_ID]);
END
GO

-- ============================================================================
-- Migration History (EF Core Tracking)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL PRIMARY KEY,
        [ProductVersion] nvarchar(32) NOT NULL
    );
END
GO

-- Insert migration record if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260317000000_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317000000_InitialCreate', N'10.0.5');
END
GO

PRINT 'Migration completed successfully!' + CHAR(13) + CHAR(10) +
      'Created tables:' + CHAR(13) + CHAR(10) +
      '  - GST_SUPPLIER' + CHAR(13) + CHAR(10) +
      '  - GST_MAIN (with PAN unique constraint)' + CHAR(13) + CHAR(10) +
      '  - GST_HSNDET (cascading delete enabled)' + CHAR(13) + CHAR(10) +
      '  - GST_SERVDET (cascading delete enabled)' + CHAR(13) + CHAR(10) +
      '  - GST_STATEREGDET (cascading delete enabled)';
