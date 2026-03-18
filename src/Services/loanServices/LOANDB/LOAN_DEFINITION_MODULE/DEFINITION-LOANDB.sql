-- ==========================================
-- Module: LOAN_DEFINITION
-- Database: LOANDB
-- Description: Loan Products, Rules, and Setup
-- ==========================================

USE [LOANDB];
GO

-- Table: LOAN_TYPEMASTER - Loan Type
CREATE TABLE [LOAN_TYPEMASTER] (
    [LOAN_TYPE] BIGINT NOT NULL  -- Loan Type,
    [LOAN_NAME] VARCHAR(200) NOT NULL  -- Loan Name,
    [LOAN_CATEGORY] CHAR(10) NOT NULL  -- Loan Category,
    [LOAN_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [LOAN_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOAN_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [LOAN_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [PK_LOAN_TYPEMASTER] PRIMARY KEY ([LOAN_TYPE])
);
GO

-- Table: LOAN_MASTER - Loan Master
CREATE TABLE [LOAN_MASTER] (
    [LOAN_ID] BIGINT NOT NULL  -- Loan ID,
    [LOAN_NAME] VARCHAR(65) NOT NULL  -- Loan Name,
    [LOAN_PURPOSE] VARCHAR(200) NOT NULL  -- Purpose of Loan,
    [LOAN_APPLYToUNIT] INT NOT NULL  -- Apply to all or Selected units  0 - All ; 1 - SEL,
    [LOAN_ORGID] BIGINT NOT NULL  -- Org ID (If the Loan is applied to all units),
    [LOAN_UNITID] BIGINT NOT NULL  -- Payroll Unit ID,
    [LOAN_TYPEID] BIGINT NOT NULL  -- Loan Type,
    [LOAN_APPLYToCONFIRMEMP] CHAR(1) NOT NULL  -- Applicable for Confirmed Employees,
    [LOAN_GRADECATAGORY] CHAR(3) NOT NULL  -- Grade Category,
    [LOAN_APPLYToALLGRADE] INT NOT NULL  -- Apply to all Grades (0 - ALL ; 1 - SEL),
    [LOAN_GRADEID] BIGINT NOT NULL  -- Grade ID,
    [LOAN_MINIMUMLIMIT] BIGINT NOT NULL  -- Minimum Limit for Principal Amount,
    [LOAN_MAXIMUMLIMIT] BIGINT NOT NULL  -- Maximum Limit for Principal Amount,
    [LOAN_AUTOPAYONCOMPLETION] CHAR(1) NOT NULL  -- Auto payment of new loan on completion of loan repayment (Y/N),
    [LOAN_ALLOWFORCECLOSE] CHAR(1) NOT NULL  -- Allow to foreclose in the middle and apply,
    [LOAN_ALLOWMULTIPLENOS] CHAR(1) NOT NULL  -- Multiple loans taken at the same time,
    [LOAN_ONCONFIRMATION] CHAR(1) NOT NULL  -- Loan can be taken only on Confirmation (Y/N),
    [LOAN_CHECKENTITLEMENT] CHAR(1) NOT NULL  -- Check Entitlement on Loan application (Y/N),
    [LOAN_RECOVERABLE] CHAR(1) NOT NULL  -- Recoverable (Y/N),
    [LOAN_APPLICATIONNOS] INT NOT NULL  -- No of Times can be applied in an Employee's career; 0- Indicate No limit on no of times loan can be taken,
    [LOAN_CHECKNETPAYPERCENTAGE] CHAR(1) NOT NULL  -- Net pay (based on past month salary) Percentage should be checked at the time of application,
    [LOAN_BKDINTERESTRATEREVISION] CHAR(1) NOT NULL  -- Backdated Interest Rate revision allowed,
    [LOAN_SUBCLASSAVAILABLE] CHAR(1) NOT NULL  -- Sub Class available (Y/N),
    [LOAN_ITCLASS] CHAR(3) NULL  -- IT Classification - Program LOV Master,
    [LOAN_DOCUMENTREQUIRED] CHAR(1) NOT NULL  -- Documents Required,
    [LOAN_DOCUMENTUPLOADREQUIRED] CHAR(1) NOT NULL  -- Document upload required,
    [LOAN_SLFAPPALLOWED] CHAR(1) NOT NULL  -- Self Application Allowed (Y/N),
    [LOAN_EMPSPECIFICRATESALLOWED] CHAR(1) NOT NULL  -- Loan application specific interest rate (Y/N),
    [LOAN_HRAPPROVAL] CHAR(1) NOT NULL  -- HR Approval required - Y/N,
    [LOAN_EFFDATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [LOAN_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    [LOAN_COMFACTOR] CHAR(1) NOT NULL  -- Compounding Factor,
    [LOAN_INTFREQUENCY] CHAR(1) NOT NULL  -- Interest Frequency,
    [LOAN_RECTYPE] CHAR(3) NOT NULL  -- Recovery Method (RBM, EM1,EMA,FPI),
    [LOAN_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [LOAN_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOAN_LASTMODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOAN_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    [LOAN_BULKUPLOADALLOWED] CHAR(1) NOT NULL  -- Bulk Upload Allowed (Y/N),
    [LOAN_PRNRECEDID] BIGINT NOT NULL  -- Principal Recovery ED ID,
    [LOAN_INTRECEDID] BIGINT NOT NULL  -- Interest Recovery ED ID,
    [LOAN_PRNPAYEDID] BIGINT NOT NULL  -- Principal Payment ED ID,
    [LOAN_POLICYFILENAME] VARCHAR(250) NOT NULL  -- Policy attachment,
    [LOAN_GUARANTORREQUIRED] CHAR(1) NOT NULL  -- Guarantor Required Yes/No,
    [LOAN_CHKBASICENTITLEMENT] CHAR(1) NOT NULL  -- Check Entitlement on Loan application (Y/N),
    [LOAN_ALLOWADDLLOAN] CHAR(1) NOT NULL  -- Allow additional Loan Yes/No,
    [LOAN_ADDITONALLOANNO] BIGINT NOT NULL  -- Allow additional Loan number of application,
    [LOAN_CURRECOVERY] CHAR(1) NOT NULL  -- Current month recovery Yes/No,
    [LOAN_REPUNITAPPLICABLE] CHAR(1) NOT NULL  -- Reporting Unit applicable Yes/No,
    [LOAN_REPUNITID] INT NOT NULL  -- Reporting Unit (0- All/ or specific unit ID),
    [LOAN_FLEXIFIRSTINSDATE] CHAR(1) NOT NULL  -- Flexible First Installment Date,
    CONSTRAINT [PK_LOAN_MASTER] PRIMARY KEY ([LOAN_ID])
);
GO

-- Table: LOAN_SUBCLASS - Loan Subclass
CREATE TABLE [LOAN_SUBCLASS] (
    [SUBCLASS_ID] BIGINT NOT NULL  -- Sub Class ID,
    [SUBCLASS_LOANID] BIGINT NOT NULL  -- Loan ID,
    [SUBCLASS_DESC] VARCHAR(200) NOT NULL  -- Sub Class Definition,
    [SUBCLASS_IT] CHAR(3) NULL  -- IT Classification - Program LOV Master,
    [SUBCLASS_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [SUBCLASS_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    [SUBCLASS_PRNRECEDID] BIGINT NULL,
    [SUBCLASS_INTRECEDID] BIGINT NULL,
    CONSTRAINT [PK_LOAN_SUBCLASS] PRIMARY KEY ([SUBCLASS_ID])
);
GO

-- Table: LOAN_INTRATEMAST - Loan Interest Rate Master
CREATE TABLE [LOAN_INTRATEMAST] (
    [LOANINT_RATEID] BIGINT NOT NULL  -- Loan Rule ID,
    [LOANINT_LOANID] BIGINT NOT NULL  -- Loan ID,
    [LOANINT_EFFDATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [LOANINT_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    [LOANINT_RATE] INT NOT NULL  -- Interest Rate (%),
    [LOANINT_LASTMODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOANINT_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    [LOANINT_EMIAMT] BIGINT NOT NULL  -- EMI Amount/Instalment Amount,
    [LOANINT_INSNOS] INT NOT NULL  -- No of Installments,
    [LOANINT_RANGESPECIFIC] CHAR(1) NOT NULL  -- Range Specific interest.,
    CONSTRAINT [PK_LOAN_INTRATEMAST] PRIMARY KEY ([LOANINT_RATEID])
);
GO

-- Table: LOANLIMITRANGE_MAST - Loan Limit Range Master
CREATE TABLE [LOANLIMITRANGE_MAST] (
    [LOANLIMITRANGE_RATEID] BIGINT NOT NULL  -- Loan Range Rate ID,
    [LOANLIMITRANGE_LOANID] BIGINT NOT NULL  -- Loan ID,
    [LOANLIMITRANGE_MINYEAR] BIGINT NOT NULL  -- Minimum Years,
    [LOANLIMITRANGE_MAXYEAR] BIGINT NOT NULL  -- Maximum Years,
    [LOANLIMITRANGE_LOANAMOUNT] DECIMAL(19,0) NOT NULL  -- Loan Amount (Value),
    [LOANLIMITRANGE_EFFDATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [LOANLIMITRANGE_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    [LOANLIMITRANGE_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [LOANLIMITRANGE_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOANLIMITRANGE_MODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOANLIMITRANGE_MODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    [LOANLIMITRANGE_INTRATE] DECIMAL(38) NOT NULL  -- Interest Rate,
    [LOANLIMITRANGE_ADDLMINVALUE] DECIMAL(19,0) NULL,
    CONSTRAINT [PK_LOANLIMITRANGE_MAST] PRIMARY KEY ([LOANLIMITRANGE_RATEID])
);
GO

-- Table: LOAN_PRQ - Loan IT Perquisite
CREATE TABLE [LOAN_PRQ] (
    [LOAN_PRQID] BIGINT NOT NULL  -- Loan Perquisite ID,
    [LOAN_CLASSID] CHAR(3) NOT NULL  -- IT Class,
    [LOAN_EFFDATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [LOAN_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    [LOAN_ITINTRATE] INT NOT NULL  -- IT Interest Rate,
    [LOAN_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [LOAN_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    [LOAN_MINAMT] DECIMAL(19,0) NOT NULL  -- Minimum Loan Amount for Perquisite Computation,
    CONSTRAINT [PK_LOAN_PRQ] PRIMARY KEY ([LOAN_PRQID])
);
GO

-- Table: LOAN_FESTIVALS - Loan Festivals
CREATE TABLE [LOAN_FESTIVALS] (
    [LOANFEST_ID] BIGINT NOT NULL  -- Festival ID,
    [LOANFEST_DESC] VARCHAR(200) NOT NULL  -- Festival Description,
    [LOANFEST_STRDATE] DATETIME2(3) NOT NULL  -- Start Date,
    [LOANFEST_ENDDATE] DATETIME2(3) NOT NULL  -- End Date,
    [LOANFEST_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [LOANFEST_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [PK_LOAN_FESTIVALS] PRIMARY KEY ([LOANFEST_ID])
);
GO

-- Table: LOAN_FESTIVALMAP - Loan Festival Map
CREATE TABLE [LOAN_FESTIVALMAP] (
    [LOANFESTMAP_ID] BIGINT NOT NULL  -- Loan Festival Map ID,
    [LOANFESTMAP_LOANID] BIGINT NOT NULL  -- Loan ID,
    [LOANFESTMAP_FESTIVALID] BIGINT NOT NULL  -- Festival ID,
    [LOANFESTMAP_MODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOANFESTMAP_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [PK_LOAN_FESTIVALMAP] PRIMARY KEY ([LOANFESTMAP_ID])
);
GO

-- Table: LOAN_ACCMAST - Loan Account Master (Types corrected)
CREATE TABLE [LOAN_ACCMAST] (
    [LOAN_ACID] BIGINT NOT NULL  -- Account ID,
    [LOAN_TYPE] BIGINT NOT NULL  -- Loan Type,
    [LOAN_GRADETYPE] CHAR(3) NOT NULL  -- Grade Type,
    [LOAN_ACCODE] CHAR(5) NOT NULL  -- AC Code,
    [LOAN_UPDATEDBY] BIGINT NOT NULL  -- Last Updated By,
    [LOAN_UPDATEDON] DATETIME2(3) NOT NULL  -- Last Updated On,
    CONSTRAINT [PK_LOAN_ACCMAST] PRIMARY KEY ([LOAN_ACID])
);
GO

-- Indexes on Foreign Key Columns
CREATE INDEX [IDX_LOAN_INTRATEMAST_LOANINT_LOANID] ON [LOAN_INTRATEMAST]([LOANINT_LOANID]);
GO
CREATE INDEX [IDX_LOAN_MASTER_LOAN_TYPEID] ON [LOAN_MASTER]([LOAN_TYPEID]);
GO
CREATE INDEX [IDX_LOAN_PRQ_LOAN_CLASSID] ON [LOAN_PRQ]([LOAN_CLASSID]);
GO
