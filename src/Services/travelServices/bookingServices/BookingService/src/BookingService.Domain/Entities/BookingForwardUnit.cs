namespace BookingService.Domain.Entities;

public class BookingForwardUnit
{
    public decimal BkBokNum { get; set; }
    public decimal BkSrlNum { get; set; }
    public long AdmUnit { get; set; }
    public long FwdAdmUnit { get; set; }
    public DateTime? FwdDate { get; set; }
}
