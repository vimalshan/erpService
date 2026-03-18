using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Repositories;

namespace LeaveServices.Application.Features.LeaveRequests.Commands;

public sealed class CreateLeaveRequestHandler : IRequestHandler<CreateLeaveRequestCommand, LeaveRequestDto>
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLeaveRequestHandler(ILeaveRequestRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveRequestDto> Handle(CreateLeaveRequestCommand request, CancellationToken ct)
    {
        var leaveRequest = LeaveRequest.Create(
            request.ReqNum,
            request.FinyearSrlno,
            request.EmpUserId,
            request.SupUserId);

        await _repository.AddAsync(leaveRequest, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new LeaveRequestDto(
            leaveRequest.ReqNum,
            leaveRequest.FinyearSrlno,
            leaveRequest.EmpUserId,
            leaveRequest.SupUserId,
            leaveRequest.ReqDate,
            []);
    }
}

public sealed class AddLeaveRequestDetailHandler : IRequestHandler<AddLeaveRequestDetailCommand, LeaveRequestDetailDto>
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddLeaveRequestDetailHandler(ILeaveRequestRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveRequestDetailDto> Handle(AddLeaveRequestDetailCommand command, CancellationToken ct)
    {
        var request = await _repository.GetByIdAsync(command.ReqNum, ct)
            ?? throw new KeyNotFoundException($"Leave request {command.ReqNum} not found.");

        var detail = request.AddDetail(command.SrlNum, command.ModUser, command.PrefModDev, command.ActTaken);
        await _unitOfWork.SaveChangesAsync(ct);

        return new LeaveRequestDetailDto(
            detail.LsReqNum, detail.LsSrlNum, detail.LsModDat,
            detail.LsModUser, detail.LsPrefModdev, detail.LsActTaken,
            detail.LsCrsId, detail.LsRevType, detail.LsLetsubCode?.ToString());
    }
}
