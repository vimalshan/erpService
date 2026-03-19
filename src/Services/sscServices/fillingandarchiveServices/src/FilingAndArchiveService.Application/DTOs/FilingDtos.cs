namespace FilingAndArchiveService.Application.DTOs;

public class FilingCounterDto
{
    public string FilingBuId { get; set; } = default!;
    public long FileCount { get; set; }
}

public class FilingDocPrintDto
{
    public long DocSeq { get; set; }
    public string DocKey { get; set; } = default!;
    public long DocFileNo { get; set; }
}

public class FilingDocErrorDto
{
    public string? DocKey { get; set; }
    public string? Remarks { get; set; }
    public long? SysId { get; set; }
    public DateTime? AccountingDate { get; set; }
    public string? Flag { get; set; }
    public string? Status { get; set; }
    public long? Sno { get; set; }
}
