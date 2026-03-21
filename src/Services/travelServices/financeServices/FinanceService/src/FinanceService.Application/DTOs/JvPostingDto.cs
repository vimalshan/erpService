namespace FinanceService.Application.DTOs;

public class JvPostingDto
{
    public long JvIntCode { get; set; }
    public int JvDocNum { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string GradeType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Comment { get; set; }
    public long? PayNumber { get; set; }
    public DateTime? JvDate { get; set; }
}
