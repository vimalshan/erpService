using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderScheduleService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTUAL_ORDER_MAP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiedOrderDetailId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ActualLineId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MappingQuantity = table.Column<int>(type: "int", nullable: true),
                    SciUserIdModified = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACTUAL_ORDER_MAP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_ACTUAL_ORDER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    HeaderId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LineId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    OrderedItem = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OrderedItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualShipmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    OrderQuantityUom = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CancelledQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    FulfilledQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ShippingQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ShippingQuantityUom = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    InvoicedQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ShipFromOrgId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SoldFromOrgId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SoldToOrgId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipToOrgId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ConsigneeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustPoNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderSourceId = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_ACTUAL_ORDER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_ACTUAL_ORDER_SCHEDULE",
                columns: table => new
                {
                    CtOrderId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LineId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    OrderedItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    NewScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewFillQuantity = table.Column<int>(type: "int", nullable: true),
                    FillingAllotted = table.Column<int>(type: "int", nullable: true),
                    SciUserIdModified = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_ACTUAL_ORDER_SCHEDULE", x => new { x.CtOrderId, x.LineId });
                });

            migrationBuilder.CreateTable(
                name: "OS_CAPACITY_CHANGES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FillingLineId = table.Column<int>(type: "int", nullable: true),
                    FillingGroupId = table.Column<int>(type: "int", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RerunStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RerunDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_CAPACITY_CHANGES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_EMPTIES_ORDER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SciItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    OrderQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    NeedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_EMPTIES_ORDER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_SCHEDULE_CONFIRM",
                columns: table => new
                {
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_SCHEDULE_CONFIRM", x => x.ScheduleDate);
                });

            migrationBuilder.CreateTable(
                name: "OS_SCHEDULE_MASTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FillingPointGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    OrderLineId = table.Column<long>(type: "bigint", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ShiftCapacity = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    TotalAllocatedQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_SCHEDULE_MASTER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_SHIFT_MASTER",
                columns: table => new
                {
                    ShiftCode = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    CompanyUnitId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ShiftDescription = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StartDay = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    EndDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_SHIFT_MASTER", x => new { x.ShiftCode, x.CompanyUnitId });
                });

            migrationBuilder.CreateTable(
                name: "OS_TIED_ORDER_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyUnitId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RecordStatus = table.Column<string>(type: "char(1)", nullable: false),
                    ModifiedSciUserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderNumberCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LcNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_TIED_ORDER_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OS_SCHEDULE_DETAILS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    FillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FillingShift = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    FillQuantity = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    FillingPointGroupId = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ReferenceScheduleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_SCHEDULE_DETAILS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OS_SCHEDULE_DETAILS_OS_SCHEDULE_MASTER_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "OS_SCHEDULE_MASTER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OS_TIED_ORDER_DETAILS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiedOrderId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OrderQuantity = table.Column<long>(type: "bigint", nullable: false),
                    DispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuantityFromCurrentStock = table.Column<long>(type: "bigint", nullable: true),
                    FillingAllotted = table.Column<long>(type: "bigint", nullable: true),
                    CancelFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(19,4)", nullable: true),
                    ScheduleFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IgnoreEmptiesCheck = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IgnoreCurrentStock = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS_TIED_ORDER_DETAILS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OS_TIED_ORDER_DETAILS_OS_TIED_ORDER_HEADER_TiedOrderId",
                        column: x => x.TiedOrderId,
                        principalTable: "OS_TIED_ORDER_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OS_SHIFT_MASTER",
                columns: new[] { "CompanyUnitId", "ShiftCode", "EndDay", "EndTime", "ShiftDescription", "StartDay", "StartTime" },
                values: new object[,]
                {
                    { 1m, "A", 0, "14:00", "Morning Shift", 0, "06:00" },
                    { 1m, "B", 0, "22:00", "Afternoon Shift", 0, "14:00" },
                    { 1m, "C", 1, "06:00", "Night Shift", 0, "22:00" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OS_SCHEDULE_DETAILS_ScheduleId",
                table: "OS_SCHEDULE_DETAILS",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OS_TIED_ORDER_DETAILS_TiedOrderId",
                table: "OS_TIED_ORDER_DETAILS",
                column: "TiedOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTUAL_ORDER_MAP");

            migrationBuilder.DropTable(
                name: "OS_ACTUAL_ORDER");

            migrationBuilder.DropTable(
                name: "OS_ACTUAL_ORDER_SCHEDULE");

            migrationBuilder.DropTable(
                name: "OS_CAPACITY_CHANGES");

            migrationBuilder.DropTable(
                name: "OS_EMPTIES_ORDER");

            migrationBuilder.DropTable(
                name: "OS_SCHEDULE_CONFIRM");

            migrationBuilder.DropTable(
                name: "OS_SCHEDULE_DETAILS");

            migrationBuilder.DropTable(
                name: "OS_SHIFT_MASTER");

            migrationBuilder.DropTable(
                name: "OS_TIED_ORDER_DETAILS");

            migrationBuilder.DropTable(
                name: "OS_SCHEDULE_MASTER");

            migrationBuilder.DropTable(
                name: "OS_TIED_ORDER_HEADER");
        }
    }
}
