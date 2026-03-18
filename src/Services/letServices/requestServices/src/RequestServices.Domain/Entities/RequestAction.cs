namespace RequestServices.Domain.Entities;

/// <summary>Represents REQUEST_ACTION — review / action records against a request.</summary>
public class RequestAction
{
    public long?  RequestId      { get; private set; }
    public long   SerialNumber   { get; private set; }
    public long?  ActionNumber   { get; private set; }
    public string? KeyExperience { get; private set; }
    public string? UsageExperience { get; private set; }
    public decimal? TimeExperience { get; private set; }
    public string? SupervisorExperience { get; private set; }
    public DateTime? CancellationDate { get; private set; }
    public DateTime? EntryDate    { get; private set; }
    public char?  EntryUser       { get; private set; }
    public char?  ReviewUser      { get; private set; }
    public DateTime ReviewDate    { get; private set; }
    public long?  ReviewNotes     { get; private set; }
    public long?  CourseId        { get; private set; }
    public char?  ActionFlag      { get; private set; }
    public string? CancellationRemark { get; private set; }

    private RequestAction() { }

    public static RequestAction Create(
        long? requestId, long serialNumber, DateTime reviewDate,
        string? keyExperience = null, char? actionFlag = null)
    {
        return new RequestAction
        {
            RequestId    = requestId,
            SerialNumber = serialNumber,
            ReviewDate   = reviewDate,
            KeyExperience = keyExperience,
            ActionFlag   = actionFlag
        };
    }
}
