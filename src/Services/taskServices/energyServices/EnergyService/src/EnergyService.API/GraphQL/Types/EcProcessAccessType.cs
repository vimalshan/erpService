using EnergyService.Application.DTOs;

namespace EnergyService.API.GraphQL.Types;

public class EcProcessAccessType : ObjectType<EcProcessAccessDto>
{
    protected override void Configure(IObjectTypeDescriptor<EcProcessAccessDto> descriptor)
    {
        descriptor.Name("EcProcessAccess");
        descriptor.Field(f => f.PaId).Description("Access ID");
        descriptor.Field(f => f.PaProcessId).Description("Process ID");
        descriptor.Field(f => f.PaEmpSysId).Description("Employee system ID");
        descriptor.Field(f => f.PaStartDate).Description("Access start date");
        descriptor.Field(f => f.PaCloseDate).Description("Access close date");
    }
}
