using AutoMapper;
using InvestmentService.Application.DTOs;
using InvestmentService.Domain.Entities;
using InvestmentService.Domain.Interfaces;
using MediatR;

namespace InvestmentService.Application.Commands.Handlers;

public class CreateInvestmentHandler : IRequestHandler<CreateInvestmentCommand, InvestmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;

    public CreateInvestmentHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    {
        _uow = uow;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<InvestmentDto> Handle(CreateInvestmentCommand cmd, CancellationToken ct)
    {
        var investment = new Investment { InvNo = cmd.InvNo };
        investment.RecordPurchase(cmd.CategoryId, cmd.Units, cmd.PurchaseRate,
            cmd.PurchaseDate, cmd.MaturityDate, cmd.InterestRate, cmd.EnteredBy);

        investment.SubCategoryId = cmd.SubCategoryId;
        investment.Tenure = cmd.Tenure;
        investment.InterestOption = cmd.InterestOption;
        investment.InterestFrequency = cmd.InterestFrequency;
        investment.PaymentMode = cmd.PaymentMode;
        investment.BrokerId = cmd.BrokerId;
        investment.CertificateNumber = cmd.CertificateNumber;

        await _uow.Investments.AddAsync(investment, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("investment", "investment.created",
            new { investment.InvNo, investment.PurchaseValue, investment.PurchaseDate }, ct);

        return _mapper.Map<InvestmentDto>(investment);
    }
}

public class RedeemInvestmentHandler : IRequestHandler<RedeemInvestmentCommand, SaleDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;

    public RedeemInvestmentHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    {
        _uow = uow;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<SaleDetailDto> Handle(RedeemInvestmentCommand cmd, CancellationToken ct)
    {
        var investment = await _uow.Investments.GetByIdAsync(cmd.InvNo, ct)
            ?? throw new KeyNotFoundException($"Investment {cmd.InvNo} not found");

        investment.Redeem(cmd.SaleNo, cmd.SaleType, cmd.SaleDate, cmd.SaleValue, cmd.EnteredBy);
        await _uow.Investments.UpdateAsync(investment, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("investment", "investment.redeemed",
            new { cmd.InvNo, cmd.SaleValue, cmd.SaleDate }, ct);

        var sale = investment.SaleDetails.First(s => s.SaleNo == cmd.SaleNo);
        return _mapper.Map<SaleDetailDto>(sale);
    }
}

public class UpdateInvestmentHandler : IRequestHandler<UpdateInvestmentCommand, InvestmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateInvestmentHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<InvestmentDto> Handle(UpdateInvestmentCommand cmd, CancellationToken ct)
    {
        var investment = await _uow.Investments.GetByIdAsync(cmd.InvNo, ct)
            ?? throw new KeyNotFoundException($"Investment {cmd.InvNo} not found");

        if (cmd.RevisedInterestRate.HasValue)
        {
            investment.RevisedInterestRate = cmd.RevisedInterestRate;
            investment.RevisedInterestFrom = cmd.RevisedInterestFrom;
        }
        if (cmd.YtmRate.HasValue) investment.YtmRate = cmd.YtmRate;
        if (cmd.Status != null) investment.Status = cmd.Status;
        investment.LastModBy = cmd.ModifiedBy;
        investment.LastModOn = DateTime.UtcNow;

        await _uow.Investments.UpdateAsync(investment, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<InvestmentDto>(investment);
    }
}

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<CategoryDto> Handle(CreateCategoryCommand cmd, CancellationToken ct)
    {
        var category = new InvestmentCategory
        {
            Code = cmd.Code, ShortCode = cmd.ShortCode, Name = cmd.Name,
            Denomination = cmd.Denomination, GroupId = cmd.GroupId
        };
        await _uow.Categories.AddAsync(category, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<CategoryDto>(category);
    }
}

public class CreateSubCategoryHandler : IRequestHandler<CreateSubCategoryCommand, SubCategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateSubCategoryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<SubCategoryDto> Handle(CreateSubCategoryCommand cmd, CancellationToken ct)
    {
        var sub = new InvestmentSubCategory
        {
            Id = cmd.Id, ShortName = cmd.ShortName, Name = cmd.Name, CategoryId = cmd.CategoryId
        };
        await _uow.SubCategories.AddAsync(sub, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<SubCategoryDto>(sub);
    }
}

public class CreateBrokerHandler : IRequestHandler<CreateBrokerCommand, BrokerDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateBrokerHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<BrokerDto> Handle(CreateBrokerCommand cmd, CancellationToken ct)
    {
        var broker = new Broker
        {
            BrokerId = cmd.BrokerId, BrokerName = cmd.BrokerName, BrokerStatus = cmd.BrokerStatus
        };
        await _uow.Brokers.AddAsync(broker, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<BrokerDto>(broker);
    }
}

public class RecordInterestReceiptHandler : IRequestHandler<RecordInterestReceiptCommand, ScheduleDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RecordInterestReceiptHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<ScheduleDetailDto> Handle(RecordInterestReceiptCommand cmd, CancellationToken ct)
    {
        var schedules = await _uow.ScheduleDetails.GetByInvestmentAsync(0, ct);
        var schedule = schedules.FirstOrDefault(s => s.SchId == cmd.SchId)
            ?? throw new KeyNotFoundException($"Schedule {cmd.SchId} not found");

        schedule.ReceivedAmount = cmd.ReceivedAmount;
        schedule.ReceivedDate = cmd.ReceivedDate;
        schedule.ReceivedTransactionId = cmd.ReceivedTransactionId;

        await _uow.ScheduleDetails.UpdateAsync(schedule, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<ScheduleDetailDto>(schedule);
    }
}

public class ApproveInvestmentHandler : IRequestHandler<ApproveInvestmentCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessagePublisher _publisher;

    public ApproveInvestmentHandler(IUnitOfWork uow, IMessagePublisher publisher) { _uow = uow; _publisher = publisher; }

    public async Task<bool> Handle(ApproveInvestmentCommand cmd, CancellationToken ct)
    {
        var investment = await _uow.Investments.GetByIdAsync(cmd.InvestmentId, ct)
            ?? throw new KeyNotFoundException($"Investment {cmd.InvestmentId} not found");

        var approval = new ApprovalDetail
        {
            ApprovalDetailId = cmd.ApprovalDetailId,
            InvestmentId = cmd.InvestmentId,
            RefId = cmd.RefId,
            ApprovalLevel = cmd.ApprovalLevel,
            Flag = cmd.Flag,
            ApproverSysId = cmd.ApproverSysId,
            ApprovedOn = DateTime.UtcNow,
            Remarks = cmd.Remarks
        };

        investment.ApprovalDetails.Add(approval);
        await _uow.Investments.UpdateAsync(investment, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("investment", "investment.approved",
            new { cmd.InvestmentId, cmd.ApproverSysId, cmd.Flag }, ct);

        return true;
    }
}

public class GenerateInterestScheduleHandler : IRequestHandler<GenerateInterestScheduleCommand, List<ScheduleDetailDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GenerateInterestScheduleHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<ScheduleDetailDto>> Handle(GenerateInterestScheduleCommand cmd, CancellationToken ct)
    {
        var investment = await _uow.Investments.GetByIdAsync(cmd.InvNo, ct)
            ?? throw new KeyNotFoundException($"Investment {cmd.InvNo} not found");

        var schedules = new List<ScheduleDetail>();
        var startDate = investment.PurchaseDate ?? DateTime.UtcNow;
        var endDate = investment.MaturityDate ?? startDate.AddYears(1);
        var interestRate = investment.IssuedInterestRate ?? 0;
        var faceValue = investment.PurchaseValue ?? 0;

        int monthsInterval = investment.InterestFrequency switch
        {
            "M" => 1, "Q" => 3, "H" => 6, "Y" => 12, _ => 6
        };

        long schId = 1;
        var currentDate = startDate;
        while (currentDate < endDate)
        {
            var nextDate = currentDate.AddMonths(monthsInterval);
            if (nextDate > endDate) nextDate = endDate;

            var days = (nextDate - currentDate).Days;
            var dueAmount = faceValue * interestRate * days / 36500;

            schedules.Add(new ScheduleDetail
            {
                SchId = schId++,
                InvNo = cmd.InvNo,
                SlId = schId,
                ScheduleType = "INT",
                InterestFrom = currentDate,
                InterestTo = nextDate,
                InterestOption = interestRate,
                DueAmount = Math.Round(dueAmount, 0),
                DueDate = nextDate,
                Year = cmd.Year
            });
            currentDate = nextDate;
        }

        await _uow.ScheduleDetails.AddRangeAsync(schedules, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<List<ScheduleDetailDto>>(schedules);
    }
}
