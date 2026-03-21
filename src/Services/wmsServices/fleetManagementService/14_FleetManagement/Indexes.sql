-- ======================================================
-- Module: Fleet Management - Indexes
-- ======================================================
CREATE INDEX IX_Vehicle_Warehouse ON Vehicle(warehouse_id);
CREATE INDEX IX_Vehicle_Status ON Vehicle(status);
CREATE INDEX IX_Driver_Employee ON Driver(employee_id);
CREATE INDEX IX_Trip_Vehicle ON Trip(vehicle_id);
CREATE INDEX IX_Trip_Driver ON Trip(driver_id);
CREATE INDEX IX_Trip_Route ON Trip(route_id);
CREATE INDEX IX_Trip_Status ON Trip(status);
CREATE INDEX IX_TripStop_Trip ON TripStop(trip_id);
CREATE INDEX IX_MaintenanceLog_Vehicle ON MaintenanceLog(vehicle_id);
CREATE INDEX IX_FuelLog_Vehicle ON FuelLog(vehicle_id);
GO
