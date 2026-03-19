using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceProcessing.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOC_APPROVER",
                columns: table => new
                {
                    DOC_APPRID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_BU = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOC_LOC = table.Column<long>(type: "bigint", nullable: false),
                    DOC_APPRTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DOC_APPREMPID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_ENTBY = table.Column<long>(type: "bigint", nullable: false),
                    DOC_ENTON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_APPROVER", x => x.DOC_APPRID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_COUNTER",
                columns: table => new
                {
                    DOC_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOC_NO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_COUNTER", x => x.DOC_BUID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_DEFECTIVEATT",
                columns: table => new
                {
                    DEFATT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEFATT_ALLID = table.Column<long>(type: "bigint", nullable: false),
                    DEFATT_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_DEFECTIVEATT", x => x.DEFATT_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_DET",
                columns: table => new
                {
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ORGID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOC_LOCID = table.Column<int>(type: "int", nullable: false),
                    DOC_NO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DOC_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DOC_MAINCAT = table.Column<long>(type: "bigint", nullable: false),
                    DOC_SUBCAT = table.Column<long>(type: "bigint", nullable: false),
                    DOC_PONO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOC_VNDSITEID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_VNDID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_DUEDAYS = table.Column<int>(type: "int", nullable: false),
                    DOC_POID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_MRCREM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_VATFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DOC_INVOICENO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOC_INVAMT = table.Column<long>(type: "bigint", nullable: false),
                    DOC_CURRENCY = table.Column<int>(type: "int", nullable: false),
                    DOC_INVDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_INVRECDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_PAGES = table.Column<long>(type: "bigint", nullable: false),
                    DOC_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DOC_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_PAYBY = table.Column<int>(type: "int", nullable: false),
                    DOC_SIGNATORY1 = table.Column<long>(type: "bigint", nullable: true),
                    DOC_SIGNATORY2 = table.Column<long>(type: "bigint", nullable: true),
                    DOC_APPROVER = table.Column<long>(type: "bigint", nullable: true),
                    DOC_OWNER = table.Column<long>(type: "bigint", nullable: false),
                    DOC_DOCSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DOC_INVSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DOC_USERID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_SUBMITTEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_RECEIVEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DOC_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_CANCELFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DOC_CANCELUSER = table.Column<long>(type: "bigint", nullable: false),
                    DOC_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOC_APALLID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_ORAINVNO = table.Column<long>(type: "bigint", nullable: false),
                    DOC_PAYTYPENO = table.Column<long>(type: "bigint", nullable: false),
                    DOC_ACCOUNTCODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOC_SSCINVOICEPDF = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_KEY = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DOC_USRINVOICEPDF = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_INVPROCDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOC_INVPROCALLID = table.Column<long>(type: "bigint", nullable: true),
                    DOC_INVVALIDDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOC_INVVALIDALLID = table.Column<long>(type: "bigint", nullable: true),
                    DOC_HOLDSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_DEDUCTION = table.Column<long>(type: "bigint", nullable: true),
                    DOC_THIRDPARTYFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_THIRDPARTYVENDOR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOC_DEDUCTIONREMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DOC_FILEID = table.Column<long>(type: "bigint", nullable: true),
                    DOC_CANCELREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_HOLDPAYMENTFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_HOLDPAYMENTREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_HOLDRELEASEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_SCANFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_DET", x => x.DOC_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_DUPLICATE_CHK",
                columns: table => new
                {
                    DOC_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DOC_REPORTFIELDS",
                columns: table => new
                {
                    RPT_FIELDID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RPT_COLFIELD = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RPT_COLDISPFIELD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_REPORTFIELDS", x => x.RPT_FIELDID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_SHAREPOINT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: true),
                    UNIT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CATEGORY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SUBCAT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BUSINESS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VENDORNAMESITE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VENDORNAME = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VENDORSITE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PONO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MRCNO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    R12VOUCHER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CURRENTCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AMOUNT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DOC_KEY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    INV_NO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    INV_DATE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PAYTO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VENDORCODE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    R12BUCODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DOC_STATUS",
                columns: table => new
                {
                    DOC_FLAG = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DOC_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DOC_COMPLETEDREM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOC_PENDINGREM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOC_STAGEORDER = table.Column<long>(type: "bigint", nullable: true),
                    DOC_CATGROUP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOC_STAGENO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_STATUS", x => x.DOC_FLAG);
                });

            migrationBuilder.CreateTable(
                name: "DOC_APALLDET",
                columns: table => new
                {
                    APALL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APALL_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    APALL_ACTION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APALL_GROUPID = table.Column<long>(type: "bigint", nullable: false),
                    APALL_PULLSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APALL_PULLUSERID = table.Column<long>(type: "bigint", nullable: false),
                    APALL_PRIORITY = table.Column<int>(type: "int", nullable: false),
                    APALL_ALLBY = table.Column<long>(type: "bigint", nullable: false),
                    APALL_ALLON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APALL_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APALL_ACTIONFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APALL_ACTIONDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    APALL_CORRID = table.Column<long>(type: "bigint", nullable: true),
                    APALL_DEFTYPE = table.Column<long>(type: "bigint", nullable: true),
                    APALL_CLOSEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APALL_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    APALL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APALL_PULLEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_APALLDET", x => x.APALL_ID);
                    table.ForeignKey(
                        name: "FK_DOC_APALLDET_DOC_DET_APALL_DOCID",
                        column: x => x.APALL_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_APPDET",
                columns: table => new
                {
                    APP_SEQID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APP_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    APP_USERID = table.Column<long>(type: "bigint", nullable: false),
                    APP_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APP_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_APPDET", x => x.APP_SEQID);
                    table.ForeignKey(
                        name: "FK_DOC_APPDET_DOC_DET_APP_DOCID",
                        column: x => x.APP_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_ATTACHMENT",
                columns: table => new
                {
                    ATTACH_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ATTACH_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    ATTACH_FILEPATH = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_ATTACHMENT", x => x.ATTACH_ID);
                    table.ForeignKey(
                        name: "FK_DOC_ATTACHMENT_DOC_DET_ATTACH_DOCID",
                        column: x => x.ATTACH_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_CC",
                columns: table => new
                {
                    CC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CC_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    CC_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_LOCCODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_ACCOUNTCODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_SUBACC = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_PRODUCT = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CC_PER = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_CC", x => x.CC_ID);
                    table.ForeignKey(
                        name: "FK_DOC_CC_DOC_DET_CC_DOCID",
                        column: x => x.CC_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_CORRESPOND",
                columns: table => new
                {
                    CORR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CORR_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    CORR_ALLID = table.Column<long>(type: "bigint", nullable: false),
                    CORR_HOLDCAT = table.Column<long>(type: "bigint", nullable: false),
                    CORR_HOLDTYPE = table.Column<long>(type: "bigint", nullable: false),
                    CORR_HOLDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CORR_HOLDREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CORR_HOLDBY = table.Column<long>(type: "bigint", nullable: false),
                    CORR_HOLDSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CORR_RELDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CORR_RELREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CORR_RELBY = table.Column<long>(type: "bigint", nullable: true),
                    CORR_HOLDNATURE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_CORRESPOND", x => x.CORR_ID);
                    table.ForeignKey(
                        name: "FK_DOC_CORRESPOND_DOC_DET_CORR_DOCID",
                        column: x => x.CORR_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_MRCLIST",
                columns: table => new
                {
                    MRC_SEQID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRC_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    MRC_LINEID = table.Column<long>(type: "bigint", nullable: false),
                    MRC_ID = table.Column<long>(type: "bigint", nullable: false),
                    MRC_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MRC_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MRC_PO_LINEID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_MRCLIST", x => x.MRC_SEQID);
                    table.ForeignKey(
                        name: "FK_DOC_MRCLIST_DOC_DET_MRC_DOCID",
                        column: x => x.MRC_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_ORACLEBNKDET",
                columns: table => new
                {
                    DOC_BNKID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CHECK_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BUSINESS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ORG_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_SITE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FILE_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RECORD_IDETIFIER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MAIL_TO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_MAIL_ADDRESS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_BANK_AC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PAY_TO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CHECK_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AMOUNT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HUNDI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PAYMENT_LOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PAYMENT_NUMBER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CHECK_NUMBER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PAYMENT_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RECORS_ANNEXURE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PRINT_LOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_IFSC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_ACCOUNT_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_BANK_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_BANK_AC22 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_BANK_BRANCH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_BANK_LOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BENE_MAIL_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REF_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UTR_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REJECT_REASON1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REJECT_REASON2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    STATUS_LOOKUP_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_ORACLEBNKDET", x => x.DOC_BNKID);
                    table.ForeignKey(
                        name: "FK_DOC_ORACLEBNKDET_DOC_DET_DOC_ID",
                        column: x => x.DOC_ID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_ORACLEDUEDET",
                columns: table => new
                {
                    DOC_DUEID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    ORG_ID = table.Column<long>(type: "bigint", nullable: true),
                    INVOICEID = table.Column<long>(type: "bigint", nullable: false),
                    VOUCHER_NO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOCUMENT_ID = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PAYMENT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    DUE_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_DUECREATEDBY = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_DUECREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_ORACLEDUEDET", x => x.DOC_DUEID);
                    table.ForeignKey(
                        name: "FK_DOC_ORACLEDUEDET_DOC_DET_DOC_ID",
                        column: x => x.DOC_ID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_ORACLEINVDET",
                columns: table => new
                {
                    DOC_INVID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_VOUCHERNO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_INVOICETYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DOC_VENDORID = table.Column<long>(type: "bigint", nullable: true),
                    DOC_VENDOR_SITEID = table.Column<long>(type: "bigint", nullable: true),
                    DOC_INVOICENUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOC_INVOICEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOC_INVOICEAMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_INVOICEID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_INVOICESTATUS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DOC_INVOICECREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOC_INVOICECREATEDBY = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_PAYMENT_METHOD_CODE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DOC_ACCOUNTING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_ORACLEINVDET", x => x.DOC_INVID);
                    table.ForeignKey(
                        name: "FK_DOC_ORACLEINVDET_DOC_DET_DOC_ID",
                        column: x => x.DOC_ID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_ORACLEPAYDET",
                columns: table => new
                {
                    DOC_PAYID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_PAYMENTNUM = table.Column<long>(type: "bigint", nullable: false),
                    DOC_INVOICEID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOC_GROSSAMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_AMOUNTREMAINING = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_PAYMENT_STATUS = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    DOC_PAYMENT_METHOD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DOC_PREPAYMENT_APPLIED = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DOC_PAYMENT_CREATEDBY = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DOC_PAYMENT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHECK_ID = table.Column<long>(type: "bigint", nullable: true),
                    BNKSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CHECK_NUMBER = table.Column<long>(type: "bigint", nullable: true),
                    CHECK_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHECK_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_ORACLEPAYDET", x => x.DOC_PAYID);
                    table.ForeignKey(
                        name: "FK_DOC_ORACLEPAYDET_DOC_DET_DOC_ID",
                        column: x => x.DOC_ID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_POLIST",
                columns: table => new
                {
                    PO_SEQID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PO_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    PO_ID = table.Column<long>(type: "bigint", nullable: false),
                    PO_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PO_LINENO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PO_LINE_ID = table.Column<long>(type: "bigint", nullable: true),
                    PO_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PO_TERM_ID = table.Column<long>(type: "bigint", nullable: true),
                    PO_TERM_SEQNO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_POLIST", x => x.PO_SEQID);
                    table.ForeignKey(
                        name: "FK_DOC_POLIST_DOC_DET_PO_DOCID",
                        column: x => x.PO_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_RESCANDET",
                columns: table => new
                {
                    RESCAN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RESCAN_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    RESCAN_ALLID = table.Column<long>(type: "bigint", nullable: false),
                    RESCAN_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RESCAN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RESCAN_TO = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RESCAN_REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RESCAN_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RESCAN_BY = table.Column<long>(type: "bigint", nullable: true),
                    RESCAN_COMPLETIONREM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RESCAN_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_RESCANDET", x => x.RESCAN_ID);
                    table.ForeignKey(
                        name: "FK_DOC_RESCANDET_DOC_DET_RESCAN_DOCID",
                        column: x => x.RESCAN_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_REVOKEDET",
                columns: table => new
                {
                    DOC_REVOKEDETID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_REVOKEREMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DOC_REVOKESTATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DOC_REVOKEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DOC_REVOKEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_REVOKEDET", x => x.DOC_REVOKEDETID);
                    table.ForeignKey(
                        name: "FK_DOC_REVOKEDET_DOC_DET_DOC_ID",
                        column: x => x.DOC_ID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_SSFILELIST",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FILE_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_PATH = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_SSFILELIST", x => x.FILE_ID);
                    table.ForeignKey(
                        name: "FK_DOC_SSFILELIST_DOC_DET_FILE_DOCID",
                        column: x => x.FILE_DOCID,
                        principalTable: "DOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOC_CORRESPONDATT",
                columns: table => new
                {
                    ATT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ATT_CORRID = table.Column<long>(type: "bigint", nullable: false),
                    ATT_CORRSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ATT_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_CORRESPONDATT", x => x.ATT_ID);
                    table.ForeignKey(
                        name: "FK_DOC_CORRESPONDATT_DOC_CORRESPOND_ATT_CORRID",
                        column: x => x.ATT_CORRID,
                        principalTable: "DOC_CORRESPOND",
                        principalColumn: "CORR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOC_APALLDET_APALL_DOCID",
                table: "DOC_APALLDET",
                column: "APALL_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_APPDET_APP_DOCID",
                table: "DOC_APPDET",
                column: "APP_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_ATTACHMENT_ATTACH_DOCID",
                table: "DOC_ATTACHMENT",
                column: "ATTACH_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_CC_CC_DOCID",
                table: "DOC_CC",
                column: "CC_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_CORRESPOND_CORR_DOCID",
                table: "DOC_CORRESPOND",
                column: "CORR_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_CORRESPONDATT_ATT_CORRID",
                table: "DOC_CORRESPONDATT",
                column: "ATT_CORRID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_DET_DOC_DOCSTATUS",
                table: "DOC_DET",
                column: "DOC_DOCSTATUS");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_DET_DOC_INVOICENO",
                table: "DOC_DET",
                column: "DOC_INVOICENO");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_DET_DOC_ORGID",
                table: "DOC_DET",
                column: "DOC_ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_DET_DOC_VNDID",
                table: "DOC_DET",
                column: "DOC_VNDID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_MRCLIST_MRC_DOCID",
                table: "DOC_MRCLIST",
                column: "MRC_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_ORACLEBNKDET_DOC_ID",
                table: "DOC_ORACLEBNKDET",
                column: "DOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_ORACLEDUEDET_DOC_ID",
                table: "DOC_ORACLEDUEDET",
                column: "DOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_ORACLEINVDET_DOC_ID",
                table: "DOC_ORACLEINVDET",
                column: "DOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_ORACLEPAYDET_DOC_ID",
                table: "DOC_ORACLEPAYDET",
                column: "DOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_POLIST_PO_DOCID",
                table: "DOC_POLIST",
                column: "PO_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_RESCANDET_RESCAN_DOCID",
                table: "DOC_RESCANDET",
                column: "RESCAN_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_REVOKEDET_DOC_ID",
                table: "DOC_REVOKEDET",
                column: "DOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOC_SSFILELIST_FILE_DOCID",
                table: "DOC_SSFILELIST",
                column: "FILE_DOCID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOC_APALLDET");

            migrationBuilder.DropTable(
                name: "DOC_APPDET");

            migrationBuilder.DropTable(
                name: "DOC_APPROVER");

            migrationBuilder.DropTable(
                name: "DOC_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "DOC_CC");

            migrationBuilder.DropTable(
                name: "DOC_CORRESPONDATT");

            migrationBuilder.DropTable(
                name: "DOC_COUNTER");

            migrationBuilder.DropTable(
                name: "DOC_DEFECTIVEATT");

            migrationBuilder.DropTable(
                name: "DOC_DUPLICATE_CHK");

            migrationBuilder.DropTable(
                name: "DOC_MRCLIST");

            migrationBuilder.DropTable(
                name: "DOC_ORACLEBNKDET");

            migrationBuilder.DropTable(
                name: "DOC_ORACLEDUEDET");

            migrationBuilder.DropTable(
                name: "DOC_ORACLEINVDET");

            migrationBuilder.DropTable(
                name: "DOC_ORACLEPAYDET");

            migrationBuilder.DropTable(
                name: "DOC_POLIST");

            migrationBuilder.DropTable(
                name: "DOC_REPORTFIELDS");

            migrationBuilder.DropTable(
                name: "DOC_RESCANDET");

            migrationBuilder.DropTable(
                name: "DOC_REVOKEDET");

            migrationBuilder.DropTable(
                name: "DOC_SHAREPOINT");

            migrationBuilder.DropTable(
                name: "DOC_SSFILELIST");

            migrationBuilder.DropTable(
                name: "DOC_STATUS");

            migrationBuilder.DropTable(
                name: "DOC_CORRESPOND");

            migrationBuilder.DropTable(
                name: "DOC_DET");
        }
    }
}
