using System;

namespace LocationService.Domain.ValueObjects
{
    /// <summary>
    /// Value Object representing contact information
    /// </summary>
    public class Contact : IEquatable<Contact>
    {
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public string? ContactPerson { get; private set; }

        public Contact() { }

        public Contact(string? phone, string? email, string? contactPerson)
        {
            Phone = phone;
            Email = email;
            ContactPerson = contactPerson;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Contact);
        }

        public bool Equals(Contact? other)
        {
            if (other is null) return false;
            return Phone == other.Phone
                && Email == other.Email
                && ContactPerson == other.ContactPerson;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Phone, Email, ContactPerson);
        }

        public override string ToString()
        {
            return $"{ContactPerson} - {Phone} - {Email}";
        }
    }
}
