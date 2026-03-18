namespace BatchService.Application.DTOs;

public sealed class BatchDto
{
    public long     BatchId             { get; init; }
    public int      BatchMonthNo        { get; init; }
    public char     BatchStatus         { get; init; }
    public string   BatchStatusLabel    { get; init; } = string.Empty;
    public long     BatchLastModifiedBy { get; init; }
    public DateTime BatchLastModifiedOn { get; init; }
}
