using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetFilesByOrg;

public record GetFilesByOrgQuery(string OrgId, long? Year = null) : IRequest<IEnumerable<FileMasterDto>>;
