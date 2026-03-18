Loan Database Microservices Design Document

LOANDB.sql

SQL 21.31KB

this document i want to create modulus in technical document in microservices in c#

We need to parse the provided SQL file which creates a bunch of tables for a loan management database. The user says: "this document i want to create modulus in technical document in microservices in c#". Possibly they want to create a modular design or module documentation for microservices based on this database schema. It might be a request to generate a technical design document that outlines microservices boundaries, entities, and possibly C# code artifacts.



Interpretation: The user likely wants to design microservices around this loan database schema. They might want to decompose the monolithic database into domain-driven microservices, each owning certain tables/aggregates. They also mention "modulus" possibly meaning modules. They want a technical document describing these modules/microservices in the context of C#.



Given that the input is a SQL schema, we need to analyze the tables and propose a set of microservices, their responsibilities, and maybe some C# class outlines. The output should be a technical document.



We'll produce a structured document that:



Introduces the database schema.



Proposes microservice boundaries (e.g., Loan Management, Employee Management, Document Management, etc.) based on related tables.



Describes each microservice's purpose, key entities, and potential relationships.



May include C# code snippets for models (classes) and perhaps API endpoints.



Possibly includes a diagram description (textual).



Addresses considerations like data consistency, communication between services.



We need to be concise but comprehensive.



Given the tables, we can group them into logical domains:



Loan Core: LOAN\_MAIN, LOAN\_APPLICATION, LOAN\_ADDITIONAL, LOAN\_ADJUSTMENT, LOAN\_LEDGER, LOAN\_SET, LOAN\_INS, LOAN\_EMPINTRATEMAST, LOAN\_INTRATEMAST, LOANLIMITRANGE\_MAST, LOAN\_MASTER, LOAN\_SUBCLASS, LOAN\_TYPEMASTER, LOAN\_PRQ, LOAN\_FESTIVALMAP, LOAN\_FESTIVALS, maybe LOAN\_ACCMAST.



Documents: LOAN\_DOCUMENTS.



LOV (List of Values): LOV\_MASTER, LOV\_TYPEMAST, PROGRAMLOV\_MAST (lookup tables).



IUTA (Inter-Unit Transfer Accounting?): LOAN\_IUTAJV, TEMPLOAN\_IUTAJV.



TOAD\_PLAN\_SQL seems like a utility table, ignore.



LOAN\_UPD is a simple update table, maybe part of loan.



We might also need to consider that some tables reference employee, unit, grade etc., which are likely in other systems (HR, payroll). So microservices will need to integrate with those.



We'll propose services like:



LoanDefinitionService (manages loan products: LOAN\_MASTER, LOAN\_TYPEMASTER, LOAN\_SUBCLASS, LOAN\_INTRATEMAST, LOANLIMITRANGE\_MAST, LOAN\_PRQ, LOAN\_FESTIVALMAP, LOAN\_FESTIVALS).



LoanApplicationService (handles loan applications: LOAN\_APPLICATION, maybe LOAN\_ADDITIONAL).



LoanAccountService (manages active loans: LOAN\_MAIN, LOAN\_EMPINTRATEMAST, LOAN\_INS, LOAN\_LEDGER, LOAN\_SET, LOAN\_ADJUSTMENT, LOAN\_IUTAJV).



DocumentService (handles loan documents: LOAN\_DOCUMENTS).



LookupService (handles LOVs: LOV\_MASTER, LOV\_TYPEMAST, PROGRAMLOV\_MAST).



We'll also need to consider shared kernel or integration events.



We'll produce a document with sections: Introduction, Domain Analysis, Microservice Boundaries, Data Ownership, Service Contracts (APIs), Data Models (C#), Inter-service Communication, and Conclusion.



We'll include C# code snippets for some entity classes, maybe using EF Core.



We need to ensure it's a "modulus in technical document" meaning maybe a modular technical design. We'll write in a clear, professional manner.



Now, we'll generate the response.



Technical Design Document: Loan Management Microservices

1\. Introduction

This document outlines the decomposition of the monolithic loan management database (LOANDB) into a set of modular microservices. The goal is to create a scalable, maintainable, and domain‑driven architecture using C# and .NET. Each microservice will own a subset of related tables and expose APIs for business operations. The design emphasizes clear boundaries, data ownership, and interoperability.



2\. Domain Analysis

The database schema reveals several distinct business capabilities:



Domain Area	Key Tables	Description

Loan Product Definition	LOAN\_MASTER, LOAN\_TYPEMASTER, LOAN\_SUBCLASS, LOAN\_INTRATEMAST, LOANLIMITRANGE\_MAST, LOAN\_PRQ, LOAN\_FESTIVALMAP, LOAN\_FESTIVALS	Defines loan types, interest rates, limits, festivals, and perquisite rules.

Loan Application	LOAN\_APPLICATION, LOAN\_ADDITIONAL	Handles employee loan applications, approvals, and guarantor information.

Loan Account Management	LOAN\_MAIN, LOAN\_EMPINTRATEMAST, LOAN\_INS, LOAN\_LEDGER, LOAN\_SET, LOAN\_ADJUSTMENT, LOAN\_IUTAJV, TEMPLOAN\_IUTAJV	Manages active loans, instalments, ledger entries, settlements, adjustments, and inter‑unit transfers.

Document Management	LOAN\_DOCUMENTS	Stores references to loan‑related documents.

Lookups	LOV\_MASTER, LOV\_TYPEMAST, PROGRAMLOV\_MAST	Centralised list‑of‑values for dropdowns and enumerations.

Tables like LOAN\_ACCMAST (with incorrect data types) and TOAD\_PLAN\_SQL are considered legacy and will be handled separately or deprecated.



3\. Microservice Boundaries

Based on the domain analysis, we propose the following microservices:



3.1 LoanDefinitionService

Purpose: Manages loan products, interest rate rules, eligibility criteria, and festival calendars.



Owned Tables: LOAN\_MASTER, LOAN\_TYPEMASTER, LOAN\_SUBCLASS, LOAN\_INTRATEMAST, LOANLIMITRANGE\_MAST, LOAN\_PRQ, LOAN\_FESTIVALMAP, LOAN\_FESTIVALS.



Key Operations:



CRUD on loan types and subclasses.



Define interest rate slabs and effective periods.



Manage festival dates for seasonal loans.



Compute perquisite interest rates.



Dependencies: May reference organisation/unit data from an external HR service.



3.2 LoanApplicationService

Purpose: Processes loan applications, approvals, and rejections.



Owned Tables: LOAN\_APPLICATION, LOAN\_ADDITIONAL.



Key Operations:



Submit new application.



Approve/reject with remarks.



Fetch applications by employee.



Validate eligibility by calling external services (employee, grade, entitlements).



Dependencies: Employee data, loan product definitions.



3.3 LoanAccountService

Purpose: Manages the lifecycle of active loans – disbursement, instalment scheduling, repayments, adjustments, and closures.



Owned Tables: LOAN\_MAIN, LOAN\_EMPINTRATEMAST, LOAN\_INS, LOAN\_LEDGER, LOAN\_SET, LOAN\_ADJUSTMENT, LOAN\_IUTAJV, TEMPLOAN\_IUTAJV.



Key Operations:



Create loan account upon approval.



Generate instalment schedule.



Record repayments and adjustments.



Calculate outstanding balances.



Handle loan settlements and write‑offs.



Process inter‑unit transfer adjustments.



Dependencies: Loan definitions, employee data, payroll unit details.



3.4 DocumentService

Purpose: Stores and retrieves loan‑related documents.



Owned Tables: LOAN\_DOCUMENTS.



Key Operations:



Upload document (store file in blob storage, metadata in DB).



Download document.



List documents for a loan.



Dependencies: Loan identifiers, employee identifiers.



3.5 LookupService

Purpose: Provides a centralised repository for all list‑of‑values (LOVs).



Owned Tables: LOV\_MASTER, LOV\_TYPEMAST, PROGRAMLOV\_MAST.



Key Operations:



Get all values for a given LOV type.



Maintain LOVs (admin only).



Dependencies: None.



4\. Data Ownership and Integrity

Each microservice owns its database schema and tables. No two services share the same table. Cross‑service references are maintained via logical foreign keys (IDs) without database constraints. Data consistency is ensured through:



Synchronous API calls for read operations.



Asynchronous events (e.g., using a message broker) for state changes that affect multiple services.



For example, when a loan application is approved in LoanApplicationService, it publishes an event LoanApproved. LoanAccountService subscribes and creates the loan account.



5\. Service Contracts (APIs)

Each microservice exposes a RESTful API. Below are illustrative endpoints:



LoanDefinitionService

GET /api/loandefinitions – list loan products.



GET /api/loandefinitions/{id} – get loan product details.



POST /api/loandefinitions – create loan product.



GET /api/interestrates?loanId={loanId} – get interest rate history.



GET /api/festivals – list festivals.



LoanApplicationService

POST /api/applications – submit application.



GET /api/applications/{id} – get application status.



PUT /api/applications/{id}/approve – approve application.



PUT /api/applications/{id}/reject – reject application.



GET /api/applications?employeeId={empId} – list employee applications.



LoanAccountService

GET /api/loans/{loanNo} – get loan account details.



GET /api/loans/{loanNo}/installments – list instalments.



POST /api/loans/{loanNo}/repayments – record a repayment.



POST /api/loans/{loanNo}/adjustments – record an adjustment.



POST /api/loans/{loanNo}/settlements – settle loan.



DocumentService

POST /api/documents – upload document (multipart/form-data).



GET /api/documents/{docId} – download document.



GET /api/documents?loanId={loanId} – list document metadata.



LookupService

GET /api/lookups/{typeCode} – get all values for a given LOV type.



GET /api/lookups/{typeCode}/{code} – get specific LOV value.



6\. Data Models (C#)

Below are example C# entity classes for each microservice (using EF Core). Only key properties are shown.



LoanDefinitionService – LoanMaster

csharp

public class LoanMaster

{

&nbsp;   public long LoanId { get; set; }

&nbsp;   public string LoanName { get; set; }

&nbsp;   public string Purpose { get; set; }

&nbsp;   public long OrgId { get; set; }

&nbsp;   public long UnitId { get; set; }

&nbsp;   public long TypeId { get; set; }

&nbsp;   public long MinimumLimit { get; set; }

&nbsp;   public long MaximumLimit { get; set; }

&nbsp;   public bool IsRecoverable { get; set; }

&nbsp;   public DateTime EffectiveDate { get; set; }

&nbsp;   public DateTime? ClosureDate { get; set; }

&nbsp;   // ... other fields

&nbsp;   public ICollection<LoanInterestRateMaster> InterestRates { get; set; }

}

LoanApplicationService – LoanApplication

csharp

public class LoanApplication

{

&nbsp;   public long LoanAppId { get; set; }

&nbsp;   public long EmployeeSysId { get; set; }

&nbsp;   public long LoanId { get; set; }

&nbsp;   public long AppliedBy { get; set; }

&nbsp;   public DateTime AppliedOn { get; set; }

&nbsp;   public string Source { get; set; } // DIR/SLF

&nbsp;   public long Amount { get; set; }

&nbsp;   public long? SubclassId { get; set; }

&nbsp;   public string Reason { get; set; }

&nbsp;   public string Status { get; set; } // P, A, R, C

&nbsp;   public long Guarantor { get; set; }

&nbsp;   public long? ApprovedBy { get; set; }

&nbsp;   public DateTime? ApprovedOn { get; set; }

&nbsp;   // ... other fields

}

LoanAccountService – LoanMain

csharp

public class LoanMain

{

&nbsp;   public long LoanNo { get; set; }

&nbsp;   public long LoanAppId { get; set; }

&nbsp;   public long EmployeeSysId { get; set; }

&nbsp;   public long LoanId { get; set; }

&nbsp;   public string DisbursementType { get; set; } // NEW/ADJ

&nbsp;   public decimal PrincipalAmount { get; set; }

&nbsp;   public decimal PrincipalOutstanding { get; set; }

&nbsp;   public DateTime FirstInstallmentDate { get; set; }

&nbsp;   public DateTime LoanDate { get; set; }

&nbsp;   public DateTime? ClosureDate { get; set; }

&nbsp;   public long UnitId { get; set; }

&nbsp;   public string ClosureType { get; set; } // SET/WOF/ADJ/LIV

&nbsp;   public long? NewLoanNo { get; set; }

&nbsp;   // ... other fields

&nbsp;   public ICollection<LoanInstallment> Installments { get; set; }

}

DocumentService – LoanDocument

csharp

public class LoanDocument

{

&nbsp;   public long DocumentId { get; set; }

&nbsp;   public long LoanId { get; set; }

&nbsp;   public long DocumentTypeId { get; set; }

&nbsp;   public string FileName { get; set; }        // not in original table, added

&nbsp;   public string BlobUri { get; set; }          // not in original table, added

&nbsp;   public long LastModifiedBy { get; set; }

&nbsp;   public DateTime LastModifiedOn { get; set; }

}

LookupService – LovMaster

csharp

public class LovMaster

{

&nbsp;   public long LovId { get; set; }

&nbsp;   public int LovTypeId { get; set; }

&nbsp;   public string LovName { get; set; }

&nbsp;   public DateTime CreatedOn { get; set; }

&nbsp;   public long CreatedBy { get; set; }

&nbsp;   public DateTime UpdatedOn { get; set; }

&nbsp;   public long UpdatedBy { get; set; }

}

7\. Inter‑service Communication

Synchronous: For queries that need real‑time data (e.g., loan application validating loan product), services call each other’s REST endpoints.



Asynchronous: For state changes that trigger actions in other services, we use a message broker (e.g., Azure Service Bus, RabbitMQ). Events are published with a consistent schema.



Example events:



LoanApplicationApproved – published by LoanApplicationService; consumed by LoanAccountService to create the loan.



LoanClosed – published by LoanAccountService; consumed by LoanApplicationService to update any related application status (if needed).



DocumentUploaded – published by DocumentService; may be used for notifications.



8\. Technical Stack

Language: C# 10+



Framework: .NET 6/8



API: ASP.NET Core, RESTful, OpenAPI (Swagger)



Data Access: Entity Framework Core



Database: SQL Server (each service has its own database)



Messaging: Azure Service Bus / RabbitMQ



Containerisation: Docker, Kubernetes for orchestration



API Gateway: Ocelot / YARP for routing and composition



9\. Migration Strategy

Extract each microservice’s tables into separate databases.



Implement data sync for existing data (ETL) and then switch to event‑driven consistency.



Use the strangler pattern: new features use microservices; legacy monolith gradually retired.



10\. Conclusion

The proposed microservices architecture decomposes the loan management system into cohesive, loosely coupled services. Each service aligns with a clear business capability, enabling independent development, scaling, and deployment. The design leverages C# and modern .NET practices to ensure maintainability and performance.

