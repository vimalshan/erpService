namespace TransactionService.Domain.Entities;

public class SaaLevel : BaseEntity
{
    public string LevelDesc { get; set; } = string.Empty;
    public string LevelAmount { get; set; } = string.Empty;
    public string LevelReason { get; set; } = string.Empty;
    public decimal LevelMin { get; set; }
    public decimal LevelMax { get; set; }
    public DateTime LevelEffDate { get; set; }
    public DateTime? LevelCloseDate { get; set; }
    public long LevelUpdatedBy { get; set; }
    public DateTime LevelUpdatedOn { get; set; }

    public SaaLevel() { }

    public SaaLevel(string levelDesc, string levelAmount, string levelReason, decimal levelMin, decimal levelMax, DateTime levelEffDate, long updatedBy)
    {
        LevelDesc = levelDesc;
        LevelAmount = levelAmount;
        LevelReason = levelReason;
        LevelMin = levelMin;
        LevelMax = levelMax;
        LevelEffDate = levelEffDate;
        LevelUpdatedBy = updatedBy;
        LevelUpdatedOn = DateTime.UtcNow;
    }
}
