namespace FilingAndArchiveService.Domain.Entities;

public class FilingCounter
{
    public string FilingBuId { get; set; } = default!;
    public long FileCount { get; set; }

    public long NextCount()
    {
        FileCount++;
        return FileCount;
    }
}
