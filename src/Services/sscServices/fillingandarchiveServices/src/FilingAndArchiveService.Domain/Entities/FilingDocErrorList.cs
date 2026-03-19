namespace FilingAndArchiveService.Domain.Entities;

public class FilingDocErrorList
{
    public string? DocKey { get; set; }
    public string? Remarks { get; set; }
    public long? SysId { get; set; }
    public DateTime? AccountingDate { get; set; }
    public string? Flag { get; set; }
    public string? Status { get; set; }
    public long? Sno { get; set; }
}
