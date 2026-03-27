using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Domain.Entities;

namespace OrganizationSetup.Application.PpLimits.Commands;

public sealed class CreatePpLimitCommandHandler : IRequestHandler<CreatePpLimitCommand, PpLimitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreatePpLimitCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<PpLimitDto> Handle(CreatePpLimitCommand request, CancellationToken cancellationToken)
    {
        var limit = DealPpLimit.Create(request.LimitId, request.OrgId, request.TranType, request.BaseCurr, request.LimitAmt, request.FinYear, null, _currentUserService.UserId);
        await _unitOfWork.PpLimits.AddAsync(limit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<PpLimitDto>(limit);
    }
}

public sealed class UpdatePpLimitCommandHandler : IRequestHandler<UpdatePpLimitCommand, PpLimitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdatePpLimitCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<PpLimitDto> Handle(UpdatePpLimitCommand request, CancellationToken cancellationToken)
    {
        var limit = await _unitOfWork.PpLimits.GetByIdAsync(request.LimitId, cancellationToken)
            ?? throw new KeyNotFoundException($"PpLimit with ID {request.LimitId} not found.");
        if (request.NewLimitAct.HasValue)
            limit.UpdateActual(request.NewLimitAct.Value, _currentUserService.UserId ?? 0);
        await _unitOfWork.PpLimits.UpdateAsync(limit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<PpLimitDto>(limit);
    }
}

public sealed class UploadPpCertificateCommandHandler : IRequestHandler<UploadPpCertificateCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorage;
    private readonly ICurrentUserService _currentUserService;

    public UploadPpCertificateCommandHandler(IUnitOfWork unitOfWork, IBlobStorageService blobStorage, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _blobStorage = blobStorage;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(UploadPpCertificateCommand request, CancellationToken cancellationToken)
    {
        var limit = await _unitOfWork.PpLimits.GetByIdAsync(request.LimitId, cancellationToken)
            ?? throw new KeyNotFoundException($"PpLimit with ID {request.LimitId} not found.");
        var blobUrl = await _blobStorage.UploadAsync("pp-certificates", request.FileName, request.CertificateStream, cancellationToken);
        limit.UpdateCertificate(blobUrl, _currentUserService.UserId ?? 0);
        await _unitOfWork.PpLimits.UpdateAsync(limit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return blobUrl;
    }
}
