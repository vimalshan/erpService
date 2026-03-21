namespace FinanceService.Application.DTOs;

public class BatchDto
{
    public string UnitCode { get; set; } = string.Empty;
    public decimal BatchNumber { get; set; }
    public DateTime? BatchDate { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? BatchStatus { get; set; }
    public string? AdminRemarks { get; set; }
    public string? FinanceRemarks { get; set; }
    public decimal? AgencyCode { get; set; }
    public decimal? TotalApprovedAmount { get; set; }
    public decimal? Total { get; set; }
    public decimal? CgstAmount { get; set; }
    public decimal? SgstAmount { get; set; }
    public decimal? IgstAmount { get; set; }
    public List<BatchLineDto> Lines { get; set; } = new();
}

public class BatchLineDto
{
    public decimal SerialNumber { get; set; }
    public decimal? BookingNumber { get; set; }
    public decimal? TicketCost { get; set; }
    public decimal? TicketAdjustment { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
}
