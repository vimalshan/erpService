using MediatR;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Events;

public class LovMasterUpdatedEvent : INotification
{
    public LovMaster LovMaster { get; }

    public LovMasterUpdatedEvent(LovMaster lovMaster)
    {
        LovMaster = lovMaster;
    }
}
