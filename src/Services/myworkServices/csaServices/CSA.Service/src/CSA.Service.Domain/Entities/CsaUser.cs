namespace CSA.Service.Domain.Entities;

public class CsaUser
{
    public decimal EmployeeNo { get; set; }
    public decimal? PinNumber { get; set; }
    public string? Name { get; set; }
    public long? SystemId { get; set; }
    public string? Email { get; set; }
}
