using LovService.Application.DTOs;

namespace LovService.API.GraphQL.Subscriptions;

public class LovSubscription
{
    [Subscribe]
    [Topic("LovTypeCreated")]
    public LovTypeDto OnLovTypeCreated([EventMessage] LovTypeDto lovType) => lovType;

    [Subscribe]
    [Topic("LovMasterCreated")]
    public LovMasterDto OnLovMasterCreated([EventMessage] LovMasterDto lovMaster) => lovMaster;

    [Subscribe]
    [Topic("LovMasterUpdated")]
    public LovMasterDto OnLovMasterUpdated([EventMessage] LovMasterDto lovMaster) => lovMaster;
}
