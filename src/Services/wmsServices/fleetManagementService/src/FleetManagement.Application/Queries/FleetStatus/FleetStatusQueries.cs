using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.FleetStatus;

public record GetFleetStatusQuery(int? WarehouseId) : IRequest<IEnumerable<FleetStatusDto>>;

public class GetFleetStatusHandler(IDapperQueryService dapper) : IRequestHandler<GetFleetStatusQuery, IEnumerable<FleetStatusDto>>
{
    public async Task<IEnumerable<FleetStatusDto>> Handle(GetFleetStatusQuery request, CancellationToken ct)
    {
        const string sql = """
            SELECT 
                v.vehicle_id AS VehicleId,
                v.code AS Code,
                v.license_plate AS LicensePlate,
                v.vehicle_type AS VehicleType,
                v.status AS Status,
                NULL AS HomeWarehouse,
                (SELECT COUNT(*) FROM Trip WHERE vehicle_id = v.vehicle_id AND status = 'IN_PROGRESS') AS ActiveTrips,
                (SELECT TOP 1 maintenance_date FROM MaintenanceLog WHERE vehicle_id = v.vehicle_id ORDER BY maintenance_date DESC) AS LastMaintenance,
                (SELECT TOP 1 next_due_date FROM MaintenanceLog WHERE vehicle_id = v.vehicle_id ORDER BY maintenance_date DESC) AS NextMaintenanceDue
            FROM Vehicle v
            WHERE (@WarehouseId IS NULL OR v.warehouse_id = @WarehouseId)
            ORDER BY v.status, v.code
            """;

        return await dapper.QueryAsync<FleetStatusDto>(sql, new { request.WarehouseId }, ct);
    }
}
