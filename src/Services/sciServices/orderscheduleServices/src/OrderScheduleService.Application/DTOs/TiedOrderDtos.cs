namespace OrderScheduleService.Application.DTOs;

public class CreateOrderDetailsDto
{
    public decimal ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public long OrderQuantity { get; set; }
    public DateTime? DispatchDate { get; set; }
    public decimal? Price { get; set; }
}

public class CreateTiedOrderDto
{
    public string CustomerCode { get; set; } = null!;
    public decimal CompanyUnitId { get; set; }
    public string ModifiedUserId { get; set; } = null!;
    public List<CreateOrderDetailsDto> Details { get; set; } = new();
}

public class TiedOrderDetailDto
{
    public long Id { get; set; }
    public long TiedOrderId { get; set; }
    public decimal ItemId { get; set; }
    public string? ItemName { get; set; }
    public long OrderQuantity { get; set; }
    public DateTime? DispatchDate { get; set; }
    public long? FillingAllotted { get; set; }
    public string? CancelFlag { get; set; }
    public decimal? Price { get; set; }
}

public class TiedOrderDto
{
    public long Id { get; set; }
    public string CustomerCode { get; set; } = null!;
    public DateTime OrderedDate { get; set; }
    public decimal CompanyUnitId { get; set; }
    public char RecordStatus { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<TiedOrderDetailDto> Details { get; set; } = new();
}
