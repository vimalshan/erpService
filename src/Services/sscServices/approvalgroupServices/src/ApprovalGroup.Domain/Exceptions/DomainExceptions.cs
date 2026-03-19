namespace ApprovalGroup.Domain.Exceptions;

public class ApprovalGroupNotFoundException : Exception
{
    public ApprovalGroupNotFoundException(long groupId)
        : base($"Approval group with ID '{groupId}' was not found.") { }
}

public class ApprovalGroupMapNotFoundException : Exception
{
    public ApprovalGroupMapNotFoundException(long mapId)
        : base($"Approval group map with ID '{mapId}' was not found.") { }
}

public class UserMapNotFoundException : Exception
{
    public UserMapNotFoundException(long mapId)
        : base($"User map with ID '{mapId}' was not found.") { }
}

public class PullMatrixNotFoundException : Exception
{
    public PullMatrixNotFoundException(long matId)
        : base($"Pull matrix detail with ID '{matId}' was not found.") { }
}

public class DuplicateApprovalGroupException : Exception
{
    public DuplicateApprovalGroupException(string groupName)
        : base($"An approval group with name '{groupName}' already exists.") { }
}
