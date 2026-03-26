using MediatR;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Events;

public class LovMasterCreatedEvent : INotification
{
    public LovMaster LovMaster { get; }

    public LovMasterCreatedEvent(LovMaster lovMaster)
    {
        LovMaster = lovMaster;
    }
}
