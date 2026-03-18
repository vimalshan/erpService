using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinyearAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert sample financial year data
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = 1)
                BEGIN
                    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                    VALUES (1, 'FY 2024-2025', '2024-04-01', '2025-03-31', 1, GETUTCDATE());
                END;

                IF NOT EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = 2)
                BEGIN
                    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                    VALUES (2, 'FY 2025-2026', '2025-04-01', '2026-03-31', 1, GETUTCDATE());
                END;

                IF NOT EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = 3)
                BEGIN
                    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                    VALUES (3, 'FY 2026-2027', '2026-04-01', '2027-03-31', 1, GETUTCDATE());
                END;

                IF NOT EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = 4)
                BEGIN
                    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                    VALUES (4, 'FY 2023-2024', '2023-04-01', '2024-03-31', 1, GETUTCDATE());
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete sample data
            migrationBuilder.Sql(@"
                DELETE FROM FINYEAR_MASTER WHERE FY_ID IN (1, 2, 3, 4);
            ");
        }
    }
}
