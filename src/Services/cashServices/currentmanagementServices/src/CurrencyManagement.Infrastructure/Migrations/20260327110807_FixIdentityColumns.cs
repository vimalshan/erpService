using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server cannot ALTER COLUMN to remove IDENTITY.
            // Must recreate tables. Order matters due to FK dependencies:
            // DEAL_CURRATES -> DEAL_CURRMAST (FK_DEAL_CURRATES_DEAL_CURRMAST_CurrencyId)
            // DEAL_ORGCURRMAP -> DEAL_CURRMAST (FK_DEAL_ORGCURRMAP_CURRMAST)
            // So we must drop ALL FKs to DEAL_CURRMAST first, then recreate both tables.

            migrationBuilder.Sql(@"
                -- Step 1: Drop ALL foreign keys referencing DEAL_CURRMAST
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DEAL_CURRATES_DEAL_CURRMAST_CurrencyId')
                    ALTER TABLE DEAL_CURRATES DROP CONSTRAINT FK_DEAL_CURRATES_DEAL_CURRMAST_CurrencyId;

                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DEAL_ORGCURRMAP_CURRMAST')
                    ALTER TABLE DEAL_ORGCURRMAP DROP CONSTRAINT FK_DEAL_ORGCURRMAP_CURRMAST;

                -- Step 2: Recreate DEAL_CURRATES without IDENTITY
                CREATE TABLE DEAL_CURRATES_NEW (
                    CURRATE_ID BIGINT NOT NULL,
                    CURRATE_FINYEAR BIGINT NOT NULL,
                    CURRATE_MONTH BIGINT NOT NULL,
                    CURRATE_FROMCUR BIGINT NOT NULL,
                    CURRATE_TOCUR BIGINT NOT NULL,
                    CURRATE_RATE DECIMAL(19,6) NOT NULL,
                    CURRATE_MODIFIEDBY BIGINT NOT NULL,
                    CURRATE_MODIFIEDON DATETIME2(3) NOT NULL,
                    CurrencyId BIGINT NULL,
                    CONSTRAINT PK_DEAL_CURRATES_NEW PRIMARY KEY (CURRATE_ID)
                );

                INSERT INTO DEAL_CURRATES_NEW (CURRATE_ID, CURRATE_FINYEAR, CURRATE_MONTH, CURRATE_FROMCUR, CURRATE_TOCUR, CURRATE_RATE, CURRATE_MODIFIEDBY, CURRATE_MODIFIEDON, CurrencyId)
                SELECT CURRATE_ID, CURRATE_FINYEAR, CURRATE_MONTH, CURRATE_FROMCUR, CURRATE_TOCUR, CURRATE_RATE, CURRATE_MODIFIEDBY, CURRATE_MODIFIEDON, CurrencyId FROM DEAL_CURRATES;

                DROP TABLE DEAL_CURRATES;

                EXEC sp_rename 'DEAL_CURRATES_NEW', 'DEAL_CURRATES';
                EXEC sp_rename 'PK_DEAL_CURRATES_NEW', 'PK_DEAL_CURRATES', 'OBJECT';

                -- Step 3: Recreate DEAL_CURRMAST without IDENTITY
                CREATE TABLE DEAL_CURRMAST_NEW (
                    CURR_ID BIGINT NOT NULL,
                    CURR_NAME NVARCHAR(255) NOT NULL,
                    CURR_SYMBOL NVARCHAR(25) NOT NULL,
                    CURR_MODIFIEDBY BIGINT NOT NULL,
                    CURR_MODIFIEDON DATETIME2(3) NOT NULL,
                    CONSTRAINT PK_DEAL_CURRMAST_NEW PRIMARY KEY (CURR_ID)
                );

                INSERT INTO DEAL_CURRMAST_NEW (CURR_ID, CURR_NAME, CURR_SYMBOL, CURR_MODIFIEDBY, CURR_MODIFIEDON)
                SELECT CURR_ID, CURR_NAME, CURR_SYMBOL, CURR_MODIFIEDBY, CURR_MODIFIEDON FROM DEAL_CURRMAST;

                DROP TABLE DEAL_CURRMAST;

                EXEC sp_rename 'DEAL_CURRMAST_NEW', 'DEAL_CURRMAST';
                EXEC sp_rename 'PK_DEAL_CURRMAST_NEW', 'PK_DEAL_CURRMAST', 'OBJECT';

                -- Step 4: Re-add all foreign keys and indexes
                ALTER TABLE DEAL_ORGCURRMAP ADD CONSTRAINT FK_DEAL_ORGCURRMAP_CURRMAST
                    FOREIGN KEY (ORG_CURRID) REFERENCES DEAL_CURRMAST(CURR_ID) ON DELETE CASCADE;

                ALTER TABLE DEAL_CURRATES ADD CONSTRAINT FK_DEAL_CURRATES_DEAL_CURRMAST_CurrencyId
                    FOREIGN KEY (CurrencyId) REFERENCES DEAL_CURRMAST(CURR_ID);

                CREATE INDEX IX_DEAL_CURRATES_FINYEAR_MONTH ON DEAL_CURRATES (CURRATE_FINYEAR, CURRATE_MONTH);
                CREATE INDEX IX_DEAL_CURRATES_FROMCUR_TOCUR ON DEAL_CURRATES (CURRATE_FROMCUR, CURRATE_TOCUR);
                CREATE INDEX IX_DEAL_CURRATES_CurrencyId ON DEAL_CURRATES (CurrencyId);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverting to IDENTITY columns is complex; not implementing for dev environment
        }
    }
}
