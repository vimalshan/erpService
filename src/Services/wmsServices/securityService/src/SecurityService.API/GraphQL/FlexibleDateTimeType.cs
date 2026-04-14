using System.Globalization;
using HotChocolate.Language;
using HotChocolate.Types;

namespace SecurityService.API.GraphQL;

public class FlexibleDateTimeType : ScalarType<DateTime, StringValueNode>
{
    public FlexibleDateTimeType() : base("DateTime")
    {
        Description = "DateTime scalar that accepts ISO 8601 with or without timezone suffix.";
    }

    protected override DateTime ParseLiteral(StringValueNode valueSyntax)
    {
        if (DateTime.TryParse(valueSyntax.Value, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var dt))
            return dt;

        throw new SerializationException(
            $"Cannot parse `{valueSyntax.Value}` as DateTime.", this);
    }

    protected override StringValueNode ParseValue(DateTime runtimeValue)
        => new(runtimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));

    public override IValueNode ParseResult(object? resultValue) => resultValue switch
    {
        DateTime dt => ParseValue(dt),
        DateTimeOffset dto => ParseValue(dto.UtcDateTime),
        string s when DateTime.TryParse(s, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var parsed)
            => ParseValue(parsed),
        null => NullValueNode.Default,
        _ => throw new SerializationException(
            $"Cannot parse result value of type `{resultValue.GetType()}`.", this)
    };

    public override bool TrySerialize(object? runtimeValue, out object? resultValue)
    {
        switch (runtimeValue)
        {
            case DateTime dt:
                resultValue = dt.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
                return true;
            case null:
                resultValue = null;
                return true;
            default:
                resultValue = null;
                return false;
        }
    }

    public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
    {
        switch (resultValue)
        {
            case string s when DateTime.TryParse(s, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var dt):
                runtimeValue = dt;
                return true;
            case DateTime dt:
                runtimeValue = dt;
                return true;
            case DateTimeOffset dto:
                runtimeValue = dto.UtcDateTime;
                return true;
            case null:
                runtimeValue = null;
                return true;
            default:
                runtimeValue = null;
                return false;
        }
    }
}
