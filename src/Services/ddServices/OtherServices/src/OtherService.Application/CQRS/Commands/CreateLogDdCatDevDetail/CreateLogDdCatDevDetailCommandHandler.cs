using AutoMapper;
using MediatR;
using OtherService.Application.DTOs;
using OtherService.Domain.Entities;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;

public sealed class CreateLogDdCatDevDetailCommandHandler
    : IRequestHandler<CreateLogDdCatDevDetailCommand, LogDdCatDevDetailDto>
{
    private readonly ILogDdCatDevDetailRepository _repository;
    private readonly IMapper _mapper;

    public CreateLogDdCatDevDetailCommandHandler(
        ILogDdCatDevDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<LogDdCatDevDetailDto> Handle(
        CreateLogDdCatDevDetailCommand request,
        CancellationToken cancellationToken)
    {
        var entity = LogDdCatDevDetail.Create(
            request.AppId,
            request.AppNum,
            request.ReqNum,
            request.QtnNum,
            request.AnsSrl,
            request.EntDat,
            request.Desc,
            request.Need);

        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LogDdCatDevDetailDto>(entity);
    }
}
