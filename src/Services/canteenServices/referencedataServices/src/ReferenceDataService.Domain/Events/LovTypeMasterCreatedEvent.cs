using MediatR;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Events;

public class LovTypeMasterCreatedEvent : INotification
{
    public LovTypeMaster LovTypeMaster { get; }

    public LovTypeMasterCreatedEvent(LovTypeMaster lovTypeMaster)
    {
        LovTypeMaster = lovTypeMaster;
    }
}
