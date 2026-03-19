namespace ClubMembershipService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class ClubNotFoundException : DomainException
{
    public ClubNotFoundException(long clubId) : base($"Club with ID {clubId} was not found.") { }
}

public class MembershipNotFoundException : DomainException
{
    public MembershipNotFoundException(long membershipId) : base($"Membership with ID {membershipId} was not found.") { }
}

public class ActivityNotFoundException : DomainException
{
    public ActivityNotFoundException(long activityId) : base($"Activity with ID {activityId} was not found.") { }
}

public class DuplicateMembershipException : DomainException
{
    public DuplicateMembershipException(long clubId, long memberId)
        : base($"Member {memberId} already has an active membership in club {clubId}.") { }
}
