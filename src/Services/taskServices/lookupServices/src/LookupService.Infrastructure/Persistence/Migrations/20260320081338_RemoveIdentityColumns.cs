using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LookupService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server requires drop/recreate to remove IDENTITY from a column.
            // Drop dependent objects, recreate LOV_MASTER without IDENTITY, then restore.
            migrationBuilder.Sql(@"
                -- Drop FK and indexes referencing LOV_MASTER
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID')
                    ALTER TABLE [LOV_UNITMAP] DROP CONSTRAINT [FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID')
                    ALTER TABLE [LOV_PANELMAP] DROP CONSTRAINT [FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE')
                    ALTER TABLE [LOV_MASTER] DROP CONSTRAINT [FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE];

                -- Save data
                SELECT * INTO #LOV_MASTER_BACKUP FROM [LOV_MASTER];

                -- Drop and recreate without IDENTITY
                DROP TABLE [LOV_MASTER];

                CREATE TABLE [LOV_MASTER] (
                    [LOV_ID] BIGINT NOT NULL,
                    [LOV_TYPE] CHAR(3) NULL,
                    [LOV_NAME] NVARCHAR(200) NULL,
                    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
                );

                -- Restore data
                INSERT INTO [LOV_MASTER] ([LOV_ID], [LOV_TYPE], [LOV_NAME])
                SELECT [LOV_ID], [LOV_TYPE], [LOV_NAME] FROM #LOV_MASTER_BACKUP;
                DROP TABLE #LOV_MASTER_BACKUP;

                -- Restore index
                CREATE INDEX [IX_LOV_MASTER_LOV_TYPE] ON [LOV_MASTER] ([LOV_TYPE]);

                -- Restore FK constraints
                ALTER TABLE [LOV_MASTER] ADD CONSTRAINT [FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE]
                    FOREIGN KEY ([LOV_TYPE]) REFERENCES [LOV_TYPEMASTER] ([LOV_TYPECODE]);
                ALTER TABLE [LOV_UNITMAP] ADD CONSTRAINT [FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID]
                    FOREIGN KEY ([LU_LOVID]) REFERENCES [LOV_MASTER] ([LOV_ID]);
                ALTER TABLE [LOV_PANELMAP] ADD CONSTRAINT [FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID]
                    FOREIGN KEY ([LP_LOVID]) REFERENCES [LOV_MASTER] ([LOV_ID]);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Drop FK and indexes referencing LOV_MASTER
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID')
                    ALTER TABLE [LOV_UNITMAP] DROP CONSTRAINT [FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID')
                    ALTER TABLE [LOV_PANELMAP] DROP CONSTRAINT [FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE')
                    ALTER TABLE [LOV_MASTER] DROP CONSTRAINT [FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE];

                SELECT * INTO #LOV_MASTER_BACKUP FROM [LOV_MASTER];
                DROP TABLE [LOV_MASTER];

                CREATE TABLE [LOV_MASTER] (
                    [LOV_ID] BIGINT NOT NULL IDENTITY(1,1),
                    [LOV_TYPE] CHAR(3) NULL,
                    [LOV_NAME] NVARCHAR(200) NULL,
                    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
                );

                SET IDENTITY_INSERT [LOV_MASTER] ON;
                INSERT INTO [LOV_MASTER] ([LOV_ID], [LOV_TYPE], [LOV_NAME])
                SELECT [LOV_ID], [LOV_TYPE], [LOV_NAME] FROM #LOV_MASTER_BACKUP;
                SET IDENTITY_INSERT [LOV_MASTER] OFF;
                DROP TABLE #LOV_MASTER_BACKUP;

                CREATE INDEX [IX_LOV_MASTER_LOV_TYPE] ON [LOV_MASTER] ([LOV_TYPE]);

                ALTER TABLE [LOV_MASTER] ADD CONSTRAINT [FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE]
                    FOREIGN KEY ([LOV_TYPE]) REFERENCES [LOV_TYPEMASTER] ([LOV_TYPECODE]);
                ALTER TABLE [LOV_UNITMAP] ADD CONSTRAINT [FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID]
                    FOREIGN KEY ([LU_LOVID]) REFERENCES [LOV_MASTER] ([LOV_ID]);
                ALTER TABLE [LOV_PANELMAP] ADD CONSTRAINT [FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID]
                    FOREIGN KEY ([LP_LOVID]) REFERENCES [LOV_MASTER] ([LOV_ID]);
            ");
        }
    }
}
