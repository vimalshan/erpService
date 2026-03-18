using System;

namespace LocationService.Domain.ValueObjects
{
    /// <summary>
    /// Value Object representing entity status (Active/Inactive)
    /// </summary>
    public class Status : IEquatable<Status>
    {
        private const string ActiveStatus = "A";
        private const string InactiveStatus = "I";

        public string Value { get; private set; }

        public static Status Active => new(ActiveStatus);
        public static Status Inactive => new(InactiveStatus);

        public Status(string value)
        {
            if (value != ActiveStatus && value != InactiveStatus)
                throw new ArgumentException($"Status must be either '{ActiveStatus}' (Active) or '{InactiveStatus}' (Inactive)", nameof(value));

            Value = value;
        }

        public bool IsActive => Value == ActiveStatus;
        public bool IsInactive => Value == InactiveStatus;

        public override bool Equals(object? obj)
        {
            return Equals(obj as Status);
        }

        public bool Equals(Status? other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value == ActiveStatus ? "Active" : "Inactive";
        }

        public static implicit operator string(Status status) => status.Value;
        public static explicit operator Status(string value) => new(value);
    }
}
