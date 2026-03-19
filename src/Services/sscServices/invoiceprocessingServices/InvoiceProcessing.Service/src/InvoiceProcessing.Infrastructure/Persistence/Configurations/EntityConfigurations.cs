using InvoiceProcessing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceProcessing.Infrastructure.Persistence.Configurations;

public class DocumentDetailConfiguration : IEntityTypeConfiguration<DocumentDetail>
{
    public void Configure(EntityTypeBuilder<DocumentDetail> builder)
    {
        builder.ToTable("DOC_DET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DOC_ID");
        builder.Property(e => e.OrgId).HasColumnName("DOC_ORGID").HasMaxLength(25);
        builder.Property(e => e.LocationId).HasColumnName("DOC_LOCID");
        builder.Property(e => e.DocumentNo).HasColumnName("DOC_NO").HasMaxLength(25);
        builder.Property(e => e.DocumentType).HasColumnName("DOC_TYPE").HasMaxLength(3);
        builder.Property(e => e.MainCategory).HasColumnName("DOC_MAINCAT");
        builder.Property(e => e.SubCategory).HasColumnName("DOC_SUBCAT");
        builder.Property(e => e.PoNumber).HasColumnName("DOC_PONO").HasMaxLength(25);
        builder.Property(e => e.VendorSiteId).HasColumnName("DOC_VNDSITEID");
        builder.Property(e => e.VendorId).HasColumnName("DOC_VNDID");
        builder.Property(e => e.DueDays).HasColumnName("DOC_DUEDAYS");
        builder.Property(e => e.PoId).HasColumnName("DOC_POID");
        builder.Property(e => e.MrcRemarks).HasColumnName("DOC_MRCREM").HasMaxLength(200);
        builder.Property(e => e.VatFlag).HasColumnName("DOC_VATFLAG").HasMaxLength(1);
        builder.Property(e => e.InvoiceNo).HasColumnName("DOC_INVOICENO").HasMaxLength(100);
        builder.Property(e => e.InvoiceAmount).HasColumnName("DOC_INVAMT");
        builder.Property(e => e.Currency).HasColumnName("DOC_CURRENCY");
        builder.Property(e => e.InvoiceDate).HasColumnName("DOC_INVDATE");
        builder.Property(e => e.InvoiceReceiptDate).HasColumnName("DOC_INVRECDATE");
        builder.Property(e => e.Pages).HasColumnName("DOC_PAGES");
        builder.Property(e => e.Remarks).HasColumnName("DOC_REMARKS").HasMaxLength(1000);
        builder.Property(e => e.PaymentDueDate).HasColumnName("DOC_DUEDATE");
        builder.Property(e => e.PayBy).HasColumnName("DOC_PAYBY");
        builder.Property(e => e.Signatory1).HasColumnName("DOC_SIGNATORY1");
        builder.Property(e => e.Signatory2).HasColumnName("DOC_SIGNATORY2");
        builder.Property(e => e.Approver).HasColumnName("DOC_APPROVER");
        builder.Property(e => e.Owner).HasColumnName("DOC_OWNER");
        builder.Property(e => e.DocumentStatus).HasColumnName("DOC_DOCSTATUS").HasMaxLength(2);
        builder.Property(e => e.InvoiceStatus).HasColumnName("DOC_INVSTATUS").HasMaxLength(2);
        builder.Property(e => e.UserId).HasColumnName("DOC_USERID");
        builder.Property(e => e.CreatedOn).HasColumnName("DOC_CREATEDON");
        builder.Property(e => e.SubmittedOn).HasColumnName("DOC_SUBMITTEDON");
        builder.Property(e => e.ReceivedBy).HasColumnName("DOC_RECEIVEDBY");
        builder.Property(e => e.ReceivedOn).HasColumnName("DOC_RECEIVEDON");
        builder.Property(e => e.CancelFlag).HasColumnName("DOC_CANCELFLAG").HasMaxLength(1);
        builder.Property(e => e.CancelUser).HasColumnName("DOC_CANCELUSER");
        builder.Property(e => e.CancelDate).HasColumnName("DOC_CANCELDATE");
        builder.Property(e => e.CurrentAllocationId).HasColumnName("DOC_APALLID");
        builder.Property(e => e.OracleVoucherNo).HasColumnName("DOC_ORAINVNO");
        builder.Property(e => e.PaymentTypeNo).HasColumnName("DOC_PAYTYPENO");
        builder.Property(e => e.AccountCode).HasColumnName("DOC_ACCOUNTCODE").HasMaxLength(25);
        builder.Property(e => e.SscInvoicePdf).HasColumnName("DOC_SSCINVOICEPDF").HasMaxLength(200);
        builder.Property(e => e.DocumentKey).HasColumnName("DOC_KEY").HasMaxLength(25);
        builder.Property(e => e.UserInvoicePdf).HasColumnName("DOC_USRINVOICEPDF").HasMaxLength(200);
        builder.Property(e => e.FilePath).HasColumnName("DOC_FILEPATH").HasMaxLength(200);
        builder.Property(e => e.InvoiceProcessedDate).HasColumnName("DOC_INVPROCDATE");
        builder.Property(e => e.InvoiceProcessedAllocationId).HasColumnName("DOC_INVPROCALLID");
        builder.Property(e => e.InvoiceValidationDate).HasColumnName("DOC_INVVALIDDATE");
        builder.Property(e => e.InvoiceValidationAllocationId).HasColumnName("DOC_INVVALIDALLID");
        builder.Property(e => e.HoldStatus).HasColumnName("DOC_HOLDSTATUS").HasMaxLength(1);
        builder.Property(e => e.Deduction).HasColumnName("DOC_DEDUCTION");
        builder.Property(e => e.ThirdPartyFlag).HasColumnName("DOC_THIRDPARTYFLAG").HasMaxLength(1);
        builder.Property(e => e.ThirdPartyVendor).HasColumnName("DOC_THIRDPARTYVENDOR").HasMaxLength(50);
        builder.Property(e => e.DeductionRemarks).HasColumnName("DOC_DEDUCTIONREMARKS").HasMaxLength(1000);
        builder.Property(e => e.FileId).HasColumnName("DOC_FILEID");
        builder.Property(e => e.CancelRemarks).HasColumnName("DOC_CANCELREMARKS").HasMaxLength(200);
        builder.Property(e => e.HoldPaymentFlag).HasColumnName("DOC_HOLDPAYMENTFLAG").HasMaxLength(1);
        builder.Property(e => e.HoldPaymentRemarks).HasColumnName("DOC_HOLDPAYMENTREMARKS").HasMaxLength(200);
        builder.Property(e => e.HoldReleaseRemarks).HasColumnName("DOC_HOLDRELEASEREMARKS").HasMaxLength(200);
        builder.Property(e => e.ScanFlag).HasColumnName("DOC_SCANFLAG").HasMaxLength(1);
        builder.Property(e => e.ApprovedBy).HasColumnName("DOC_APPROVEDBY");

        builder.HasMany(e => e.OracleInvoiceDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.OraclePaymentDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.OracleBankDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.PoList).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.ApprovalDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.MrcList).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.CostCenters).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.Attachments).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.ApAllocations).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.Correspondences).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.RescanDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.RevokeDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.OracleDueDetails).WithOne(e => e.Document).HasForeignKey(e => e.DocId);
        builder.HasMany(e => e.SscFiles).WithOne(e => e.Document).HasForeignKey(e => e.DocId);

        builder.HasIndex(e => e.OrgId);
        builder.HasIndex(e => e.DocumentStatus);
        builder.HasIndex(e => e.InvoiceNo);
        builder.HasIndex(e => e.VendorId);
    }
}

public class OracleInvoiceDetailConfiguration : IEntityTypeConfiguration<OracleInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<OracleInvoiceDetail> builder)
    {
        builder.ToTable("DOC_ORACLEINVDET");
        builder.HasKey(e => e.InvId);
        builder.Property(e => e.InvId).HasColumnName("DOC_INVID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.VoucherNo).HasColumnName("DOC_VOUCHERNO").HasPrecision(18, 2);
        builder.Property(e => e.InvoiceType).HasColumnName("DOC_INVOICETYPE").HasMaxLength(25);
        builder.Property(e => e.VendorId).HasColumnName("DOC_VENDORID");
        builder.Property(e => e.VendorSiteId).HasColumnName("DOC_VENDOR_SITEID");
        builder.Property(e => e.InvoiceNum).HasColumnName("DOC_INVOICENUM").HasMaxLength(50);
        builder.Property(e => e.InvoiceDate).HasColumnName("DOC_INVOICEDATE");
        builder.Property(e => e.InvoiceAmount).HasColumnName("DOC_INVOICEAMOUNT").HasPrecision(18, 2);
        builder.Property(e => e.InvoiceId).HasColumnName("DOC_INVOICEID");
        builder.Property(e => e.InvoiceStatus).HasColumnName("DOC_INVOICESTATUS").HasMaxLength(4000);
        builder.Property(e => e.InvoiceCreatedOn).HasColumnName("DOC_INVOICECREATEDON");
        builder.Property(e => e.InvoiceCreatedBy).HasColumnName("DOC_INVOICECREATEDBY").HasPrecision(18, 2);
        builder.Property(e => e.PaymentMethodCode).HasColumnName("DOC_PAYMENT_METHOD_CODE").HasMaxLength(30);
        builder.Property(e => e.AccountingDate).HasColumnName("DOC_ACCOUNTING_DATE");
    }
}

public class OraclePaymentDetailConfiguration : IEntityTypeConfiguration<OraclePaymentDetail>
{
    public void Configure(EntityTypeBuilder<OraclePaymentDetail> builder)
    {
        builder.ToTable("DOC_ORACLEPAYDET");
        builder.HasKey(e => e.PayId);
        builder.Property(e => e.PayId).HasColumnName("DOC_PAYID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.PaymentNum).HasColumnName("DOC_PAYMENTNUM");
        builder.Property(e => e.InvoiceId).HasColumnName("DOC_INVOICEID");
        builder.Property(e => e.DueDate).HasColumnName("DOC_DUEDATE");
        builder.Property(e => e.GrossAmount).HasColumnName("DOC_GROSSAMOUNT").HasPrecision(18, 2);
        builder.Property(e => e.AmountRemaining).HasColumnName("DOC_AMOUNTREMAINING").HasPrecision(18, 2);
        builder.Property(e => e.PaymentStatus).HasColumnName("DOC_PAYMENT_STATUS").HasMaxLength(14);
        builder.Property(e => e.PaymentMethod).HasColumnName("DOC_PAYMENT_METHOD").HasMaxLength(30);
        builder.Property(e => e.PrepaymentApplied).HasColumnName("DOC_PREPAYMENT_APPLIED").HasMaxLength(3);
        builder.Property(e => e.PaymentCreatedBy).HasColumnName("DOC_PAYMENT_CREATEDBY").HasPrecision(18, 2);
        builder.Property(e => e.PaymentCreatedOn).HasColumnName("DOC_PAYMENT_CREATEDON");
        builder.Property(e => e.CheckId).HasColumnName("CHECK_ID");
        builder.Property(e => e.BnkStatus).HasColumnName("BNKSTATUS").HasMaxLength(1);
        builder.Property(e => e.CheckNumber).HasColumnName("CHECK_NUMBER");
        builder.Property(e => e.CheckDate).HasColumnName("CHECK_DATE");
        builder.Property(e => e.CheckAmount).HasColumnName("CHECK_AMOUNT").HasPrecision(18, 2);
    }
}

public class OracleBankDetailConfiguration : IEntityTypeConfiguration<OracleBankDetail>
{
    public void Configure(EntityTypeBuilder<OracleBankDetail> builder)
    {
        builder.ToTable("DOC_ORACLEBNKDET");
        builder.HasKey(e => e.BnkId);
        builder.Property(e => e.BnkId).HasColumnName("DOC_BNKID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.Type).HasColumnName("TYPE").HasMaxLength(255);
        builder.Property(e => e.CheckId).HasColumnName("CHECK_ID").HasMaxLength(255);
        builder.Property(e => e.Business).HasColumnName("BUSINESS").HasMaxLength(255);
        builder.Property(e => e.OrgId).HasColumnName("ORG_ID").HasMaxLength(255);
        builder.Property(e => e.VendorSiteId).HasColumnName("VENDOR_SITE_ID").HasMaxLength(255);
        builder.Property(e => e.FileName).HasColumnName("FILE_NAME").HasMaxLength(255);
        builder.Property(e => e.RecordIdentifier).HasColumnName("RECORD_IDETIFIER").HasMaxLength(255);
        builder.Property(e => e.TransactionType).HasColumnName("TRANSACTION_TYPE").HasMaxLength(255);
        builder.Property(e => e.VendorCode).HasColumnName("VENDOR_CODE").HasMaxLength(255);
        builder.Property(e => e.MailTo).HasColumnName("MAIL_TO").HasMaxLength(255);
        builder.Property(e => e.BeneMailAddress).HasColumnName("BENE_MAIL_ADDRESS").HasMaxLength(255);
        builder.Property(e => e.BeneBankAc).HasColumnName("BENE_BANK_AC").HasMaxLength(255);
        builder.Property(e => e.PayTo).HasColumnName("PAY_TO").HasMaxLength(255);
        builder.Property(e => e.CheckDate).HasColumnName("CHECK_DATE").HasMaxLength(255);
        builder.Property(e => e.Amount).HasColumnName("AMOUNT").HasMaxLength(255);
        builder.Property(e => e.Hundi).HasColumnName("HUNDI").HasMaxLength(255);
        builder.Property(e => e.Currency).HasColumnName("CURRENCY").HasMaxLength(255);
        builder.Property(e => e.PaymentLocation).HasColumnName("PAYMENT_LOCATION").HasMaxLength(255);
        builder.Property(e => e.PaymentNumber).HasColumnName("PAYMENT_NUMBER").HasMaxLength(255);
        builder.Property(e => e.CheckNumber).HasColumnName("CHECK_NUMBER").HasMaxLength(255);
        builder.Property(e => e.PaymentDate).HasColumnName("PAYMENT_DATE").HasMaxLength(255);
        builder.Property(e => e.RecordAnnexure).HasColumnName("RECORS_ANNEXURE").HasMaxLength(255);
        builder.Property(e => e.PrintLocation).HasColumnName("PRINT_LOCATION").HasMaxLength(255);
        builder.Property(e => e.BeneIfsc).HasColumnName("BENE_IFSC").HasMaxLength(255);
        builder.Property(e => e.BeneAccountType).HasColumnName("BENE_ACCOUNT_TYPE").HasMaxLength(255);
        builder.Property(e => e.BeneBankName).HasColumnName("BENE_BANK_NAME").HasMaxLength(255);
        builder.Property(e => e.BeneBankAc22).HasColumnName("BENE_BANK_AC22").HasMaxLength(255);
        builder.Property(e => e.BeneBankBranch).HasColumnName("BENE_BANK_BRANCH").HasMaxLength(255);
        builder.Property(e => e.BeneBankLocation).HasColumnName("BENE_BANK_LOCATION").HasMaxLength(255);
        builder.Property(e => e.BeneMailId).HasColumnName("BENE_MAIL_ID").HasMaxLength(255);
        builder.Property(e => e.RefNo).HasColumnName("REF_NO").HasMaxLength(255);
        builder.Property(e => e.UtrNo).HasColumnName("UTR_NO").HasMaxLength(255);
        builder.Property(e => e.RejectReason1).HasColumnName("REJECT_REASON1").HasMaxLength(255);
        builder.Property(e => e.RejectReason2).HasColumnName("REJECT_REASON2").HasMaxLength(255);
        builder.Property(e => e.StatusLookupCode).HasColumnName("STATUS_LOOKUP_CODE").HasMaxLength(25);
    }
}

public class DocumentPoListConfiguration : IEntityTypeConfiguration<DocumentPoList>
{
    public void Configure(EntityTypeBuilder<DocumentPoList> builder)
    {
        builder.ToTable("DOC_POLIST");
        builder.HasKey(e => e.SeqId);
        builder.Property(e => e.SeqId).HasColumnName("PO_SEQID");
        builder.Property(e => e.DocId).HasColumnName("PO_DOCID");
        builder.Property(e => e.PoId).HasColumnName("PO_ID");
        builder.Property(e => e.PoNo).HasColumnName("PO_NO").HasMaxLength(15);
        builder.Property(e => e.PoLineNo).HasColumnName("PO_LINENO").HasMaxLength(15);
        builder.Property(e => e.PoLineId).HasColumnName("PO_LINE_ID");
        builder.Property(e => e.PoDate).HasColumnName("PO_DATE");
        builder.Property(e => e.PoTermId).HasColumnName("PO_TERM_ID");
        builder.Property(e => e.PoTermSeqNo).HasColumnName("PO_TERM_SEQNO");
    }
}

public class DocumentApprovalDetailConfiguration : IEntityTypeConfiguration<DocumentApprovalDetail>
{
    public void Configure(EntityTypeBuilder<DocumentApprovalDetail> builder)
    {
        builder.ToTable("DOC_APPDET");
        builder.HasKey(e => e.SeqId);
        builder.Property(e => e.SeqId).HasColumnName("APP_SEQID");
        builder.Property(e => e.DocId).HasColumnName("APP_DOCID");
        builder.Property(e => e.UserId).HasColumnName("APP_USERID");
        builder.Property(e => e.Status).HasColumnName("APP_STATUS").HasMaxLength(1);
        builder.Property(e => e.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(200);
        builder.Property(e => e.ApprovalDate).HasColumnName("APP_DATE");
    }
}

public class DocumentMrcListConfiguration : IEntityTypeConfiguration<DocumentMrcList>
{
    public void Configure(EntityTypeBuilder<DocumentMrcList> builder)
    {
        builder.ToTable("DOC_MRCLIST");
        builder.HasKey(e => e.SeqId);
        builder.Property(e => e.SeqId).HasColumnName("MRC_SEQID");
        builder.Property(e => e.DocId).HasColumnName("MRC_DOCID");
        builder.Property(e => e.LineId).HasColumnName("MRC_LINEID");
        builder.Property(e => e.MrcId).HasColumnName("MRC_ID");
        builder.Property(e => e.MrcNo).HasColumnName("MRC_NO").HasMaxLength(15);
        builder.Property(e => e.MrcDate).HasColumnName("MRC_DATE");
        builder.Property(e => e.PoLineId).HasColumnName("MRC_PO_LINEID");
    }
}

public class DocumentCostCenterConfiguration : IEntityTypeConfiguration<DocumentCostCenter>
{
    public void Configure(EntityTypeBuilder<DocumentCostCenter> builder)
    {
        builder.ToTable("DOC_CC");
        builder.HasKey(e => e.CcId);
        builder.Property(e => e.CcId).HasColumnName("CC_ID");
        builder.Property(e => e.DocId).HasColumnName("CC_DOCID");
        builder.Property(e => e.BusinessUnitId).HasColumnName("CC_BUID").HasMaxLength(25);
        builder.Property(e => e.LocationCode).HasColumnName("CC_LOCCODE").HasMaxLength(25);
        builder.Property(e => e.AccountCode).HasColumnName("CC_ACCOUNTCODE").HasMaxLength(25);
        builder.Property(e => e.SubAccount).HasColumnName("CC_SUBACC").HasMaxLength(25);
        builder.Property(e => e.CostCenterCode).HasColumnName("CC_CODE").HasMaxLength(25);
        builder.Property(e => e.Product).HasColumnName("CC_PRODUCT").HasMaxLength(25);
        builder.Property(e => e.Percentage).HasColumnName("CC_PER");
    }
}

public class DocumentAttachmentConfiguration : IEntityTypeConfiguration<DocumentAttachment>
{
    public void Configure(EntityTypeBuilder<DocumentAttachment> builder)
    {
        builder.ToTable("DOC_ATTACHMENT");
        builder.HasKey(e => e.AttachId);
        builder.Property(e => e.AttachId).HasColumnName("ATTACH_ID");
        builder.Property(e => e.DocId).HasColumnName("ATTACH_DOCID");
        builder.Property(e => e.FilePath).HasColumnName("ATTACH_FILEPATH").HasMaxLength(25);
    }
}

public class DocumentSscFileConfiguration : IEntityTypeConfiguration<DocumentSscFile>
{
    public void Configure(EntityTypeBuilder<DocumentSscFile> builder)
    {
        builder.ToTable("DOC_SSFILELIST");
        builder.HasKey(e => e.FileId);
        builder.Property(e => e.FileId).HasColumnName("FILE_ID");
        builder.Property(e => e.DocId).HasColumnName("FILE_DOCID");
        builder.Property(e => e.FilePath).HasColumnName("FILE_PATH").HasMaxLength(25);
    }
}

public class DocumentApAllocationConfiguration : IEntityTypeConfiguration<DocumentApAllocation>
{
    public void Configure(EntityTypeBuilder<DocumentApAllocation> builder)
    {
        builder.ToTable("DOC_APALLDET");
        builder.HasKey(e => e.AllocationId);
        builder.Property(e => e.AllocationId).HasColumnName("APALL_ID");
        builder.Property(e => e.DocId).HasColumnName("APALL_DOCID");
        builder.Property(e => e.Action).HasColumnName("APALL_ACTION").HasMaxLength(1);
        builder.Property(e => e.GroupId).HasColumnName("APALL_GROUPID");
        builder.Property(e => e.PullStatus).HasColumnName("APALL_PULLSTATUS").HasMaxLength(1);
        builder.Property(e => e.PullUserId).HasColumnName("APALL_PULLUSERID");
        builder.Property(e => e.Priority).HasColumnName("APALL_PRIORITY");
        builder.Property(e => e.AllocatedBy).HasColumnName("APALL_ALLBY");
        builder.Property(e => e.AllocatedOn).HasColumnName("APALL_ALLON");
        builder.Property(e => e.Remarks).HasColumnName("APALL_REMARKS").HasMaxLength(200);
        builder.Property(e => e.ActionFlag).HasColumnName("APALL_ACTIONFLAG").HasMaxLength(1);
        builder.Property(e => e.ActionDate).HasColumnName("APALL_ACTIONDATE");
        builder.Property(e => e.CorrespondenceId).HasColumnName("APALL_CORRID");
        builder.Property(e => e.DefectType).HasColumnName("APALL_DEFTYPE");
        builder.Property(e => e.CloseRemarks).HasColumnName("APALL_CLOSEREMARKS").HasMaxLength(200);
        builder.Property(e => e.ModifiedBy).HasColumnName("APALL_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("APALL_MODIFIEDON");
        builder.Property(e => e.PulledOn).HasColumnName("APALL_PULLEDON");
    }
}

public class DocumentCorrespondenceConfiguration : IEntityTypeConfiguration<DocumentCorrespondence>
{
    public void Configure(EntityTypeBuilder<DocumentCorrespondence> builder)
    {
        builder.ToTable("DOC_CORRESPOND");
        builder.HasKey(e => e.CorrId);
        builder.Property(e => e.CorrId).HasColumnName("CORR_ID");
        builder.Property(e => e.DocId).HasColumnName("CORR_DOCID");
        builder.Property(e => e.AllocationId).HasColumnName("CORR_ALLID");
        builder.Property(e => e.HoldCategory).HasColumnName("CORR_HOLDCAT");
        builder.Property(e => e.HoldType).HasColumnName("CORR_HOLDTYPE");
        builder.Property(e => e.HoldDate).HasColumnName("CORR_HOLDDATE");
        builder.Property(e => e.HoldRemarks).HasColumnName("CORR_HOLDREMARKS").HasMaxLength(200);
        builder.Property(e => e.HoldBy).HasColumnName("CORR_HOLDBY");
        builder.Property(e => e.HoldStatus).HasColumnName("CORR_HOLDSTATUS").HasMaxLength(1);
        builder.Property(e => e.ReleaseDate).HasColumnName("CORR_RELDATE");
        builder.Property(e => e.ReleaseRemarks).HasColumnName("CORR_RELREMARKS").HasMaxLength(200);
        builder.Property(e => e.ReleaseBy).HasColumnName("CORR_RELBY");
        builder.Property(e => e.HoldNature).HasColumnName("CORR_HOLDNATURE").HasPrecision(18, 2);

        builder.HasMany(e => e.Attachments).WithOne(e => e.Correspondence).HasForeignKey(e => e.CorrId);
    }
}

public class DocumentCorrespondenceAttachmentConfiguration : IEntityTypeConfiguration<DocumentCorrespondenceAttachment>
{
    public void Configure(EntityTypeBuilder<DocumentCorrespondenceAttachment> builder)
    {
        builder.ToTable("DOC_CORRESPONDATT");
        builder.HasKey(e => e.AttId);
        builder.Property(e => e.AttId).HasColumnName("ATT_ID");
        builder.Property(e => e.CorrId).HasColumnName("ATT_CORRID");
        builder.Property(e => e.CorrStatus).HasColumnName("ATT_CORRSTATUS").HasMaxLength(1);
        builder.Property(e => e.FilePath).HasColumnName("ATT_FILEPATH").HasMaxLength(200);
    }
}

public class DocumentDefectiveAttachmentConfiguration : IEntityTypeConfiguration<DocumentDefectiveAttachment>
{
    public void Configure(EntityTypeBuilder<DocumentDefectiveAttachment> builder)
    {
        builder.ToTable("DOC_DEFECTIVEATT");
        builder.HasKey(e => e.DefAttId);
        builder.Property(e => e.DefAttId).HasColumnName("DEFATT_ID");
        builder.Property(e => e.AllocationId).HasColumnName("DEFATT_ALLID");
        builder.Property(e => e.FilePath).HasColumnName("DEFATT_FILEPATH").HasMaxLength(200);
    }
}

public class DocumentStatusConfiguration : IEntityTypeConfiguration<DocumentStatus>
{
    public void Configure(EntityTypeBuilder<DocumentStatus> builder)
    {
        builder.ToTable("DOC_STATUS");
        builder.HasKey(e => e.Flag);
        builder.Property(e => e.Flag).HasColumnName("DOC_FLAG").HasMaxLength(2);
        builder.Property(e => e.Type).HasColumnName("DOC_TYPE").HasMaxLength(1);
        builder.Property(e => e.CompletedRemarks).HasColumnName("DOC_COMPLETEDREM").HasMaxLength(100);
        builder.Property(e => e.PendingRemarks).HasColumnName("DOC_PENDINGREM").HasMaxLength(100);
        builder.Property(e => e.StageOrder).HasColumnName("DOC_STAGEORDER");
        builder.Property(e => e.CategoryGroup).HasColumnName("DOC_CATGROUP").HasMaxLength(50);
        builder.Property(e => e.StageNo).HasColumnName("DOC_STAGENO");
    }
}

public class DocumentRescanDetailConfiguration : IEntityTypeConfiguration<DocumentRescanDetail>
{
    public void Configure(EntityTypeBuilder<DocumentRescanDetail> builder)
    {
        builder.ToTable("DOC_RESCANDET");
        builder.HasKey(e => e.RescanId);
        builder.Property(e => e.RescanId).HasColumnName("RESCAN_ID");
        builder.Property(e => e.DocId).HasColumnName("RESCAN_DOCID");
        builder.Property(e => e.AllocationId).HasColumnName("RESCAN_ALLID");
        builder.Property(e => e.Status).HasColumnName("RESCAN_STATUS").HasMaxLength(1);
        builder.Property(e => e.RescanDate).HasColumnName("RESCAN_DATE");
        builder.Property(e => e.RescanTo).HasColumnName("RESCAN_TO").HasMaxLength(1);
        builder.Property(e => e.Remarks).HasColumnName("RESCAN_REMARKS").HasMaxLength(100);
        builder.Property(e => e.CompletedOn).HasColumnName("RESCAN_ON");
        builder.Property(e => e.CompletedBy).HasColumnName("RESCAN_BY");
        builder.Property(e => e.CompletionRemarks).HasColumnName("RESCAN_COMPLETIONREM").HasMaxLength(100);
        builder.Property(e => e.FilePath).HasColumnName("RESCAN_FILEPATH").HasMaxLength(200);
    }
}

public class DocumentRevokeDetailConfiguration : IEntityTypeConfiguration<DocumentRevokeDetail>
{
    public void Configure(EntityTypeBuilder<DocumentRevokeDetail> builder)
    {
        builder.ToTable("DOC_REVOKEDET");
        builder.HasKey(e => e.RevokeDetailId);
        builder.Property(e => e.RevokeDetailId).HasColumnName("DOC_REVOKEDETID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.RevokeRemarks).HasColumnName("DOC_REVOKEREMARKS").HasMaxLength(1000);
        builder.Property(e => e.RevokeStatus).HasColumnName("DOC_REVOKESTATUS").HasMaxLength(10);
        builder.Property(e => e.RevokedBy).HasColumnName("DOC_REVOKEDBY");
        builder.Property(e => e.RevokedOn).HasColumnName("DOC_REVOKEDON");
    }
}

public class DocumentApproverConfiguration : IEntityTypeConfiguration<DocumentApprover>
{
    public void Configure(EntityTypeBuilder<DocumentApprover> builder)
    {
        builder.ToTable("DOC_APPROVER");
        builder.HasKey(e => e.ApprId);
        builder.Property(e => e.ApprId).HasColumnName("DOC_APPRID");
        builder.Property(e => e.BusinessUnit).HasColumnName("DOC_BU").HasMaxLength(25);
        builder.Property(e => e.Location).HasColumnName("DOC_LOC");
        builder.Property(e => e.ApproverType).HasColumnName("DOC_APPRTYPE").HasMaxLength(1);
        builder.Property(e => e.ApproverEmployeeId).HasColumnName("DOC_APPREMPID");
        builder.Property(e => e.EnteredBy).HasColumnName("DOC_ENTBY");
        builder.Property(e => e.EnteredOn).HasColumnName("DOC_ENTON");
    }
}

public class DocumentCounterConfiguration : IEntityTypeConfiguration<DocumentCounter>
{
    public void Configure(EntityTypeBuilder<DocumentCounter> builder)
    {
        builder.ToTable("DOC_COUNTER");
        builder.HasKey(e => e.BusinessUnitId);
        builder.Property(e => e.BusinessUnitId).HasColumnName("DOC_BUID").HasMaxLength(25);
        builder.Property(e => e.DocumentNo).HasColumnName("DOC_NO");
    }
}

public class DocumentDuplicateCheckConfiguration : IEntityTypeConfiguration<DocumentDuplicateCheck>
{
    public void Configure(EntityTypeBuilder<DocumentDuplicateCheck> builder)
    {
        builder.ToTable("DOC_DUPLICATE_CHK");
        builder.HasNoKey();
        builder.Property(e => e.DocId).HasColumnName("DOC_ID").HasMaxLength(50);
    }
}

public class OracleDueDetailConfiguration : IEntityTypeConfiguration<OracleDueDetail>
{
    public void Configure(EntityTypeBuilder<OracleDueDetail> builder)
    {
        builder.ToTable("DOC_ORACLEDUEDET");
        builder.HasKey(e => e.DueId);
        builder.Property(e => e.DueId).HasColumnName("DOC_DUEID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.OrgId).HasColumnName("ORG_ID");
        builder.Property(e => e.InvoiceId).HasColumnName("INVOICEID");
        builder.Property(e => e.VoucherNo).HasColumnName("VOUCHER_NO").HasPrecision(18, 2);
        builder.Property(e => e.DocumentId).HasColumnName("DOCUMENT_ID").HasMaxLength(150);
        builder.Property(e => e.DueDate).HasColumnName("DUEDATE");
        builder.Property(e => e.PaymentNum).HasColumnName("PAYMENT_NUM");
        builder.Property(e => e.DueAmount).HasColumnName("DUE_AMOUNT").HasPrecision(18, 2);
        builder.Property(e => e.DueCreatedBy).HasColumnName("DOC_DUECREATEDBY").HasPrecision(18, 2);
        builder.Property(e => e.DueCreatedOn).HasColumnName("DOC_DUECREATEDON");
    }
}

public class DocumentReportFieldConfiguration : IEntityTypeConfiguration<DocumentReportField>
{
    public void Configure(EntityTypeBuilder<DocumentReportField> builder)
    {
        builder.ToTable("DOC_REPORTFIELDS");
        builder.HasKey(e => e.FieldId);
        builder.Property(e => e.FieldId).HasColumnName("RPT_FIELDID");
        builder.Property(e => e.ColumnField).HasColumnName("RPT_COLFIELD").HasMaxLength(2000);
        builder.Property(e => e.ColumnDisplayField).HasColumnName("RPT_COLDISPFIELD").HasMaxLength(30);
    }
}

public class DocumentSharePointConfiguration : IEntityTypeConfiguration<DocumentSharePoint>
{
    public void Configure(EntityTypeBuilder<DocumentSharePoint> builder)
    {
        builder.ToTable("DOC_SHAREPOINT");
        builder.HasNoKey();
        builder.Property(e => e.SharePointId).HasColumnName("ID");
        builder.Property(e => e.Unit).HasColumnName("UNIT").HasMaxLength(50);
        builder.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(100);
        builder.Property(e => e.Category).HasColumnName("CATEGORY").HasMaxLength(100);
        builder.Property(e => e.SubCategory).HasColumnName("SUBCAT").HasMaxLength(100);
        builder.Property(e => e.Business).HasColumnName("BUSINESS").HasMaxLength(50);
        builder.Property(e => e.VendorNameSite).HasColumnName("VENDORNAMESITE").HasMaxLength(500);
        builder.Property(e => e.VendorName).HasColumnName("VENDORNAME").HasMaxLength(500);
        builder.Property(e => e.VendorSite).HasColumnName("VENDORSITE").HasMaxLength(500);
        builder.Property(e => e.PoNo).HasColumnName("PONO").HasMaxLength(500);
        builder.Property(e => e.MrcNo).HasColumnName("MRCNO").HasMaxLength(500);
        builder.Property(e => e.R12Voucher).HasColumnName("R12VOUCHER").HasMaxLength(100);
        builder.Property(e => e.Currency).HasColumnName("CURRENTCY").HasMaxLength(10);
        builder.Property(e => e.Amount).HasColumnName("AMOUNT").HasMaxLength(100);
        builder.Property(e => e.DocKey).HasColumnName("DOC_KEY").HasMaxLength(100);
        builder.Property(e => e.InvNo).HasColumnName("INV_NO").HasMaxLength(1000);
        builder.Property(e => e.InvDate).HasColumnName("INV_DATE").HasMaxLength(100);
        builder.Property(e => e.PayTo).HasColumnName("PAYTO").HasMaxLength(1000);
        builder.Property(e => e.VendorCode).HasColumnName("VENDORCODE").HasMaxLength(200);
        builder.Property(e => e.R12BuCode).HasColumnName("R12BUCODE").HasMaxLength(25);
    }
}
