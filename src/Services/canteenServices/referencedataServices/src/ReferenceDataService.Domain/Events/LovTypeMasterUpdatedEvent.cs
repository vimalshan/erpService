using MediatR;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Events;

public class LovTypeMasterUpdatedEvent : INotification
{
    public LovTypeMaster LovTypeMaster { get; }

    public LovTypeMasterUpdatedEvent(LovTypeMaster lovTypeMaster)
    {
        LovTypeMaster = lovTypeMaster;
    }
}
