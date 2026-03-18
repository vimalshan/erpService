using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSA.Service.Infrastructure.Data;

public class CsaDbContext : DbContext, IUnitOfWork
{
    public CsaDbContext(DbContextOptions<CsaDbContext> options) : base(options) { }

    public DbSet<Control> Controls => Set<Control>();
    public DbSet<Evidence> Evidences => Set<Evidence>();
    public DbSet<Process> Processes => Set<Process>();
    public DbSet<SubProcess> SubProcesses => Set<SubProcess>();
    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<SurveyQuestion> SurveyQuestions => Set<SurveyQuestion>();
    public DbSet<SurveyFeedback> SurveyFeedbacks => Set<SurveyFeedback>();
    public DbSet<SurveyAttachment> SurveyAttachments => Set<SurveyAttachment>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<UnitMapDetail> UnitMapDetails => Set<UnitMapDetail>();
    public DbSet<CsaUser> CsaUsers => Set<CsaUser>();
    public DbSet<CsaMainUpload> CsaMainUploads => Set<CsaMainUpload>();
    public DbSet<CsaMainUploadErr> CsaMainUploadErrors => Set<CsaMainUploadErr>();
    public DbSet<CsaData> CsaData => Set<CsaData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CsaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
