using OtherService.Application.DTOs;
using OtherService.Domain.Entities;

namespace OtherService.API.GraphQL.Types;

/// <summary>HotChocolate type for LogDdCatDevDetailDto.</summary>
public sealed class LogDdCatDevDetailType : ObjectType<LogDdCatDevDetailDto>
{
    protected override void Configure(IObjectTypeDescriptor<LogDdCatDevDetailDto> descriptor)
    {
        descriptor.Description("Represents a category development detail log entry.");

        descriptor.Field(f => f.AppId).Description("User ID (CT_APP_ID).");
        descriptor.Field(f => f.AppNum).Description("User Number (CT_APP_NUM).");
        descriptor.Field(f => f.ReqNum).Description("Request Number (CT_REQ_NUM).");
        descriptor.Field(f => f.QtnNum).Description("Question Number (CT_QTN_NUM).");
        descriptor.Field(f => f.AnsSrl).Description("Answer Serial Number (CT_ANS_SRL).");
        descriptor.Field(f => f.EntDat).Description("Entry Date (CT_ENT_DAT).");
        descriptor.Field(f => f.Desc).Description("Areas for Development (CT_DESC).");
        descriptor.Field(f => f.Need).Description("Why do you need it? (CT_NEED).");
    }
}
