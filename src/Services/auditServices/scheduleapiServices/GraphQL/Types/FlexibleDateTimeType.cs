using HotChocolate.Language;
using HotChocolate.Types;

namespace ScheduleService.GraphQL.Types
{
    public class FlexibleDateTimeType : ScalarType<DateTime, StringValueNode>
    {
        public FlexibleDateTimeType() : base("DateTime") { }

        public override IValueNode ParseResult(object? resultValue)
        {
            if (resultValue is DateTime dt)
                return new StringValueNode(dt.ToString("yyyy-MM-ddTHH:mm:ss"));
            if (resultValue is string s)
                return new StringValueNode(s);
            throw new SerializationException("Cannot parse DateTime result value.", this);
        }

        protected override DateTime ParseLiteral(StringValueNode valueSyntax)
        {
            if (DateTime.TryParse(valueSyntax.Value,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                out var dt))
                return dt;
            throw new SerializationException($"Cannot parse '{valueSyntax.Value}' as DateTime.", this);
        }

        protected override StringValueNode ParseValue(DateTime runtimeValue)
            => new StringValueNode(runtimeValue.ToString("yyyy-MM-ddTHH:mm:ss"));

        public override bool TrySerialize(object? runtimeValue, out object? resultValue)
        {
            if (runtimeValue is DateTime dt)
            {
                resultValue = dt.ToString("yyyy-MM-ddTHH:mm:ss");
                return true;
            }
            resultValue = null;
            return false;
        }

        public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
        {
            if (resultValue is string s && DateTime.TryParse(s,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                out var dt))
            {
                runtimeValue = dt;
                return true;
            }
            if (resultValue is DateTime d)
            {
                runtimeValue = d;
                return true;
            }
            runtimeValue = null;
            return false;
        }
    }
}
