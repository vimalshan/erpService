namespace WarehouseStructure.Application.DTOs;

public class CreateZoneDto
{
    public int WarehouseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ZoneType { get; set; } = string.Empty;
    public string? Description { get; set; }
}
