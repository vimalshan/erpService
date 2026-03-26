using MediatR;

namespace ReferenceDataService.Domain.Events;

public class LovMasterDeletedEvent : INotification
{
    public string LovId { get; }

    public LovMasterDeletedEvent(string lovId)
    {
        LovId = lovId;
    }
}
