# MedicineManagement & PurchaseManagement Modules

## Purpose
Manages pharmacy operations including medicine inventory, stock tracking, and medicine purchase transactions.

## MedicineManagement Tables

### Master Tables
- **MEDICINE_TYPMAST**: Medicine type definitions
  - Primary Key: MT_TYP_COD
  - Tracks different medicine categories

- **MEDICINE_PKG**: Packaging types
  - Primary Key: PK_PKG_COD
  - Supports different package sizes

- **MEDICINE_MAST**: Medicine master
  - Primary Key: MM_MED_COD
  - Includes min/max order levels
  - Category: H (High), M (Medium), L (Low)

### Operational Tables
- **DOCATTEND_MAST**: Doctor/Attendant information
  - Tracks medical professionals

- **MEDICINE_CREDIT**: Stock transactions
  - Record Type: O (Opening), P (Purchase), I (Issue), E (Expire)
  - Primary Key: MD_COM_COD
  - Lot number and quantity tracking

- **MEDICINE_ISSUE**: Medicine dispensing records
  - Links to VISIT_NUM for clinic visits
  - Tracks issued quantities

- **MED_DRCRFLG**: Doctor/Credit flags
  - Status flags for control

## PurchaseManagement Tables

### PURCHASE_MAIN
- **Primary Key**: MD_COM_COD, MD_TRN_NUM
- **Fields**:
  - MD_INV_NUM: Vendor invoice number
  - MD_INV_DAT: Invoice date
  - MD_INV_AMT: Invoice amount
  - MD_VND_NAM: Vendor name
  - MD_CAN_FLG: Cancellation flag
- **Indexes**: Company Code, Invoice Date

### PURCHASE_SUB
- **Primary Key**: MD_COM_COD, MD_TRN_NUM, MD_SRL_NUM
- **Fields**:
  - MD_MED_COD: Medicine code
  - MD_PKG_TYP: Packaging type
  - MD_PKG_QNT: Quantity per package
  - MD_TOT_QNT: Total quantity
  - MD_MFG_DAT: Manufacturing date
  - MD_EXP_DAT: Expiry date
  - MD_LOT_NUM: Lot/batch number
- **Indexes**: Transaction number

## Data Flow

### Purchase Process
```
PURCHASE_MAIN (header)
  └── PURCHASE_SUB (line items with lot details)
      └── Updates MEDICINE_CREDIT (O/P record type)
```

### Dispensing Process
```
VISIT_MAIN (clinic visit)
  └── MEDICINE_ISSUE (medicines given)
      └── Updates MEDICINE_CREDIT (I record type)
```

## Key Features

### Stock Management
- Opening balance (O)
- Purchases (P)
- Issues (I)
- Expiry (E)
- Lot tracking per medicine
- Min/Max order levels

### Audit Trail
- Entry user and date
- Modification user and date
- Cancellation flag

## Typical Operations

1. **Purchase Receipt**:
   - Create PURCHASE_MAIN
   - Add line items to PURCHASE_SUB
   - System creates MEDICINE_CREDIT (type P)

2. **Issue Medicine**:
   - Dispense from MEDICINE_ISSUE
   - Link to VISIT_NUM
   - System creates MEDICINE_CREDIT (type I)

3. **Inventory Count**:
   - Query MEDICINE_CREDIT by medicine code
   - Calculate available quantity (O + P - I)
   - Identify expired items

4. **Reorder**:
   - Compare current qty with MM_ORD_MIN
   - Generate purchase orders when below minimum

## Notes
- Supports complex lot tracking
- Expiry date management crucial
- Financial reconciliation via invoices
- Multi-company support (MD_COM_COD)
