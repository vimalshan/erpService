using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.Institutes.Commands;

public record CreateInstituteCommand(
    decimal InstituteCode,
    string? InstituteName,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Pin,
    string? Phone,
    string? Fax,
    string? Email,
    string? Url,
    string? InstituteType,
    string CampusRecruit,
    string? InstituteClass,
    decimal? ModifiedBy
) : IRequest<InstituteMasterDto>;

public record DeleteInstituteCommand(decimal Code) : IRequest<bool>;
