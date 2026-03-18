using OtherService.Domain.Exceptions;

namespace OtherService.Domain.ValueObjects;

/// <summary>
/// Value Object representing the applicant's user identity.
/// </summary>
public sealed class ApplicantId : IEquatable<ApplicantId>
{
    public string Value { get; }

    private ApplicantId(string value) => Value = value;

    public static ApplicantId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Applicant ID cannot be empty.");
        if (value.Length > 30)
            throw new DomainException("Applicant ID cannot exceed 30 characters.");
        return new ApplicantId(value.Trim());
    }

    public bool Equals(ApplicantId? other) =>
        other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ApplicantId ai && Equals(ai);
    public override int GetHashCode() => Value.ToUpperInvariant().GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(ApplicantId ai) => ai.Value;
}
