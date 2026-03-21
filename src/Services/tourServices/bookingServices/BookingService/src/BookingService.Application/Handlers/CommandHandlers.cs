using AutoMapper;
using MediatR;
using BookingService.Application.Commands;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Events;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Handlers;

public class CreateBookingHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBookingCommand, BookRequestMainDto>
{
    public async Task<BookRequestMainDto> Handle(CreateBookingCommand request, CancellationToken ct)
    {
        var entity = new BookRequestMain
        {
            BookMainId = Guid.NewGuid().ToString(),
            TpStatus = request.TpStatus,
            TpId = request.TpId,
            EmployeeSysId = request.EmployeeSysId,
            Through = request.Through,
            AdminId = request.AdminId,
            Remarks = request.Remarks,
            Type = request.Type,
            ApprovalStatus = "PENDING",
            ConfirmationStatus = "Pending",
            ProofType = request.ProofType,
            FoodPreference = request.FoodPreference,
            BudgetedCost = request.BudgetedCost,
            EmployeeCalendarId = request.EmployeeCalendarId,
            LastModifiedOn = DateTime.UtcNow,
            EnteredOn = DateTime.UtcNow
        };

        if (request.Tickets?.Count > 0)
        {
            foreach (var t in request.Tickets)
            {
                entity.Tickets.Add(new BookRequestTicket
                {
                    BookTicketId = Guid.NewGuid().ToString(),
                    MainId = entity.BookMainId,
                    ModeId = t.ModeId,
                    ClassId = t.ClassId,
                    Type = t.Type,
                    StartDate = t.StartDate,
                    StartTime = t.StartTime ?? string.Empty,
                    StartCityId = t.StartCityId,
                    StartCity = t.StartCity,
                    EndCityId = t.EndCityId,
                    EndCity = t.EndCity,
                    ConfirmationNo = string.Empty,
                    ApprovalStatus = "PENDING",
                    LastModifiedBy = request.EmployeeSysId,
                    LastModifiedOn = DateTime.UtcNow,
                    BudgetCost = t.BudgetCost ?? "0",
                    AdminRemarks = string.Empty,
                    SpecialSanction = t.SpecialSanction ?? "N",
                    SpecialSanctionReason = t.SpecialSanctionReason ?? string.Empty
                });
            }
        }

        if (request.Stays?.Count > 0)
        {
            foreach (var s in request.Stays)
            {
                entity.Stays.Add(new BookRequestStay
                {
                    BookStayId = Guid.NewGuid().ToString(),
                    MainId = entity.BookMainId,
                    CityId = s.CityId,
                    City = s.City,
                    CheckInDate = s.CheckInDate,
                    CheckOutDate = s.CheckOutDate,
                    ConfirmationNo = string.Empty,
                    LastModifiedBy = request.EmployeeSysId,
                    LastModifiedOn = DateTime.UtcNow
                });
            }
        }

        if (request.Cabs?.Count > 0)
        {
            foreach (var c in request.Cabs)
            {
                entity.Cabs.Add(new BookRequestCab
                {
                    BookCabId = Guid.NewGuid().ToString(),
                    MainId = entity.BookMainId,
                    PickupLocation = c.PickupLocation,
                    DropLocation = c.DropLocation,
                    PickupDate = c.PickupDate,
                    CarType = c.CarType,
                    Preference = c.Preference,
                    TripType = c.TripType,
                    Address = c.Address,
                    ConfirmationNo = string.Empty,
                    LastModifiedBy = request.EmployeeSysId,
                    LastModifiedOn = DateTime.UtcNow,
                    Nature = c.Nature
                });
            }
        }

        if (request.CostCentres?.Count > 0)
        {
            foreach (var cc in request.CostCentres)
            {
                entity.CostCentres.Add(new BookRequestCostCentre
                {
                    BookCcId = Guid.NewGuid().ToString(),
                    MainId = entity.BookMainId,
                    BusinessUnitCode = cc.BusinessUnitCode,
                    CostCentreCode = cc.CostCentreCode,
                    SubAccountCode = cc.SubAccountCode,
                    ProductCode = cc.ProductCode,
                    LocationSegment = cc.LocationSegment,
                    AllocationPercentage = cc.AllocationPercentage
                });
            }
        }

        entity.AddDomainEvent(new BookingCreatedEvent(entity.BookMainId, entity.EmployeeSysId, entity.Type));

        await unitOfWork.BookRequests.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<BookRequestMainDto>(entity);
    }
}

public class UpdateBookingHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBookingCommand, BookRequestMainDto>
{
    public async Task<BookRequestMainDto> Handle(UpdateBookingCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.BookRequests.GetByIdAsync(request.BookMainId, ct)
            ?? throw new KeyNotFoundException($"Booking {request.BookMainId} not found");

        entity.Remarks = request.Remarks;
        entity.FoodPreference = request.FoodPreference;
        entity.BudgetedCost = request.BudgetedCost;
        entity.LastModifiedOn = DateTime.UtcNow;

        unitOfWork.BookRequests.Update(entity);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<BookRequestMainDto>(entity);
    }
}

public class DeleteBookingHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookingCommand, bool>
{
    public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.BookRequests.GetByIdAsync(request.BookMainId, ct)
            ?? throw new KeyNotFoundException($"Booking {request.BookMainId} not found");

        unitOfWork.BookRequests.Delete(entity);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

public class ApproveBookingHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveBookingCommand, bool>
{
    public async Task<bool> Handle(ApproveBookingCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.BookRequests.GetByIdAsync(request.BookMainId, ct)
            ?? throw new KeyNotFoundException($"Booking {request.BookMainId} not found");

        entity.ApprovalStatus = "APPROVED";
        entity.LastModifiedOn = DateTime.UtcNow;
        entity.AddDomainEvent(new BookingApprovedEvent(entity.BookMainId, request.ApprovedBy));

        unitOfWork.BookRequests.Update(entity);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

public class ConfirmBookingHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<ConfirmBookingCommand, BookConfirmationDto>
{
    public async Task<BookConfirmationDto> Handle(ConfirmBookingCommand request, CancellationToken ct)
    {
        var booking = await unitOfWork.BookRequests.GetByIdAsync(request.BookId, ct)
            ?? throw new KeyNotFoundException($"Booking {request.BookId} not found");

        var confirmation = new BookRequestConfirmation
        {
            BookConfId = Guid.NewGuid().ToString(),
            Mode = request.Mode,
            BookId = request.BookId,
            RefId = request.RefId,
            ConfirmationDate = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Cost = request.Cost,
            ClassId = request.ClassId,
            VendorId = request.VendorId,
            GuestHouseSiteId = string.Empty,
            CabConfirmationId = string.Empty,
            RefundCost = "0",
            CancelDate = DateTime.MinValue,
            DebitMemoBatch = string.Empty,
            CreditMemoBatch = string.Empty,
            AdminRemarks = request.AdminRemarks,
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            ConfirmedBy = "A",
            ApprovalStatus = "APPROVED",
            OldRequestId = string.Empty
        };

        booking.ConfirmationStatus = "Confirmed";
        booking.LastModifiedOn = DateTime.UtcNow;
        booking.AddDomainEvent(new BookingConfirmedEvent(booking.BookMainId, confirmation.BookConfId));

        await unitOfWork.BookConfirmations.AddAsync(confirmation, ct);
        unitOfWork.BookRequests.Update(booking);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<BookConfirmationDto>(confirmation);
    }
}

public class CancelBookingHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelBookingCommand, bool>
{
    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.BookRequests.GetByIdAsync(request.BookMainId, ct)
            ?? throw new KeyNotFoundException($"Booking {request.BookMainId} not found");

        entity.ConfirmationStatus = "Cancelled";
        entity.Remarks = request.Reason;
        entity.LastModifiedOn = DateTime.UtcNow;
        entity.AddDomainEvent(new BookingCancelledEvent(entity.BookMainId, request.Reason));

        unitOfWork.BookRequests.Update(entity);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
