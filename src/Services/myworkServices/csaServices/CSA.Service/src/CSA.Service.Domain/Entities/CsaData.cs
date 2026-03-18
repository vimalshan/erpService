namespace CSA.Service.Domain.Entities;

public class CsaData
{
    public string? Title { get; set; }
    public string? ControlMethod { get; set; }
    public string? ControlType { get; set; }
    public string? Priority { get; set; }
    public string? ControlDescription { get; set; }
    public string? Risk { get; set; }
    public string? ApprovalRequired { get; set; }
    public string? ControlRecordRequired { get; set; }
    public string? FrequencyOfControl { get; set; }
    public string? Periodicity { get; set; }
    public string? Process { get; set; }
    public string? SubProcess { get; set; }
    public decimal? Created { get; set; }
    public decimal? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public long? Id { get; set; }
    public string? ItemType { get; set; }
    public string? Path { get; set; }
}
