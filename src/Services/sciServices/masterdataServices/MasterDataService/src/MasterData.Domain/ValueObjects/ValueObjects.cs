using System;

#nullable enable

namespace MasterData.Domain.ValueObjects
{
    /// <summary>
    /// Value Object for Email
    /// </summary>
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Invalid email format", nameof(value));

            Value = value;
        }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string value) => new(value);
    }

    /// <summary>
    /// Value Object for Code
    /// </summary>
    public record Code
    {
        public string Value { get; }

        public Code(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 25)
                throw new ArgumentException("Code must not be empty and must not exceed 25 characters", nameof(value));

            Value = value.Trim();
        }

        public static implicit operator string(Code code) => code.Value;
        public static implicit operator Code(string value) => new(value);
    }

    /// <summary>
    /// Value Object for Name
    /// </summary>
    public record Name
    {
        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                throw new ArgumentException("Name must not be empty and must not exceed 100 characters", nameof(value));

            Value = value.Trim();
        }

        public static implicit operator string(Name name) => name.Value;
        public static implicit operator Name(string value) => new(value);
    }

    /// <summary>
    /// Value Object for ContactInfo
    /// </summary>
    public record ContactInfo
    {
        public string PhoneNumber { get; }
        public Email? EmailAddress { get; }
        public string? Address { get; }

        public ContactInfo(string phoneNumber, Email? emailAddress = null, string? address = null)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Address = address;
        }
    }

    /// <summary>
    /// Value Object for Audit Information
    /// </summary>
    public record AuditInfo
    {
        public string CreatedBy { get; }
        public DateTime CreatedAt { get; }
        public string? ModifiedBy { get; }
        public DateTime? ModifiedAt { get; }

        public AuditInfo(string createdBy, DateTime createdAt, string? modifiedBy = null, DateTime? modifiedAt = null)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be empty", nameof(createdBy));

            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }
    }
}
