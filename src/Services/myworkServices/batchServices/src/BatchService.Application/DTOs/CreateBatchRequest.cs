namespace BatchService.Application.DTOs;

public sealed class CreateBatchRequest
{
    public long BatchId    { get; init; }
    public int  MonthNo    { get; init; }
    public long ModifiedBy { get; init; }
}
