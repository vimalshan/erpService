using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    driver_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    license_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    license_expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.driver_id);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    route_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    route_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    start_location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    end_location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estimated_duration = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.route_id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    vehicle_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    license_plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    vehicle_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    year = table.Column<int>(type: "int", nullable: true),
                    capacity_weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    capacity_volume = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "AVAILABLE"),
                    warehouse_id = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.vehicle_id);
                });

            migrationBuilder.CreateTable(
                name: "FuelLog",
                columns: table => new
                {
                    fuel_log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    fuel_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    gallons = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    odometer_reading = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLog", x => x.fuel_log_id);
                    table.ForeignKey(
                        name: "FK_FuelLog_Vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLog",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    maintenance_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    maintenance_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    odometer_reading = table.Column<int>(type: "int", nullable: true),
                    next_due_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    performed_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLog", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_MaintenanceLog_Vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    trip_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trip_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    route_id = table.Column<int>(type: "int", nullable: true),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    driver_id = table.Column<int>(type: "int", nullable: false),
                    trip_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    origin_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    origin_id = table.Column<int>(type: "int", nullable: true),
                    destination_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    destination_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "PLANNED"),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.trip_id);
                    table.ForeignKey(
                        name: "FK_Trip_Driver_driver_id",
                        column: x => x.driver_id,
                        principalTable: "Driver",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trip_Route_route_id",
                        column: x => x.route_id,
                        principalTable: "Route",
                        principalColumn: "route_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Trip_Vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripStop",
                columns: table => new
                {
                    stop_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trip_id = table.Column<int>(type: "int", nullable: false),
                    stop_sequence = table.Column<int>(type: "int", nullable: false),
                    stop_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    location_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    location_id = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    planned_arrival = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_arrival = table.Column<DateTime>(type: "datetime2", nullable: true),
                    planned_departure = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_departure = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStop", x => x.stop_id);
                    table.ForeignKey(
                        name: "FK_TripStop_Trip_trip_id",
                        column: x => x.trip_id,
                        principalTable: "Trip",
                        principalColumn: "trip_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Driver_code",
                table: "Driver",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Driver_Employee",
                table: "Driver",
                column: "employee_id",
                unique: true,
                filter: "[employee_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLog_Vehicle",
                table: "FuelLog",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLog_Vehicle",
                table: "MaintenanceLog",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Route_route_name",
                table: "Route",
                column: "route_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_Driver",
                table: "Trip",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_Route",
                table: "Trip",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_Status",
                table: "Trip",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_trip_number",
                table: "Trip",
                column: "trip_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_Vehicle",
                table: "Trip",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_TripStop_Trip",
                table: "TripStop",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "UQ_TripStop_Sequence",
                table: "TripStop",
                columns: new[] { "trip_id", "stop_sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_code",
                table: "Vehicle",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_license_plate",
                table: "Vehicle",
                column: "license_plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Status",
                table: "Vehicle",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Warehouse",
                table: "Vehicle",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelLog");

            migrationBuilder.DropTable(
                name: "MaintenanceLog");

            migrationBuilder.DropTable(
                name: "TripStop");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
