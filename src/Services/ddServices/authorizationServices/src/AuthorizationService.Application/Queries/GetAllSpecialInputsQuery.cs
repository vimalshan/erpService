using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetAllSpecialInputsQuery : IRequest<IEnumerable<DTOs.SpecialInputDto>>
{
    public class Handler : IRequestHandler<GetAllSpecialInputsQuery, IEnumerable<DTOs.SpecialInputDto>>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.SpecialInputDto>> Handle(GetAllSpecialInputsQuery request, CancellationToken cancellationToken)
        {
            var specialInputs = await _unitOfWork.SpecialInputs.GetAllAsync(cancellationToken);
            return specialInputs.Select(si => new DTOs.SpecialInputDto
            {
                Id = si.Id,
                SpecialInputId = si.SpecialInputId,
                YearId = si.YearId,
                RoleType = si.RoleType,
                EmployeeSysId = si.EmployeeSysId,
                AppraisalSysId = si.AppraisalSysId,
                Inputs = si.Inputs,
                Status = si.Status,
                CreatedOn = si.CreatedOn,
                SubmittedOn = si.SubmittedOn,
                IsSubmitted = si.IsSubmitted
            }).ToList();
        }
    }
}
