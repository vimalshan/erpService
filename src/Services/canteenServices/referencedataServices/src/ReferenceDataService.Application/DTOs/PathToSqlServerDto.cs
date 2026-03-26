namespace ReferenceDataService.Application.DTOs;

public class PathToSqlServerDto
{
    public int Id { get; set; }
    public string? CompanyCode { get; set; }
    public string? ServerName { get; set; }
    public string? DatabaseName { get; set; }
    public string? UserId { get; set; }
}
