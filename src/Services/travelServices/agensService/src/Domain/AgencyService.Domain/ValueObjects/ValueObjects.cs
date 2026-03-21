using AgencyService.Domain.Common;

namespace AgencyService.Domain.ValueObjects;

public class Address : ValueObject
{
    public string AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    
    public Address(string addressLine1, string? addressLine2 = null, string? addressLine3 = null, string? addressLine4 = null)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty", nameof(addressLine1));
            
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        AddressLine3 = addressLine3;
        AddressLine4 = addressLine4;
    }
    
    public override IEnumerable<object?> GetAtomicValues()
    {
        yield return AddressLine1;
        yield return AddressLine2;
        yield return AddressLine3;
        yield return AddressLine4;
    }
}

public class ContactInfo : ValueObject
{
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string? AlternatePhone { get; private set; }
    
    public ContactInfo(string email, string phone, string? alternatePhone = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
            
        if (!email.Contains("@"))
            throw new ArgumentException("Email format is invalid", nameof(email));
            
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty", nameof(phone));
            
        Email = email;
        Phone = phone;
        AlternatePhone = alternatePhone;
    }
    
    public override IEnumerable<object?> GetAtomicValues()
    {
        yield return Email;
        yield return Phone;
        yield return AlternatePhone;
    }
}

public class AgencyType : ValueObject
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    
    public static readonly AgencyType Air = new("Air", "Airline");
    public static readonly AgencyType Train = new("Train", "Train Services");
    public static readonly AgencyType Bus = new("Bus", "Bus Services");
    public static readonly AgencyType Cab = new("Cab", "Taxi Services");
    
    private AgencyType(string code, string name)
    {
        Code = code;
        Name = name;
    }
    
    public static AgencyType Create(string code)
    {
        return code switch
        {
            "Air" => Air,
            "Train" => Train,
            "Bus" => Bus,
            "Cab" => Cab,
            _ => throw new ArgumentException($"Invalid agency type: {code}")
        };
    }
    
    public override IEnumerable<object?> GetAtomicValues()
    {
        yield return Code;
        yield return Name;
    }
}
