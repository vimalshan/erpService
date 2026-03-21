using AgencyService.Domain.Common;

namespace AgencyService.Domain.Entities;

public class Airline : AggregateRoot
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    
    public Airline(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 2 || code.Length > 3)
            throw new ArgumentException("Airline code must be 2 or 3 characters", nameof(code));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Airline name cannot be empty", nameof(name));
            
        Code = code.ToUpper();
        Name = name;
        
        AddDomainEvent(new AirlineRegisteredEvent(code, name));
    }
    
    private Airline() { }
}

public class AirlineRegisteredEvent : DomainEvent
{
    public string AirlineCode { get; set; }
    public string AirlineName { get; set; }
    
    public AirlineRegisteredEvent(string airlineCode, string airlineName)
    {
        AirlineCode = airlineCode;
        AirlineName = airlineName;
    }
}
