using System;
using System.Collections.Generic;

namespace AppraisalService.Domain;

/// <summary>
/// Value object for employee identification
/// </summary>
public class EmployeeId : IEquatable<EmployeeId>
{
    public string UserId { get; }
    public long Pin { get; }

    public EmployeeId(string userId, long pin)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        if (pin <= 0)
            throw new ArgumentException("PIN must be greater than 0", nameof(pin));

        UserId = userId;
        Pin = pin;
    }

    public bool Equals(EmployeeId? other)
    {
        return other != null && UserId == other.UserId && Pin == other.Pin;
    }

    public override bool Equals(object? obj)
    {
        return obj is EmployeeId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, Pin);
    }

    public static bool operator ==(EmployeeId? left, EmployeeId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EmployeeId? left, EmployeeId? right)
    {
        return !Equals(left, right);
    }
}

/// <summary>
/// Value object for appraisal bands
/// </summary>
public class AppraisalBand : IEquatable<AppraisalBand>
{
    public long BandId { get; }
    public string? Description { get; }
    public string? Designation { get; }
    public string? Code { get; }
    public long? GradeId { get; }

    public AppraisalBand(long bandId, string? description, string? designation, string? code, long? gradeId)
    {
        if (bandId <= 0)
            throw new ArgumentException("Band ID must be greater than 0", nameof(bandId));

        BandId = bandId;
        Description = description;
        Designation = designation;
        Code = code;
        GradeId = gradeId;
    }

    public bool Equals(AppraisalBand? other)
    {
        return other != null && BandId == other.BandId;
    }

    public override bool Equals(object? obj)
    {
        return obj is AppraisalBand other && Equals(other);
    }

    public override int GetHashCode()
    {
        return BandId.GetHashCode();
    }
}

/// <summary>
/// Value object for compensation details
/// </summary>
public class CompensationDetails : IEquatable<CompensationDetails>
{
    public decimal? BasicOld { get; set; }
    public decimal? BasicNew { get; set; }
    public decimal? CtcOld { get; set; }
    public decimal? CtcNew { get; set; }
    public decimal? IncrementAmount { get; set; }
    public DateTime? EffectiveFrom { get; set; }

    // Parameterless constructor for EF Core
    public CompensationDetails()
    {
    }

    public CompensationDetails(
        decimal? basicOld, 
        decimal? basicNew,
        decimal? ctcOld,
        decimal? ctcNew,
        decimal? incrementAmount,
        DateTime? effectiveFrom)
    {
        BasicOld = basicOld;
        BasicNew = basicNew;
        CtcOld = ctcOld;
        CtcNew = ctcNew;
        IncrementAmount = incrementAmount;
        EffectiveFrom = effectiveFrom;
    }

    public bool Equals(CompensationDetails? other)
    {
        return other != null &&
               BasicOld == other.BasicOld &&
               BasicNew == other.BasicNew &&
               CtcOld == other.CtcOld &&
               CtcNew == other.CtcNew;
    }

    public override bool Equals(object? obj)
    {
        return obj is CompensationDetails other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BasicOld, BasicNew, CtcOld, CtcNew);
    }
}

/// <summary>
/// Value object for benefits availability
/// </summary>
public class BenefitsAvailability : IEquatable<BenefitsAvailability>
{
    public bool IsGratuityAvailable { get; set; }
    public bool IsSuperannuationAvailable { get; set; }
    public bool IsPfAvailable { get; set; }
    public decimal? NewFlexipay { get; set; }

    // Parameterless constructor for EF Core
    public BenefitsAvailability()
    {
    }

    public BenefitsAvailability(
        bool isGratuityAvailable,
        bool isSuperannuationAvailable,
        bool isPfAvailable,
        decimal? newFlexipay = null)
    {
        IsGratuityAvailable = isGratuityAvailable;
        IsSuperannuationAvailable = isSuperannuationAvailable;
        IsPfAvailable = isPfAvailable;
        NewFlexipay = newFlexipay ?? 0;
    }

    public bool Equals(BenefitsAvailability? other)
    {
        return other != null &&
               IsGratuityAvailable == other.IsGratuityAvailable &&
               IsSuperannuationAvailable == other.IsSuperannuationAvailable &&
               IsPfAvailable == other.IsPfAvailable;
    }

    public override bool Equals(object? obj)
    {
        return obj is BenefitsAvailability other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsGratuityAvailable, IsSuperannuationAvailable, IsPfAvailable);
    }
}

/// <summary>
/// Value object for appraisal status
/// </summary>
public class AppraisalStatus : IEquatable<AppraisalStatus>
{
    public static readonly AppraisalStatus Incomplete = new("I");
    public static readonly AppraisalStatus SubmittedByAppraisee = new("N");
    public static readonly AppraisalStatus PendingWithAppraiser = new("A");
    public static readonly AppraisalStatus SubmittedByAppraiser = new("S");
    public static readonly AppraisalStatus CompletedByAppraisee = new("C");

    public string Code { get; }

    private AppraisalStatus(string code)
    {
        Code = code;
    }

    public static AppraisalStatus FromCode(string code)
    {
        return code switch
        {
            "I" => Incomplete,
            "N" => SubmittedByAppraisee,
            "A" => PendingWithAppraiser,
            "S" => SubmittedByAppraiser,
            "C" => CompletedByAppraisee,
            _ => throw new ArgumentException($"Invalid appraisal status code: {code}", nameof(code))
        };
    }

    public bool Equals(AppraisalStatus? other)
    {
        return other != null && Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        return obj is AppraisalStatus other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}
