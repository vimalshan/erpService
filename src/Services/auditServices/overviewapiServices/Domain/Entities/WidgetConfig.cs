namespace OverviewService.Domain.Entities;

public class WidgetConfig
{
    public int Id { get; set; }
    public string WidgetKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public int DisplayOrder { get; set; }
    public string? Configuration { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
