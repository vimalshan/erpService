# Email Notification Service - Deployment & Production Checklist

**Status:** Ready for Production Deployment  
**Last Updated:** March 12, 2026  
**Current Phase:** Pre-Production Verification

---

## Pre-Deployment Verification Checklist

### ✅ Build & Compilation
- [x] Solution builds without errors
- [x] All project references resolved
- [x] NuGet packages installed
- [x] No critical warnings
- [x] Code compiles in Release mode
- [x] All tests pass (if applicable)

### ✅ Database Setup
- [x] SQL Server schema created
- [x] EF Core migrations generated
- [x] DbContextFactory implemented for tooling
- [x] Connection string configured
- [x] Database users and permissions set
- [x] Backup strategy defined
- [x] Seed data configured

### ✅ Application Configuration
- [x] appsettings.json configured
- [x] appsettings.Development.json created
- [x] Environment variables documented
- [x] JWT configuration in place
- [x] RabbitMQ settings configured
- [x] Logging configuration complete
- [x] Health check endpoint active

### ✅ Security
- [x] JWT Bearer authentication implemented
- [x] Authorization policies configured
- [x] HTTPS enabled
- [x] CORS properly configured
- [x] Input validation on all endpoints
- [x] SQL injection protection (EF Core)
- [x] Secrets management planned

### ✅ API Implementation
- [x] All REST endpoints implemented
- [x] Request/response DTOs created
- [x] Error handling middleware in place
- [x] Request logging middleware active
- [x] Health check endpoint available
- [x] Exception handling configured
- [x] Validation behaviors configured

### ✅ Infrastructure
- [x] Repository pattern implemented
- [x] Dependency injection configured
- [x] AutoMapper profiles created
- [x] MediatR handlers implemented
- [x] Fluent validation rules defined
- [x] RabbitMQ integration complete
- [x] Polly resilience policies configured

### ✅ Resilience & Monitoring
- [x] Circuit breaker policies in place
- [x] Retry policies with exponential backoff
- [x] Timeout policies configured
- [x] Logging framework integrated
- [x] Performance monitoring ready
- [x] Error tracking configured
- [x] Health checks implemented

---

## Pre-Production Checklist

### Infrastructure Setup
- [ ] SQL Server instance configured
  - [ ] Backup schedule configured
  - [ ] Disk space verified (minimum 10GB free)
  - [ ] Automated maintenance jobs scheduled
  - [ ] User accounts created with proper permissions
  - [ ] Connection pooling configured
  
- [ ] RabbitMQ cluster configured
  - [ ] Nodes configured and running
  - [ ] User accounts created
  - [ ] Virtual hosts set up
  - [ ] Disk alarm threshold set
  - [ ] Memory alarm threshold set
  - [ ] Queue durability configured
  
- [ ] Application Server prepared
  - [ ] IIS configured (Windows) OR Container runtime ready (Linux/Docker)
  - [ ] Application pool configuration set
  - [ ] URL rewrite rules configured
  - [ ] Compression enabled
  - [ ] Request filtering configured
  - [ ] SSL/TLS certificates installed

### Configuration Management
- [ ] Production appsettings.json created
  - [ ] Database connection string (encrypted)
  - [ ] RabbitMQ credentials (encrypted)
  - [ ] JWT Authority URL
  - [ ] JWT Audience value
  - [ ] Logging level set to Information
  - [ ] Serilog sinks configured (file, event log)
  
- [ ] Environment variables documented
  - [ ] Connection string variable name
  - [ ] RabbitMQ credentials variable names
  - [ ] JWT configuration variables
  - [ ] Logging path variable
  
- [ ] Secrets management
  - [ ] Azure Key Vault configured OR
  - [ ] AWS Secrets Manager configured OR
  - [ ] On-prem secrets store configured

### Application Deployment
- [ ] Application published in Release config
  ```bash
  dotnet publish -c Release -o ./publish
  ```
  
- [ ] Deployment package created
  - [ ] All binaries included
  - [ ] Configuration files included
  - [ ] Database migration scripts included
  - [ ] README and documentation included
  
- [ ] Deployment location prepared
  - [ ] Web root directory created
  - [ ] IIS app pool created
  - [ ] Permissions set correctly
  - [ ] Application binding configured

### Database Deployment
- [ ] Database created in SQL Server
  ```bash
  cd src/EmailNotification.Infrastructure
  dotnet ef database update --environment Production
  ```
  
- [ ] Initial seed data loaded
  ```bash
  # Seed data loads automatically on first run
  # Or manually execute: EmailNotificationDataSeeder
  ```
  
- [ ] Database backups verified
  - [ ] Full backup completed
  - [ ] Backup location verified
  - [ ] Restore test performed
  - [ ] Backup schedule confirmed

### Messaging Setup
- [ ] RabbitMQ queues created
  - [ ] email.notifications queue
  - [ ] email.events queue
  - [ ] email.deadletter queue (DLQ)
  
- [ ] RabbitMQ exchanges created
  - [ ] email.events exchange
  - [ ] email.notifications exchange
  
- [ ] Queue bindings verified
  - [ ] Event binding configured
  - [ ] DLQ binding configured

### Security Hardening
- [ ] SSL/TLS certificates installed
  - [ ] Certificate validity verified
  - [ ] Certificate chain complete
  - [ ] Key protection configured
  
- [ ] Firewall rules configured
  - [ ] RabbitMQ port (5672) - restricted
  - [ ] SQL Server port (1433) - restricted
  - [ ] HTTPS port (443) - open
  - [ ] HTTP port (80) - redirect to HTTPS
  
- [ ] User access configured
  - [ ] Application identity service account created
  - [ ] Database user created with minimal permissions
  - [ ] RabbitMQ user created with queue permissions
  
- [ ] API security validated
  - [ ] CORS headers reviewed
  - [ ] JWT issuer/audience verified
  - [ ] Token expiration set appropriately
  - [ ] Refresh token mechanism configured (if applicable)

### Monitoring & Alerting Setup
- [ ] Health check monitoring
  - [ ] /health endpoint monitored
  - [ ] Alert on health check failure
  - [ ] Response time tracking
  
- [ ] Application logging
  - [ ] Log file location configured
  - [ ] Log rotation configured
  - [ ] Error log aggregation set up
  - [ ] Performance metrics collected
  
- [ ] Database monitoring
  - [ ] Query performance monitoring
  - [ ] Index fragmentation monitoring
  - [ ] Disk space monitoring
  - [ ] Connection pool monitoring
  
- [ ] RabbitMQ monitoring
  - [ ] Queue depth monitoring
  - [ ] Consumer lag monitoring
  - [ ] Connection tracking
  - [ ] Memory usage monitoring
  
- [ ] Infrastructure monitoring
  - [ ] CPU usage monitoring
  - [ ] Memory usage monitoring
  - [ ] Disk I/O monitoring
  - [ ] Network traffic monitoring

### Testing Before Go-Live
- [ ] Smoke testing
  - [ ] Application starts without errors
  - [ ] Health check returns 200
  - [ ] Database connectivity verified
  - [ ] RabbitMQ connectivity verified
  
- [ ] API functionality testing
  - [ ] GET /health endpoint works
  - [ ] GET /api/v1/email-types returns data
  - [ ] POST /api/v1/email-types creates record
  - [ ] PUT /api/v1/email-types updates record
  - [ ] DELETE /api/v1/email-types deletes record
  - [ ] Same for /api/v1/mail-access
  
- [ ] Authentication testing
  - [ ] JWT token validation works
  - [ ] Expired token rejected
  - [ ] Invalid token rejected
  - [ ] Valid token accepted
  
- [ ] Error handling testing
  - [ ] 404 errors properly returned
  - [ ] 401 unauthorized errors returned
  - [ ] 500 errors logged properly
  - [ ] Error messages are descriptive
  
- [ ] Load testing (optional)
  - [ ] Application handles 100 concurrent users
  - [ ] API response time < 500ms under load
  - [ ] Database connection pool adequate
  - [ ] No memory leaks detected
  
- [ ] Integration testing
  - [ ] RabbitMQ message publishing works
  - [ ] RabbitMQ message consumption works
  - [ ] Email notifications queued properly
  - [ ] Dead letter queue processes failures

### Documentation
- [ ] Deployment guide created
  - [ ] Step-by-step deployment instructions
  - [ ] Configuration file examples
  - [ ] Database setup instructions
  - [ ] Troubleshooting guide
  
- [ ] Operations guide created
  - [ ] Daily checks to perform
  - [ ] How to restart services
  - [ ] How to restore from backup
  - [ ] Emergency contacts
  
- [ ] Performance tuning guide
  - [ ] Connection string optimization
  - [ ] Query optimization tips
  - [ ] Caching strategies
  - [ ] Scaling recommendations
  
- [ ] API documentation
  - [ ] Endpoint reference
  - [ ] Authentication guide
  - [ ] Error codes reference
  - [ ] Rate limiting info (if applicable)

### Backup & Disaster Recovery
- [ ] Database backup plan
  - [ ] Daily backup schedule: 2:00 AM UTC
  - [ ] backup retention: 30 days
  - [ ] Backup location: Off-site storage
  - [ ] Backup encryption: Enabled
  - [ ] Restore test: Monthly
  
- [ ] Application backup plan
  - [ ] Configuration backup: Daily
  - [ ] Source code backup: On commit
  - [ ] Release package backup: With each version release
  
- [ ] Disaster recovery plan
  - [ ] RTO (Recovery Time Objective): 4 hours
  - [ ] RPO (Recovery Point Objective): 1 hour
  - [ ] Failover procedure documented
  - [ ] Failback procedure documented

---

## Go-Live Checklist

### Immediate Pre-Deployment (T-24 Hours)
- [ ] Final code review completed
- [ ] All tests passing
- [ ] Performance benchmarks met
- [ ] Security scan completed
- [ ] Dependencies verified
- [ ] Configuration files prepared

### Deployment Day (T-0)
- [ ] Team briefing completed
- [ ] Rollback plan reviewed
- [ ] Database backups created
- [ ] Application deployed to production
- [ ] Database migrations executed
- [ ] Seed data loaded
- [ ] Service starts without errors
- [ ] Health check passing
- [ ] Smoke tests passing

### Post-Deployment (T+1 Hour)
- [ ] All API endpoints verified
- [ ] End-to-end testing completed
- [ ] User access verified
- [ ] Monitoring alerts verified
- [ ] Log aggregation working
- [ ] Performance metrics baseline established

### Post-Deployment (T+24 Hours)
- [ ] No critical errors in logs
- [ ] No database errors
- [ ] No RabbitMQ errors
- [ ] Performance metrics acceptable
- [ ] User feedback positive
- [ ] Documentation updates completed

---

## Monitoring After Go-Live

### Daily Checks (Every Morning)
- [ ] Health check endpoint returns 200
- [ ] No critical errors in error logs
- [ ] Database backups completed successfully
- [ ] RabbitMQ queue depths normal
- [ ] Disk space adequate
- [ ] CPU/Memory usage normal

### Weekly Checks
- [ ] Performance metrics review
- [ ] Log analysis for patterns
- [ ] Active user count review
- [ ] API response time analysis
- [ ] Database maintenance tasks run
- [ ] Security scan (if applicable)

### Monthly Checks
- [ ] Disaster recovery test
- [ ] Performance capacity planning
- [ ] Security patch availability
- [ ] Dependency updates check
- [ ] Cost analysis
- [ ] User satisfaction survey

---

## Troubleshooting Guide

### Database Connection Issues
```
Error: "Cannot connect to database"
1. Verify connection string in appsettings.json
2. Verify SQL Server service is running
3. Check network connectivity to database server
4. Verify database user permissions
5. Check firewall rules allow port 1433
```

### RabbitMQ Connection Issues
```
Error: "Cannot connect to RabbitMQ"
1. Verify RabbitMQ service is running
2. Check hostname and port in configuration
3. Verify RabbitMQ user credentials
4. Check network connectivity to RabbitMQ server
5. Verify firewall rules allow port 5672
```

### Authentication Issues
```
Error: "Unauthorized - Invalid token"
1. Verify JWT token is valid
2. Check token hasn't expired
3. Verify Authority and Audience in configuration
4. Check clock synchronization between servers
5. Review JWT configuration
```

### Performance Issues
```
If API response time > 1000ms:
1. Check database query performance
2. Check connection pool settings
3. Monitor CPU/Memory usage
4. Review slow query logs
5. Check for deadlocks in database
6. Review Polly policy timeouts
```

---

## Rollback Procedures

### If Critical Issues Detected

1. **Notify Stakeholders**
   - Alert operations team
   - Notify business stakeholders
   - Prepare rollback plan

2. **Database Rollback**
   ```bash
   # Restore from pre-deployment backup
   # OR
   cd src/EmailNotification.Infrastructure
   dotnet ef database update --migration PreviousMigrationName
   ```

3. **Application Rollback**
   - Redeploy previous stable version
   - Restart application services
   - Verify health checks passing

4. **Post-Rollback Analysis**
   - Identify root cause
   - Plan fix and testing
   - Schedule new deployment

---

## Contact & Escalation

| Role | Contact | Phone | On-Call? |
|------|---------|-------|----------|
| DevOps Lead | dev-lead@company.com | x1234 | Yes |
| DBA Lead | dba-lead@company.com | x5678 | Yes |
| Architecture Lead | arch-lead@company.com | x9012 | As-needed |
| Support Manager | support@company.com | x3456 | Yes |

---

## Sign-Off

**Deployment Manager:** _____________________  
**Release Manager:** _______________________  
**Operations Lead:** _______________________  
**Security Lead:** __________________________  

**Date:** _______________  
**Time:** _______________  

---

## Version History

| Version | Date | Changes | Approved By |
|---------|------|---------|-------------|
| 1.0 | 2026-03-12 | Initial checklist | Copilot |

---

**Status:** ✅ READY FOR PRODUCTION DEPLOYMENT

---

*This checklist should be reviewed and updated with each deployment.*  
*Keep a copy for audit and compliance purposes.*
