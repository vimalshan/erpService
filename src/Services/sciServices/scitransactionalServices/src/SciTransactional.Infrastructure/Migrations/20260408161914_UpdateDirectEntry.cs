using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SciTransactional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDirectEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VEHICLE_DIRECT_ENTRY_ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VEHICLE_DIRECT_ENTRY_ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VEHICLE_DIRECT_ENTRY_ID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "DIR_ENTERED_BY",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.DropColumn(
                name: "DIR_NO_OF_STG",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.DropColumn(
                name: "DIR_STATUS",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.DropColumn(
                name: "DIR_STG_NEW",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.DropColumn(
                name: "DIR_STG_OLD",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.RenameColumn(
                name: "DIR_TRK_NUM",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "VDE_TRK_NUM");

            migrationBuilder.RenameColumn(
                name: "VEHICLE_DIRECT_ENTRY_ID",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "VDE_ID");

            migrationBuilder.RenameColumn(
                name: "DIR_ENTERED_ON",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "VDE_ENT_DAT");

            migrationBuilder.AlterColumn<long>(
                name: "VDE_TRK_NUM",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VDE_ID",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "VDE_ENT_USR",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "VEHICLE_DIRECT_ENTRY",
                columns: new[] { "VDE_ID", "VDE_ENT_DAT", "VDE_ENT_USR", "VDE_TRK_NUM" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "ADMIN", 1001L },
                    { 2L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "GATE_USER", 1002L },
                    { 3L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "WB_USER", 1003L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VDE_ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VDE_ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "VEHICLE_DIRECT_ENTRY",
                keyColumn: "VDE_ID",
                keyValue: 3L);

            migrationBuilder.DropColumn(
                name: "VDE_ENT_USR",
                table: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.RenameColumn(
                name: "VDE_TRK_NUM",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "DIR_TRK_NUM");

            migrationBuilder.RenameColumn(
                name: "VDE_ID",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "VEHICLE_DIRECT_ENTRY_ID");

            migrationBuilder.RenameColumn(
                name: "VDE_ENT_DAT",
                table: "VEHICLE_DIRECT_ENTRY",
                newName: "DIR_ENTERED_ON");

            migrationBuilder.AlterColumn<decimal>(
                name: "DIR_TRK_NUM",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VEHICLE_DIRECT_ENTRY_ID",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "DIR_ENTERED_BY",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DIR_NO_OF_STG",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DIR_STATUS",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DIR_STG_NEW",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DIR_STG_OLD",
                table: "VEHICLE_DIRECT_ENTRY",
                type: "bigint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "VEHICLE_DIRECT_ENTRY",
                columns: new[] { "VEHICLE_DIRECT_ENTRY_ID", "DIR_ENTERED_BY", "DIR_ENTERED_ON", "DIR_STG_NEW", "DIR_NO_OF_STG", "DIR_STG_OLD", "DIR_STATUS", "DIR_TRK_NUM" },
                values: new object[,]
                {
                    { 1, 1L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3L, 2L, 1L, "A", 1001m },
                    { 2, 1L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 5L, 3L, 2L, "A", 1002m },
                    { 3, 2L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6L, 5L, 1L, "C", 1003m }
                });
        }
    }
}
