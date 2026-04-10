namespace SSCTransactional.Domain.Exceptions;

public class AllocationNotFoundException : Exception
{
    public AllocationNotFoundException(long id) : base($"AP Allocation with ID {id} was not found.") { }
}

public class CorrespondenceNotFoundException : Exception
{
    public CorrespondenceNotFoundException(long id) : base($"Correspondence with ID {id} was not found.") { }
}

public class TransactionDomainException : Exception
{
    public TransactionDomainException(string message) : base(message) { }
}

public class ApprovalNotFoundException : Exception
{
    public ApprovalNotFoundException(long id) : base($"Approval with ID {id} was not found.") { }
}

public class RescanNotFoundException : Exception
{
    public RescanNotFoundException(long id) : base($"Rescan with ID {id} was not found.") { }
}
