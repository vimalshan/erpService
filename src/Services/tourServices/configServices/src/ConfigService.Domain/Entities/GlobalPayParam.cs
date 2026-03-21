using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GlobalPayParam : AggregateRoot<string>
{
    public string ParamCode { get; private set; } = string.Empty;
    public string ParamDescription { get; private set; } = string.Empty;
    public string ParamValue { get; private set; } = string.Empty;

    private GlobalPayParam() { }

    public static GlobalPayParam Create(string id, string code, string description, string value)
    {
        return new GlobalPayParam { Id = id, ParamCode = code, ParamDescription = description, ParamValue = value };
    }

    public void Update(string code, string description, string value)
    {
        ParamCode = code;
        ParamDescription = description;
        ParamValue = value;
    }
}
