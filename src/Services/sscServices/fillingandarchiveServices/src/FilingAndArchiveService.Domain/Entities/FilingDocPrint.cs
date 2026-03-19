namespace FilingAndArchiveService.Domain.Entities;

public class FilingDocPrint
{
    public long DocSeq { get; set; }
    public string DocKey { get; set; } = default!;
    public long DocFileNo { get; set; }
}
