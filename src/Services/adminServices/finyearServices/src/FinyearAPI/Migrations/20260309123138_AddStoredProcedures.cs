using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinyearAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create stored procedure: sp_GetFinancialYearById
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_GetFinancialYearById]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_GetFinancialYearById]
                        @FY_ID BIGINT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        SELECT 
                            FY_ID,
                            FY_NAME,
                            FY_STARTDATE,
                            FY_CLOSEDATE,
                            FY_UPDATED_BY,
                            FY_UPDATED_ON
                        FROM FINYEAR_MASTER
                        WHERE FY_ID = @FY_ID;
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_GetAllFinancialYears
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_GetAllFinancialYears]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_GetAllFinancialYears]
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        SELECT 
                            FY_ID,
                            FY_NAME,
                            FY_STARTDATE,
                            FY_CLOSEDATE,
                            FY_UPDATED_BY,
                            FY_UPDATED_ON
                        FROM FINYEAR_MASTER
                        ORDER BY FY_STARTDATE DESC;
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_CreateFinancialYear
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_CreateFinancialYear]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_CreateFinancialYear]
                        @FY_NAME VARCHAR(27),
                        @FY_STARTDATE DATETIME2(3),
                        @FY_CLOSEDATE DATETIME2(3),
                        @FY_UPDATED_BY BIGINT,
                        @FY_ID BIGINT OUTPUT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        INSERT INTO FINYEAR_MASTER (FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                        VALUES (@FY_NAME, @FY_STARTDATE, @FY_CLOSEDATE, @FY_UPDATED_BY, GETUTCDATE());
                        
                        SET @FY_ID = SCOPE_IDENTITY();
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_UpdateFinancialYear
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_UpdateFinancialYear]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_UpdateFinancialYear]
                        @FY_ID BIGINT,
                        @FY_NAME VARCHAR(27),
                        @FY_STARTDATE DATETIME2(3),
                        @FY_CLOSEDATE DATETIME2(3),
                        @FY_UPDATED_BY BIGINT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        UPDATE FINYEAR_MASTER
                        SET FY_NAME = @FY_NAME,
                            FY_STARTDATE = @FY_STARTDATE,
                            FY_CLOSEDATE = @FY_CLOSEDATE,
                            FY_UPDATED_BY = @FY_UPDATED_BY,
                            FY_UPDATED_ON = GETUTCDATE()
                        WHERE FY_ID = @FY_ID;
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_DeleteFinancialYear
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_DeleteFinancialYear]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_DeleteFinancialYear]
                        @FY_ID BIGINT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        DELETE FROM FINYEAR_MASTER WHERE FY_ID = @FY_ID;
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_IsFinancialYearActive
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_IsFinancialYearActive]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_IsFinancialYearActive]
                        @FY_ID BIGINT,
                        @IsActive BIT OUTPUT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        SET @IsActive = CASE 
                            WHEN EXISTS (
                                SELECT 1 FROM FINYEAR_MASTER 
                                WHERE FY_ID = @FY_ID 
                                AND FY_STARTDATE <= GETUTCDATE() 
                                AND FY_CLOSEDATE >= GETUTCDATE()
                            ) THEN 1 
                            ELSE 0 
                        END;
                    END
                    ')
                END
            ");

            // Create stored procedure: sp_GetFinancialYearByDateRange
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[sp_GetFinancialYearByDateRange]', N'P') IS NULL
                BEGIN
                    EXEC(N'
                    CREATE PROCEDURE [dbo].[sp_GetFinancialYearByDateRange]
                        @StartDate DATETIME2(3),
                        @EndDate DATETIME2(3)
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        
                        SELECT 
                            FY_ID,
                            FY_NAME,
                            FY_STARTDATE,
                            FY_CLOSEDATE,
                            FY_UPDATED_BY,
                            FY_UPDATED_ON
                        FROM FINYEAR_MASTER
                        WHERE FY_STARTDATE >= @StartDate AND FY_CLOSEDATE <= @EndDate
                        ORDER BY FY_STARTDATE;
                    END
                    ')
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop all stored procedures in reverse order
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GetFinancialYearByDateRange];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_IsFinancialYearActive];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_DeleteFinancialYear];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_UpdateFinancialYear];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_CreateFinancialYear];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GetAllFinancialYears];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GetFinancialYearById];");
        }
    }
}
