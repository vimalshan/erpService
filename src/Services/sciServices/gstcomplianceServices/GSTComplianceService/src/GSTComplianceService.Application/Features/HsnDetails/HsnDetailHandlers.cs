using AutoMapper;
using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using MediatR;

namespace GSTComplianceService.Application.Features.HsnDetails.Commands;

public record AddHsnDetailCommand(long GstId, string? ProductName, string? HsnCode, string? Remarks) : IRequest<long>;

public class AddHsnDetailCommandHandler : IRequestHandler<AddHsnDetailCommand, long>
{
    private readonly IGstHsnDetailRepository _repository;
    private readonly IGstMainRepository _gstRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddHsnDetailCommandHandler(
        IGstHsnDetailRepository repository,
        IGstMainRepository gstRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _gstRepository = gstRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(AddHsnDetailCommand request, CancellationToken cancellationToken)
    {
        _ = await _gstRepository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(GstMain), request.GstId);

        var detail = GstHsnDetail.Create(request.GstId, request.ProductName, request.HsnCode, request.Remarks);
        await _repository.AddAsync(detail, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return detail.GstHsnId;
    }
}

public record GetHsnDetailsByGstIdQuery(long GstId) : IRequest<IEnumerable<GstHsnDetailDto>>;

public class GetHsnDetailsByGstIdQueryHandler : IRequestHandler<GetHsnDetailsByGstIdQuery, IEnumerable<GstHsnDetailDto>>
{
    private readonly IGstHsnDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetHsnDetailsByGstIdQueryHandler(IGstHsnDetailRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GstHsnDetailDto>> Handle(GetHsnDetailsByGstIdQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetByGstIdAsync(request.GstId, cancellationToken);
        return _mapper.Map<IEnumerable<GstHsnDetailDto>>(details);
    }
}
