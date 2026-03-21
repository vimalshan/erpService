using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class TravelConveyance : BaseEntity
{
    public long SerialNumber { get; set; }
    public long RequestNumber { get; set; }
    public DateTime? Date { get; set; }
    public string? Particulars { get; set; }
    public long? Mode { get; set; }
    public long? Amount { get; set; }
    public long? BookRequestNumber { get; set; }
    public string? BookStatus { get; set; }
}
