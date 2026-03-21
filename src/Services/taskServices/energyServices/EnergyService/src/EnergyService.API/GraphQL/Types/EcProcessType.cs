using EnergyService.Application.DTOs;

namespace EnergyService.API.GraphQL.Types;

public class EcProcessType : ObjectType<EcProcessDto>
{
    protected override void Configure(IObjectTypeDescriptor<EcProcessDto> descriptor)
    {
        descriptor.Name("EcProcess");
        descriptor.Field(f => f.EcProcessId).Description("Process ID");
        descriptor.Field(f => f.EcProcessDesc).Description("Process description");
        descriptor.Field(f => f.EcUnitCode).Description("Unit code (e.g., KWH, KL)");
        descriptor.Field(f => f.EcCloseFlag).Description("Close flag (Y/N)");
        descriptor.Field(f => f.LastModifiedBy).Description("Last modified by user ID");
        descriptor.Field(f => f.LastModifiedOn).Description("Last modified timestamp");
    }
}
