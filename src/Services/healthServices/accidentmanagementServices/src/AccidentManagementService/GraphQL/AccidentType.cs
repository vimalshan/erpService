using AccidentManagementService.Domain.Entities;
using HotChocolate.Types;

namespace AccidentManagementService.GraphQL
{
    public class AccidentType : ObjectType<DailyAccidentFIR>
    {
        protected override void Configure(IObjectTypeDescriptor<DailyAccidentFIR> descriptor)
        {
            descriptor
                .Description("Represents a daily accident First Information Report (FIR)");

            descriptor
                .Field(f => f.AccidentNumber)
                .Description("Unique accident reference number");

            descriptor
                .Field(f => f.CompanyCode)
                .Description("Company code where accident occurred");

            descriptor
                .Field(f => f.AccidentDateTime)
                .Description("Date and time when the accident occurred");

            descriptor
                .Field(f => f.AccidentLocation)
                .Description("Location where the accident occurred");

            descriptor
                .Field(f => f.CauseOfIncident)
                .Description("Root cause of the accident");

            descriptor
                .Field(f => f.Status)
                .Description("Current status of the accident (Reported, InvestigationInProgress, Closed, Pending)");

            descriptor
                .Field(f => f.EnteredUserID)
                .Description("User ID who entered the accident record");

            descriptor
                .Field(f => f.EnteredDate)
                .Description("Date when the accident record was entered");
        }
    }
}
