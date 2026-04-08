namespace OrderScheduleService.Application.DTOs;

public class CreateScheduleDetailDto
{
    public DateTime FillingDate { get; set; }
    public string FillingShift { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public decimal FillQuantity { get; set; }
    public long FillingPointGroupId { get; set; }
}

public class CreateScheduleDto
{
    public long FillingPointGroupId { get; set; }
    public decimal ItemId { get; set; }
    public string OrderType { get; set; } = null!;
    public long OrderId { get; set; }
    public long OrderLineId { get; set; }
    public DateTime RequiredDate { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal ShiftCapacity { get; set; }
    public List<CreateScheduleDetailDto> Details { get; set; } = new();
}

public class ScheduleDetailDto
{
    public long Id { get; set; }
    public long ScheduleId { get; set; }
    public DateTime? FillingDate { get; set; }
    public string? FillingShift { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public decimal? FillQuantity { get; set; }
    public long? FillingPointGroupId { get; set; }
}

public class ScheduleDto
{
    public long Id { get; set; }
    public long FillingPointGroupId { get; set; }
    public decimal ItemId { get; set; }
    public string OrderType { get; set; } = null!;
    public long OrderId { get; set; }
    public long OrderLineId { get; set; }
    public DateTime RequiredDate { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal ShiftCapacity { get; set; }
    public decimal TotalAllocatedQuantity { get; set; }
    public List<ScheduleDetailDto> ScheduleDetails { get; set; } = new();
}
