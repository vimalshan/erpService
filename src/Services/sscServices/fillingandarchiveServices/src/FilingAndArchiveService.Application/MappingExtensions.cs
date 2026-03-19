using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Entities;

namespace FilingAndArchiveService.Application;

internal static class MappingExtensions
{
    internal static FileMasterDto ToDto(this FileMaster f) => new()
    {
        FileId = f.FileId,
        FileOrgId = f.FileOrgId,
        FileYear = f.FileYear,
        FileNo = f.FileNo,
        FileStatus = f.FileStatus,
        FileRemarks = f.FileRemarks,
        FilePodNo = f.FilePodNo,
        FileCourierName = f.FileCourierName,
        FileCreatedOn = f.FileCreatedOn,
        FileCreatedBy = f.FileCreatedBy,
        FileUpdatedOn = f.FileUpdatedOn,
        FileUpdatedBy = f.FileUpdatedBy,
        FileDispatchedOn = f.FileDispatchedOn,
        FileDispatchedBy = f.FileDispatchedBy
    };
}
