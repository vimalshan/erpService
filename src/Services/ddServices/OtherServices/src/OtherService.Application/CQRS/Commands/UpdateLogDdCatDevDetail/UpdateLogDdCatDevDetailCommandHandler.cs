using AutoMapper;
using MediatR;
using OtherService.Application.DTOs;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Commands.UpdateLogDdCatDevDetail;

public sealed class UpdateLogDdCatDevDetailCommandHandler
    : IRequestHandler<UpdateLogDdCatDevDetailCommand, LogDdCatDevDetailDto?>
{
    private readonly ILogDdCatDevDetailRepository _repository;
    private readonly IMapper _mapper;

    public UpdateLogDdCatDevDetailCommandHandler(
        ILogDdCatDevDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<LogDdCatDevDetailDto?> Handle(
        UpdateLogDdCatDevDetailCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByKeyAsync(request.AppId, request.AppNum, cancellationToken);
        if (entity is null) return null;

        entity.Update(
            request.ReqNum,
            request.QtnNum,
            request.AnsSrl,
            request.EntDat,
            request.Desc,
            request.Need);

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LogDdCatDevDetailDto>(entity);
    }
}
