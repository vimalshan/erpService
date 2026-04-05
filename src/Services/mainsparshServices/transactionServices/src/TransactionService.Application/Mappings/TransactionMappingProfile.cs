using AutoMapper;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Mappings;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<ApprovalWorkflow, ApprovalWorkflowDto>()
            .ConstructUsing((src, ctx) => new ApprovalWorkflowDto(
                src.Id,
                src.WorkflowCode,
                src.EntityType,
                src.EntityId,
                src.EmployeeId,
                src.WorkflowStatus,
                src.CurrentApprovalLevel,
                src.CurrentApproverId,
                src.MaxApprovalLevels,
                src.Remarks,
                src.CreatedBy,
                src.CreatedOn,
                src.UpdatedBy,
                src.UpdatedOn,
                ctx.Mapper.Map<List<ApprovalStepDto>>(src.Steps)));

        CreateMap<ApprovalStep, ApprovalStepDto>()
            .ConstructUsing(src => new ApprovalStepDto(
                src.Id,
                src.WorkflowId,
                src.StepLevel,
                src.ApproverId,
                src.StepStatus,
                src.StepRemarks,
                src.ActedOn,
                src.CreatedBy,
                src.CreatedOn));

        CreateMap<TransactionLog, TransactionLogDto>()
            .ConstructUsing(src => new TransactionLogDto(
                src.Id,
                src.TransactionType,
                src.TransactionId,
                src.Action,
                src.ActionBy,
                src.ActionData,
                src.PreviousStatus,
                src.NewStatus,
                src.IpAddress,
                src.CreatedOn));
    }
}
