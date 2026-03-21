using AgencyService.Application.Common;

namespace AgencyService.Application.DTOs;

public class AgencyDto
{
    public long AgencyCode { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public class CreateAgencyDto
{
    public long AgencyCode { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
}

public class UpdateAgencyDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}

public class VendorDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string CategoryType { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
}

public class CreateVendorDto
{
    public long VendorId { get; set; }
    public required string Name { get; set; }
    public required string CategoryType { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address1 { get; set; }
    public long? CityCode { get; set; }
    public string? PAN { get; set; }
}

public class AirlineDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}

public class CreateAirlineDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}
