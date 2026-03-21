using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations;

public class BookRequestMainConfiguration : IEntityTypeConfiguration<BookRequestMain>
{
    public void Configure(EntityTypeBuilder<BookRequestMain> builder)
    {
        builder.ToTable("BOOKREQUEST_MAIN");
        builder.HasKey(e => e.BookMainId);
        builder.Property(e => e.BookMainId).HasColumnName("BOOKMAIN_ID").HasMaxLength(255);
        builder.Property(e => e.TpStatus).HasColumnName("BOOKMAIN_TPSTATUS").HasMaxLength(255);
        builder.Property(e => e.TpId).HasColumnName("BOOKMAIN_TPID").HasMaxLength(255);
        builder.Property(e => e.EmployeeSysId).HasColumnName("BOOKMAIN_EMPSYSID").HasMaxLength(255);
        builder.Property(e => e.Through).HasColumnName("BOOKMAIN_THROUGH").HasMaxLength(255);
        builder.Property(e => e.AdminId).HasColumnName("BOOKMAIN_ADMINID").HasMaxLength(255);
        builder.Property(e => e.Remarks).HasColumnName("BOOKMAIN_REMARKS").HasMaxLength(255);
        builder.Property(e => e.Type).HasColumnName("BOOKMAIN_TYPE").HasMaxLength(255);
        builder.Property(e => e.ApprovalStatus).HasColumnName("BOOKMAIN_APPSTATUS").HasMaxLength(255);
        builder.Property(e => e.ConfirmationStatus).HasColumnName("BOOKMAIN_CNFSTATUS").HasMaxLength(255);
        builder.Property(e => e.ProofType).HasColumnName("BOOKMAIN_PROOF").HasMaxLength(255);
        builder.Property(e => e.FoodPreference).HasColumnName("BOOKMAIN_FOODPREF").HasMaxLength(255);
        builder.Property(e => e.LastModifiedOn).HasColumnName("BOOKMAIN_LASTMODIFIEDON").HasPrecision(3);
        builder.Property(e => e.BudgetedCost).HasColumnName("BOOKMAIN_BUDCOST").HasMaxLength(255);
        builder.Property(e => e.EnteredBy).HasColumnName("BOOKMAIN_ENTBY").HasMaxLength(255);
        builder.Property(e => e.EnteredOn).HasColumnName("BOOKMAIN_ENTON").HasPrecision(3);
        builder.Property(e => e.EmployeeCalendarId).HasColumnName("BOOKMAIN_EMPCALID").HasMaxLength(255);

        builder.HasMany(e => e.Tickets).WithOne(e => e.Main).HasForeignKey(e => e.MainId);
        builder.HasMany(e => e.Stays).WithOne(e => e.Main).HasForeignKey(e => e.MainId);
        builder.HasMany(e => e.Cabs).WithOne(e => e.Main).HasForeignKey(e => e.MainId);
        builder.HasMany(e => e.CostCentres).WithOne(e => e.Main).HasForeignKey(e => e.MainId);
        builder.HasMany(e => e.Others).WithOne(e => e.Main).HasForeignKey(e => e.BookId);
        builder.HasMany(e => e.Confirmations).WithOne(e => e.Main).HasForeignKey(e => e.BookId);
    }
}

public class BookRequestTicketConfiguration : IEntityTypeConfiguration<BookRequestTicket>
{
    public void Configure(EntityTypeBuilder<BookRequestTicket> builder)
    {
        builder.ToTable("BOOKREQUEST_TICKET");
        builder.HasKey(e => e.BookTicketId);
        builder.Property(e => e.BookTicketId).HasColumnName("BOOKTKT_ID").HasMaxLength(255);
        builder.Property(e => e.MainId).HasColumnName("BOOKTKT_MAINID").HasMaxLength(255);
        builder.Property(e => e.ModeId).HasColumnName("BOOKTKT_MODEID").HasMaxLength(255);
        builder.Property(e => e.ClassId).HasColumnName("BOOKTKT_CLASSID").HasMaxLength(255);
        builder.Property(e => e.Type).HasColumnName("BOOKTKT_TYPE").HasMaxLength(255);
        builder.Property(e => e.StartDate).HasColumnName("BOOKTKT_STARTDATE").HasPrecision(3);
        builder.Property(e => e.StartTime).HasColumnName("BOOKTKT_STARTTIME").HasMaxLength(255);
        builder.Property(e => e.StartCityId).HasColumnName("BOOKTKT_STARTCITYID").HasMaxLength(255);
        builder.Property(e => e.StartCity).HasColumnName("BOOKTKT_STARTCITY").HasMaxLength(255);
        builder.Property(e => e.EndCityId).HasColumnName("BOOKTKT_ENDCITYID").HasMaxLength(255);
        builder.Property(e => e.EndCity).HasColumnName("BOOKTKT_ENDCITY").HasMaxLength(255);
        builder.Property(e => e.ConfirmationNo).HasColumnName("BOOKTKT_CNFNO").HasMaxLength(255);
        builder.Property(e => e.ApprovalStatus).HasColumnName("BOOKTKT_APPSTATUS").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("BOOKTKT_LASTMODIFIEDBY").HasMaxLength(255);
        builder.Property(e => e.LastModifiedOn).HasColumnName("BOOKTKT_LASTMODIFIEDON").HasPrecision(3);
        builder.Property(e => e.BudgetCost).HasColumnName("BOOKTKT_BUDGETCOST").HasMaxLength(255);
        builder.Property(e => e.AdminRemarks).HasColumnName("BOOKTKT_ADMREMARKS").HasMaxLength(255);
        builder.Property(e => e.SpecialSanction).HasColumnName("BOOKTKT_SPECIALSANCTION").HasMaxLength(255);
        builder.Property(e => e.SpecialSanctionReason).HasColumnName("BOOKTKT_SPLREASON").HasMaxLength(255);
    }
}

public class BookRequestStayConfiguration : IEntityTypeConfiguration<BookRequestStay>
{
    public void Configure(EntityTypeBuilder<BookRequestStay> builder)
    {
        builder.ToTable("BOOKREQUEST_STAY");
        builder.HasKey(e => e.BookStayId);
        builder.Property(e => e.BookStayId).HasColumnName("BOOKSTY_ID").HasMaxLength(255);
        builder.Property(e => e.MainId).HasColumnName("BOOKSTY_MAINID").HasMaxLength(255);
        builder.Property(e => e.CityId).HasColumnName("BOOKSTY_CITYID").HasMaxLength(255);
        builder.Property(e => e.City).HasColumnName("BOOKSTY_CITY").HasMaxLength(255);
        builder.Property(e => e.CheckInDate).HasColumnName("BOOKSTY_CHECKINDATE").HasPrecision(3);
        builder.Property(e => e.CheckOutDate).HasColumnName("BOOKSTY_CHECKOUTDATE").HasPrecision(3);
        builder.Property(e => e.ConfirmationNo).HasColumnName("BOOKSTY_CNFNO").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("BOOKSTY_LASTMODIFIEDBY").HasMaxLength(255);
        builder.Property(e => e.LastModifiedOn).HasColumnName("BOOKSTY_LASTMODIFIEDON").HasPrecision(3);
    }
}

public class BookRequestCabConfiguration : IEntityTypeConfiguration<BookRequestCab>
{
    public void Configure(EntityTypeBuilder<BookRequestCab> builder)
    {
        builder.ToTable("BOOKREQUEST_CAB");
        builder.HasKey(e => e.BookCabId);
        builder.Property(e => e.BookCabId).HasColumnName("BOOKCAB_ID").HasMaxLength(255);
        builder.Property(e => e.MainId).HasColumnName("BOOKCAB_MAINID").HasMaxLength(255);
        builder.Property(e => e.PickupLocation).HasColumnName("BOOKCAB_PICKUPLOC").HasMaxLength(255);
        builder.Property(e => e.DropLocation).HasColumnName("BOOKCAB_DROPLOC").HasMaxLength(255);
        builder.Property(e => e.PickupDate).HasColumnName("BOOKCAB_PICKUPDATE").HasPrecision(3);
        builder.Property(e => e.CarType).HasColumnName("BOOKCAB_CARTYPE").HasMaxLength(255);
        builder.Property(e => e.Preference).HasColumnName("BOOKCAB_PREFERENCE").HasMaxLength(255);
        builder.Property(e => e.TripType).HasColumnName("BOOKCAB_TRIPTYPE").HasMaxLength(255);
        builder.Property(e => e.Address).HasColumnName("BOOKCAB_ADDRESS").HasMaxLength(255);
        builder.Property(e => e.ConfirmationNo).HasColumnName("BOOKCAB_CNFNO").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("BOOKCAB_LASTMODIFIEDBY").HasMaxLength(255);
        builder.Property(e => e.LastModifiedOn).HasColumnName("BOOKCAB_LASTMODIFIEDON").HasPrecision(3);
        builder.Property(e => e.Nature).HasColumnName("BOOKCAB_NATURE").HasMaxLength(255);
    }
}

public class BookRequestCostCentreConfiguration : IEntityTypeConfiguration<BookRequestCostCentre>
{
    public void Configure(EntityTypeBuilder<BookRequestCostCentre> builder)
    {
        builder.ToTable("BOOKREQUEST_CC");
        builder.HasKey(e => e.BookCcId);
        builder.Property(e => e.BookCcId).HasColumnName("BOOKCC_ID").HasMaxLength(255);
        builder.Property(e => e.MainId).HasColumnName("BOOKCC_MAINID").HasMaxLength(255);
        builder.Property(e => e.BusinessUnitCode).HasColumnName("BOOKCC_BUCODE").HasMaxLength(255);
        builder.Property(e => e.CostCentreCode).HasColumnName("BOOKCC_CCCODE").HasMaxLength(255);
        builder.Property(e => e.SubAccountCode).HasColumnName("BOOKCC_SUBACCCODE").HasMaxLength(255);
        builder.Property(e => e.ProductCode).HasColumnName("BOOKCC_PRODUCTCODE").HasMaxLength(255);
        builder.Property(e => e.LocationSegment).HasColumnName("BOOKCC_LOCSEGMENT").HasMaxLength(255);
        builder.Property(e => e.AllocationPercentage).HasColumnName("BOOKCC_ALLLPER").HasMaxLength(255);
    }
}

public class BookRequestOtherConfiguration : IEntityTypeConfiguration<BookRequestOther>
{
    public void Configure(EntityTypeBuilder<BookRequestOther> builder)
    {
        builder.ToTable("BOOKREQUEST_OTHERS");
        builder.HasKey(e => e.BookOtherId);
        builder.Property(e => e.BookOtherId).HasColumnName("BOOKOTH_ID").HasMaxLength(255);
        builder.Property(e => e.BookId).HasColumnName("BOOKOTH_BOOKID").HasMaxLength(255);
        builder.Property(e => e.BookingFor).HasColumnName("BOOKOTH_FOR").HasMaxLength(255);
        builder.Property(e => e.Gender).HasColumnName("BOOKOTH_GENDER").HasMaxLength(255);
        builder.Property(e => e.Age).HasColumnName("BOOKOTH_AGE").HasMaxLength(255);
        builder.Property(e => e.ContactNo).HasColumnName("BOOKOTH_CONTACTNO").HasMaxLength(255);
        builder.Property(e => e.ApprovedBy).HasColumnName("BOOKOTH_APPROVEDBY").HasMaxLength(255);
        builder.Property(e => e.ApprovedOn).HasColumnName("BOOKOTH_APPROVEDON").HasPrecision(3);
    }
}

public class BookRequestConfirmationConfiguration : IEntityTypeConfiguration<BookRequestConfirmation>
{
    public void Configure(EntityTypeBuilder<BookRequestConfirmation> builder)
    {
        builder.ToTable("BOOKREQUEST_CONFIRMATION");
        builder.HasKey(e => e.BookConfId);
        builder.Property(e => e.BookConfId).HasColumnName("BOOKCNF_ID").HasMaxLength(255);
        builder.Property(e => e.Mode).HasColumnName("BOOKCNF_MODE").HasMaxLength(255);
        builder.Property(e => e.BookId).HasColumnName("BOOKCNF_BOOKID").HasMaxLength(255);
        builder.Property(e => e.RefId).HasColumnName("BOOKCNF_REFID").HasMaxLength(255);
        builder.Property(e => e.ConfirmationDate).HasColumnName("BOOKCNF_DATE").HasPrecision(3);
        builder.Property(e => e.StartDate).HasColumnName("BOOKCNF_STARTDATE").HasPrecision(3);
        builder.Property(e => e.EndDate).HasColumnName("BOOKCNF_ENDDATE").HasPrecision(3);
        builder.Property(e => e.Cost).HasColumnName("BOOKCNF_COST").HasMaxLength(255);
        builder.Property(e => e.ClassId).HasColumnName("BOOKCNF_CLASSID").HasMaxLength(255);
        builder.Property(e => e.VendorId).HasColumnName("BOOKCNF_VENDORID").HasMaxLength(255);
        builder.Property(e => e.GuestHouseSiteId).HasColumnName("BOOKCNF_GHSITEID").HasMaxLength(255);
        builder.Property(e => e.CabConfirmationId).HasColumnName("BOOKCNF_CABCONFID").HasMaxLength(255);
        builder.Property(e => e.RefundCost).HasColumnName("BOOKCNF_REFUNDCOST").HasMaxLength(255);
        builder.Property(e => e.CancelDate).HasColumnName("BOOKCNF_CANCELDATE").HasPrecision(3);
        builder.Property(e => e.DebitMemoBatch).HasColumnName("BOOKCNF_DEBITMEMOBATCH").HasMaxLength(255);
        builder.Property(e => e.CreditMemoBatch).HasColumnName("BOOKCNF_CREDITMEMOBATCH").HasMaxLength(255);
        builder.Property(e => e.AdminRemarks).HasColumnName("BOOKCNF_ADMREMARKS").HasMaxLength(255);
        builder.Property(e => e.LastModifiedOn).HasColumnName("BOOKCNF_LASTMODIFIEDON").HasPrecision(3);
        builder.Property(e => e.LastModifiedBy).HasColumnName("BOOKCNF_LASTMODIFIEDBY").HasMaxLength(255);
        builder.Property(e => e.ConfirmedBy).HasColumnName("BOOKCNF_CONFIRMEDBY").HasMaxLength(255);
        builder.Property(e => e.VendorSelf).HasColumnName("BOOKCNF_VENDORSLF").HasMaxLength(255);
        builder.Property(e => e.Attachment).HasColumnName("BOOKCNF_ATTACHMENT").HasMaxLength(255);
        builder.Property(e => e.ApprovalStatus).HasColumnName("BOOKCNF_APPROVALSTS").HasMaxLength(255);
        builder.Property(e => e.EnteredById).HasColumnName("BOOKCNF_ENTID").HasMaxLength(255);
        builder.Property(e => e.OldRequestId).HasColumnName("BOOKCNF_OLDREQID").HasMaxLength(255);
        builder.Property(e => e.AirlineVendorId).HasColumnName("BOOKCNF_AIRLINEVNDID").HasMaxLength(255);
        builder.Property(e => e.AirlinePnrNumber).HasColumnName("BOOKCNF_AIRPNRNUM").HasMaxLength(255);
    }
}

public class BookConfirmationCabConfiguration : IEntityTypeConfiguration<BookConfirmationCab>
{
    public void Configure(EntityTypeBuilder<BookConfirmationCab> builder)
    {
        builder.ToTable("BOOKCONF_CAB");
        builder.HasKey(e => e.ConfId);
        builder.Property(e => e.ConfId).HasColumnName("CNFCAB_CONFID");
        builder.Property(e => e.BookId).HasColumnName("CNFCAB_BOOKID");
        builder.Property(e => e.CabId).HasColumnName("CNFCAB_ID");
    }
}

public class BookConfirmationTicketConfiguration : IEntityTypeConfiguration<BookConfirmationTicket>
{
    public void Configure(EntityTypeBuilder<BookConfirmationTicket> builder)
    {
        builder.ToTable("BOOKCONF_TICKET");
        builder.HasKey(e => e.ConfTicketId);
        builder.Property(e => e.ConfTicketId).HasColumnName("CNFTKT_ID").HasMaxLength(255);
        builder.Property(e => e.BookId).HasColumnName("CNFTKT_BOOKID").HasMaxLength(255);
        builder.Property(e => e.TicketId).HasColumnName("CNFTKT_TICKETID").HasMaxLength(255);
        builder.Property(e => e.EntryDate).HasColumnName("CNFTKT_ENTDATE").HasPrecision(3);
        builder.Property(e => e.DepartureDate).HasColumnName("CNFTKT_DEPDATE").HasPrecision(3);
        builder.Property(e => e.Cost).HasColumnName("CNFTKT_COST").HasMaxLength(255);
        builder.Property(e => e.ConfirmationMainId).HasColumnName("CNFTKT_CNFID").HasMaxLength(255);
    }
}

public class BookConfirmationStayConfiguration : IEntityTypeConfiguration<BookConfirmationStay>
{
    public void Configure(EntityTypeBuilder<BookConfirmationStay> builder)
    {
        builder.ToTable("BOOKCONF_STAY");
        builder.HasKey(e => e.StayId);
        builder.Property(e => e.StayId).HasColumnName("CNFSTY_ID").HasMaxLength(255);
        builder.Property(e => e.BookId).HasColumnName("CNFSTY_BOOKID").HasMaxLength(255);
        builder.Property(e => e.CheckInDate).HasColumnName("CNFSTY_CHECKINDATE").HasPrecision(3);
        builder.Property(e => e.CheckOutDate).HasColumnName("CNFSTY_CHECKOUTDATE").HasPrecision(3);
        builder.Property(e => e.GuestHouseSiteId).HasColumnName("CNFSTY_GHSITEID").HasMaxLength(255);
        builder.Property(e => e.ConfirmationMainId).HasColumnName("CNFSTY_CNFID").HasMaxLength(255);
    }
}

public class BookConfirmationCostCentreConfiguration : IEntityTypeConfiguration<BookConfirmationCostCentre>
{
    public void Configure(EntityTypeBuilder<BookConfirmationCostCentre> builder)
    {
        builder.ToTable("BOOKCONFIRMATION_CC");
        builder.HasKey(e => e.CcId);
        builder.Property(e => e.CcId).HasColumnName("BOOKCNFCC_ID");
        builder.Property(e => e.MainId).HasColumnName("BOOKCNFCC_MAINID");
        builder.Property(e => e.BusinessUnitCode).HasColumnName("BOOKCNF_BUCODE").HasMaxLength(25);
        builder.Property(e => e.CostCentreCode).HasColumnName("BOOKCNF_CCCODE").HasMaxLength(255);
        builder.Property(e => e.SubAccountCode).HasColumnName("BOOKCNF_SUBACCCODE").HasMaxLength(255);
        builder.Property(e => e.ProductCode).HasColumnName("BOOKCNF_PRODUCTCODE").HasMaxLength(255);
        builder.Property(e => e.LocationSegment).HasColumnName("BOOKCNF_LOCSEGMENT").HasMaxLength(255);
        builder.Property(e => e.AllocationPercentage).HasColumnName("BOOKCNF_ALLLPER");
    }
}

public class BookConfirmationMainConfiguration : IEntityTypeConfiguration<BookConfirmationMain>
{
    public void Configure(EntityTypeBuilder<BookConfirmationMain> builder)
    {
        builder.ToTable("BOOKCONFIRMATION_MAIN");
        builder.HasKey(e => e.ConfId);
        builder.Property(e => e.ConfId).HasColumnName("BOOKCNF_ID").HasMaxLength(255);
        builder.Property(e => e.Mode).HasColumnName("BOOKCNF_MODE").HasMaxLength(255);
        builder.Property(e => e.BookId).HasColumnName("BOOKCNF_BOOKID").HasMaxLength(255);
        builder.Property(e => e.RefId).HasColumnName("BOOKCNF_REFID").HasMaxLength(255);
        builder.Property(e => e.ConfirmationDate).HasColumnName("BOOKCNF_DATE").HasPrecision(3);
        builder.Property(e => e.StartDate).HasColumnName("BOOKCNF_STARTDATE").HasPrecision(3);
        builder.Property(e => e.EndDate).HasColumnName("BOOKCNF_ENDDATE").HasPrecision(3);
        builder.Property(e => e.BookType).HasColumnName("BOOKCNF_BOOKTYPE").HasMaxLength(255);
        builder.Property(e => e.Status).HasColumnName("BOOKCNF_STATUS").HasMaxLength(255);
        builder.Property(e => e.Remarks).HasColumnName("BOOKCNF_REMARKS").HasMaxLength(255);
        builder.Property(e => e.AdminUnit).HasColumnName("BOOKCNF_ADMUNIT").HasMaxLength(255);
        builder.Property(e => e.PaymentBatchNo).HasColumnName("BOOKCNF_PAYBATCHNO").HasMaxLength(255);
        builder.Property(e => e.ContractId).HasColumnName("BOOKCNF_CONTRACTID").HasMaxLength(255);
        builder.Property(e => e.VendorId).HasColumnName("BOOKCNF_VENDORID").HasMaxLength(255);
        builder.Property(e => e.TripCode).HasColumnName("BOOKCNF_TRIPCODE").HasMaxLength(255);
    }
}
