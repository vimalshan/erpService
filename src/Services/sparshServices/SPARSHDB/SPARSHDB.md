SCIDB Microservices Technical Design Document

SCIDB.sql

SQL 82.02KB

this document i want to create modulus in technical document in microservices in c#

We need to interpret the request: "this document i want to create modulus in technical document in microservices in c#". Probably the user wants to convert this SQL schema into a technical document that describes the microservices architecture in C#. Possibly they want to generate entity classes, or perhaps a design document. The input is a large SQL file with many tables. The user might want to create a technical document that outlines how to build microservices based on this schema. Possibly they want to generate C# entity models and maybe a breakdown into microservices bounded contexts.



Given the ambiguity, I need to produce a structured response: likely a technical design document that groups related tables into microservices, defines aggregate roots, and provides C# code examples. Could also include recommendations for database per service, communication, etc.



Since the user said "create modulus in technical document in microservices in c#", maybe they want to create modules (microservices) from this database schema. I should analyze the tables and propose microservice boundaries.



The database is named SCIDB, and it appears to be an enterprise system for supply chain, vehicle tracking, exim (export-import), GST, dispatch planning, filling lines, etc. It's a complex schema. I need to group tables by domain.



Let me scan table names:



ACCESS\_ROLE, ACCESS\_ROLE\_MASTER, ACCESSROLE\_MENU, USER\_MASTER, USER\_ROLE, etc. => Security/Authentication service.



ACTUAL\_ORDER\_MAP, OS\_ACTUAL\_ORDER, OS\_ACTUAL\_ORDER\_TIED, OS\_TIED\_ORDER\_DETAILS, OS\_TIED\_ORDER\_HEADER, etc. => Order management.



ADVLIC\_ENTITLEMENT, ADVLIC\_MASTER => Advance License? Maybe compliance.



AUTO\_MAIL\_STATUS, AUTO\_MAILID => Notification service.



COMPANY\_UNITMASTER => Organization structure.



DECISION\_FLAG, LOG\_DECISION\_FLAG, VEHICLE\_STAGE, VEHICLE\_TRAN, etc. => Vehicle tracking / logistics.



DISPATCH\_PLAN\_... => Dispatch planning.



EXIM\_DATA\*, EXIM\_PRODUCT, EXIM\_SEARCH, etc. => Export-Import data service.



FILLING\_LINE, FILLING\_PLANT, FILLING\_POINT\_GROUP, FILLING\_CAPACITY, etc. => Production/Filling management.



GST\_\* => GST compliance.



ITEM\_MASTER, MAIN\_PRODUCT\_MASTER, PRODUCT\_MASTER, UNIT\_OF\_MEASURE => Product catalog.



MAM\_\* => Material allocation management.



NORMS\_MASTER => Production norms.



PURCHASE\_DETAILS, SALE\_MAIN, SALE\_SUB => Procurement and sales.



SUPPLIER\_MASTER, CUSTOMER? => Party management.



STRATEGIC\_STOCK => Inventory.



Given this, I can propose microservices like:



Identity and Access Management (IAM) - USER\_MASTER, ACCESS\_ROLE, etc.



Party Management - SUPPLIER\_MASTER, maybe CUSTOMER from GST tables.



Product Catalog - ITEM\_MASTER, MAIN\_PRODUCT\_MASTER, UNIT\_OF\_MEASURE, etc.



Order Management - ACTUAL\_ORDER, TIED\_ORDER, etc.



Dispatch Planning - DISPATCH\_PLAN tables.



Production Planning - FILLING\_LINE, PRODUCTION\_PLAN, etc.



Inventory/Stock - STRATEGIC\_STOCK, MAM tables.



Exim (Export-Import) - EXIM\_DATA, EXIM\_PRODUCT, etc.



GST Compliance - GST\_MAIN, GST\_STATEREGDET, etc.



Vehicle Tracking - VEHICLE\_MAST, VEHICLE\_STAGE, VEHICLE\_TRAN, etc.



Notification - AUTO\_MAIL\_STATUS, etc.



Audit/Logging - LOG\_\* tables.



Each microservice could own its tables and expose APIs. Communication via REST or messaging.



Also, need to consider shared data like COMPANY\_UNITMASTER might be used by many services; could be a separate Organization service.



Now, for the technical document, I need to provide a structured document: introduction, domain decomposition, service boundaries, data ownership, API contracts, C# project structure, entity classes (maybe examples), and communication patterns.



Given the request, I'll produce a comprehensive technical design document.



I'll start with an overview, then list microservices with their tables, then for each service, describe the domain, key entities, and sample C# classes. I'll also include recommendations for database per service, inter-service communication, and technology stack (e.g., .NET Core, Entity Framework Core, REST, maybe gRPC).



I'll also mention that the existing schema is relational, but for microservices we need to decouple, so each service should have its own database, and relationships are handled via API calls or events.



Let's craft the response.



Technical Design Document: Microservices Architecture for SCIDB

1\. Introduction

This document outlines a proposed microservices-based architecture for the SCIDB system, derived from its existing SQL Server database schema. The goal is to decompose the monolithic database into domain-aligned services, each owning its data and exposing business capabilities via well-defined APIs. The implementation will be in C# using .NET technologies.



The original schema contains over 100 tables covering diverse business domains: user management, vehicle tracking, order management, production planning, export-import, GST compliance, dispatch planning, inventory, and more. By grouping related tables, we define cohesive microservices that can be developed, deployed, and scaled independently.



2\. Guiding Principles

Domain-Driven Design (DDD): Each microservice corresponds to a bounded context.



Database per Service: Each service manages its own database; no direct cross-service queries.



API-First Communication: Services interact via REST (or gRPC) and asynchronous events.



Polyglot Persistence: Services may use relational (SQL Server) or NoSQL stores as appropriate.



Eventual Consistency: Cross-service transactions use sagas or eventual consistency patterns.



3\. Proposed Microservices

Based on the table groupings, the following microservices are identified:



Microservice	Bounded Context	Key Tables (Partial List)

Identity \& Access	User authentication, roles, permissions	USER\_MASTER, ACCESS\_ROLE, ACCESS\_ROLE\_MASTER, ACCESSROLE\_MENU, USER\_ROLE, ROLE\_MAST

Organization	Company units, locations	COMPANY\_UNITMASTER, LOCATION\_MASTER

Party Management	Suppliers, customers	SUPPLIER\_MASTER, GST\_SUPPLIER, GST\_CUSTOMER, GST\_ORACUST

Product Catalog	Products, items, units, packaging	ITEM\_MASTER, MAIN\_PRODUCT\_MASTER, PRODUCT\_MASTER, UNIT\_OF\_MEASURE, PACKAGE\_TYPE, ITEM\_CAPACITY, GRADE\_MASTER

Order Management	Customer orders, tied orders, actual orders	OS\_ACTUAL\_ORDER, OS\_TIED\_ORDER\_HEADER, OS\_TIED\_ORDER\_DETAILS, ACTUAL\_ORDER\_MAP

Dispatch Planning	Dispatch plans, main groups, subgroups	DISPATCH\_PLAN\_HEADER, DISPATCH\_PLAN\_MAINGROUP, DISPATCH\_PLAN\_SUBGROUP, DISPATCH\_PLAN\_ITEMWISE

Production	Filling lines, plants, capacities	FILLING\_LINE, FILLING\_PLANT, FILLING\_POINT\_GROUP, PRODUCTION\_PLAN, PRODUCTION\_PLANT, ITEM\_CAPACITY

Inventory	Stock levels, strategic stock, allocations	STRATEGIC\_STOCK, MAM\_ALLOCATION\_DET, MAM\_CONSUMPTION\_DET, MAM\_PRODUCTION\_DET

Vehicle Tracking	Vehicle entry/exit, stages, weight info	VEHICLE\_MAST, VEHICLE\_TRAN, VEHICLE\_STAGE, WEIGHT\_INFO, DECISION\_FLAG

Export-Import (EXIM)	EXIM data, product search, file uploads	EXIM\_DATA, EXIM\_PRODUCT, EXIM\_SEARCH, EXIM\_DATAFILE, EXIM\_PRODUCTGROUP

GST Compliance	GST registration, HSN/SAC codes, supplier GST	GST\_MAIN, GST\_HSNDET, GST\_SERVDET, GST\_STATEREGDET, GST\_ORAMAIN

Notification	Email notifications, auto mail status	AUTO\_MAIL\_STATUS, AUTO\_MAILID

Audit \& Logging	Deletion logs, change history	LOG\_\* tables, DELLOG\_\* tables

Note: Some tables like PURPOSE\_MAST, STAGE\_MAST, PURPOSE\_STAGE appear to be workflow-related and may be part of a Workflow Engine service. They are currently grouped with Vehicle Tracking, but could be extracted if workflows span multiple domains.



4\. Data Ownership and Boundaries

Each microservice owns its tables and is the sole source of truth for that data. Cross-service references are stored as foreign keys only in concept—no actual FK constraints across databases. Instead, services store the identifier of an entity from another service (e.g., CompanyUnitId in Product service refers to a record in Organization service).



5\. Technology Stack

Framework: .NET 8 (or latest LTS)



API: ASP.NET Core Web API (REST/JSON)



Database: SQL Server (or optionally PostgreSQL) – one database per service



ORM: Entity Framework Core (code-first or database-first)



Containerization: Docker, Kubernetes for orchestration



Service Discovery: Consul or Kubernetes DNS



API Gateway: Ocelot or YARP



Messaging: RabbitMQ / Azure Service Bus for events



Authentication: JWT tokens, IdentityServer (or Azure AD)



6\. Example: Product Catalog Microservice

6.1 Domain Model

Key entities (C# classes) based on tables:



csharp

public class Product

{

&nbsp;   public int ProductId { get; set; }           // MAIN\_PRODUCT\_MASTER.PRODUCT\_ID

&nbsp;   public string ProductName { get; set; }

&nbsp;   public string Description { get; set; }

&nbsp;   public int UnitId { get; set; }              // References Unit

&nbsp;   public int ProductTypeId { get; set; }        // References ProductType

&nbsp;   public int CompanyUnitId { get; set; }        // References Organization service

&nbsp;   public DateTime? ModifiedDate { get; set; }

&nbsp;   // Navigation properties (for EF within service)

&nbsp;   public Unit Unit { get; set; }

&nbsp;   public ProductType ProductType { get; set; }

}



public class Item

{

&nbsp;   public int SciItemId { get; set; }           // ITEM\_MASTER.SCI\_ITEM\_ID

&nbsp;   public string OracleCode { get; set; }

&nbsp;   public int? MainProductId { get; set; }       // References Product

&nbsp;   public string ItemName { get; set; }

&nbsp;   public string ItemType { get; set; }

&nbsp;   public int? PackageTypeId { get; set; }       // References PackageType

&nbsp;   public int ItemUomId { get; set; }            // References Unit

&nbsp;   public decimal MainProductUomConFactor { get; set; }

&nbsp;   // ...

&nbsp;   public Product Product { get; set; }

&nbsp;   public Unit Unit { get; set; }

}



public class Unit

{

&nbsp;   public int UnitId { get; set; }               // UNIT\_OF\_MEASURE.UNIT\_ID

&nbsp;   public string UnitCode { get; set; }

&nbsp;   public string UnitOfMeasure { get; set; }

&nbsp;   public int UnitClassId { get; set; }

&nbsp;   public bool IsBaseUnit { get; set; }

}

6.2 API Endpoints

GET /api/products – list products



GET /api/products/{id} – get product details



POST /api/products – create product



PUT /api/products/{id} – update product



GET /api/items – list items (with filtering by product, etc.)



GET /api/units – get units of measure



6.3 Database Context

csharp

public class ProductCatalogDbContext : DbContext

{

&nbsp;   public DbSet<Product> Products { get; set; }

&nbsp;   public DbSet<Item> Items { get; set; }

&nbsp;   public DbSet<Unit> Units { get; set; }

&nbsp;   public DbSet<ProductType> ProductTypes { get; set; }

&nbsp;   // ... other entities

}

7\. Inter-Service Communication

7.1 Synchronous (REST)

Services call each other's APIs to retrieve reference data.



Example: Order service needs product details; it calls GET /api/products/{id} in Product service.



7.2 Asynchronous (Events)

Use message broker to publish domain events (e.g., ProductCreated, OrderShipped).



Other services subscribe to relevant events to maintain eventual consistency.



Example: When a dispatch plan is created, an event triggers Inventory service to reserve stock.



8\. Data Consistency Patterns

Saga Pattern for multi-service transactions (e.g., order placement across Order, Inventory, and Production).



Compensating Transactions to revert changes if a step fails.



9\. Deployment Architecture

Each microservice is containerized and deployed in Kubernetes.



API Gateway routes requests to appropriate services.



Databases are deployed as stateful sets or managed cloud databases.



10\. Security

All APIs secured with JWT tokens issued by Identity \& Access service.



Role-based access control (RBAC) based on ACCESS\_ROLE tables.



API Gateway handles authentication and forwards user context.



11\. Migration Strategy

Start with one bounded context (e.g., Product Catalog) and extract it into a new service.



Use database views or temporary dual-writes during migration.



Gradually move other domains.



12\. Conclusion

This microservices design decomposes the monolithic SCIDB into manageable, scalable, and independently deployable services. Each service is focused on a specific business capability, enabling faster development, easier maintenance, and better alignment with business domains. The next step is to prioritize and begin extraction of the first service.



This document serves as a blueprint for the technical team to implement the microservices architecture in C#. Detailed API contracts, event schemas, and database scripts will be developed per service.

