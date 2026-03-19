using HRDocumentService.Application.DTOs;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record UploadDocumentFileCommand(
    long DocId,
    string FileName,
    string ContentType,
    Stream FileStream
) : IRequest<HRDocumentFileDto>;
