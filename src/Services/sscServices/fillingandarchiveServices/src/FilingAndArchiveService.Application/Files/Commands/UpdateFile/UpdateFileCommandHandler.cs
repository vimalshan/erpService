using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Exceptions;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.UpdateFile;

public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, FileMasterDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public UpdateFileCommandHandler(
        IFileRepository fileRepository,
        IApplicationDbContext context,
        IPublisher publisher)
    {
        _fileRepository = fileRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<FileMasterDto> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(request.FileId, cancellationToken)
            ?? throw new Domain.Exceptions.FileNotFoundException(request.FileId);

        file.UpdateDetails(request.Remarks, request.PodNo, request.CourierName, request.UpdatedBy);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in file.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);
        file.ClearDomainEvents();

        return file.ToDto();
    }
}
