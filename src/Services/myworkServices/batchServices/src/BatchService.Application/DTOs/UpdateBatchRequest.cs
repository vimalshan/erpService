namespace BatchService.Application.DTOs;

public sealed class UpdateBatchRequest
{
    public int  MonthNo    { get; init; }
    public long ModifiedBy { get; init; }
}
