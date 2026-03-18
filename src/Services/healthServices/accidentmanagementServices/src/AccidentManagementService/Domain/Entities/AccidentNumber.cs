using System;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Value Object representing a unique accident number
    /// </summary>
    public class AccidentNumber : IEquatable<AccidentNumber>
    {
        public long Value { get; private set; }

        private AccidentNumber() { }

        public AccidentNumber(long value)
        {
            if (value <= 0)
                throw new ArgumentException("Accident number must be greater than zero", nameof(value));

            Value = value;
        }

        /// <summary>
        /// Generate a new accident number based on timestamp and sequence
        /// </summary>
        public static AccidentNumber Generate()
        {
            // Format: YYYYMMDDHHHMMSS + sequence (18 digits total)
            long timestamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            long sequence = DateTime.UtcNow.Ticks % 10000; // 4-digit sequence
            long accidentNumber = timestamp * 10000 + sequence;
            
            return new AccidentNumber(accidentNumber);
        }

        public override bool Equals(object? obj)
        {
            return obj is AccidentNumber number && Equals(number);
        }

        public bool Equals(AccidentNumber? other)
        {
            return other?.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator long(AccidentNumber accidentNumber)
        {
            return accidentNumber.Value;
        }

        public static explicit operator AccidentNumber(long value)
        {
            return new AccidentNumber(value);
        }
    }
}
