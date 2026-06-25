using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Data;

public class SupervisorioDbContext(DbContextOptions<SupervisorioDbContext> options) : DbContext(options)
{
    public DbSet<MachineReading> MachineReadings => Set<MachineReading>();
    public DbSet<IndustrialTag> IndustrialTags => Set<IndustrialTag>();
    public DbSet<TagValueHistory> TagValueHistory => Set<TagValueHistory>();
    public DbSet<CommandExecution> CommandExecutions => Set<CommandExecution>();
    public DbSet<DiagnosticNotification> DiagnosticNotifications => Set<DiagnosticNotification>();
    public DbSet<MaintenanceHistory> MaintenanceHistory => Set<MaintenanceHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndustrialTag>(entity =>
        {
            entity.HasKey(tag => tag.Id);
            entity.HasIndex(tag => tag.Name).IsUnique();
            entity.Property(tag => tag.Name).HasMaxLength(160).IsRequired();
            entity.Property(tag => tag.DisplayName).HasMaxLength(160).IsRequired();
            entity.Property(tag => tag.Category).HasMaxLength(80).IsRequired();
            entity.Property(tag => tag.DataType).HasMaxLength(40).IsRequired();
            entity.Property(tag => tag.Unit).HasMaxLength(30);
            entity.Property(tag => tag.Description).HasMaxLength(300);
            entity.Property(tag => tag.CurrentValue).HasMaxLength(250).IsRequired();
        });

        modelBuilder.Entity<TagValueHistory>(entity =>
        {
            entity.HasKey(history => history.Id);
            entity.Property(history => history.TagName).HasMaxLength(160).IsRequired();
            entity.Property(history => history.Value).HasMaxLength(250).IsRequired();
            entity.Property(history => history.Source).HasMaxLength(40).IsRequired();
            entity.Property(history => history.MqttTopic).HasMaxLength(250);
            entity.HasIndex(history => new { history.TagName, history.CreatedAt });
        });

        modelBuilder.Entity<CommandExecution>(entity =>
        {
            entity.HasKey(command => command.Id);
            entity.Property(command => command.CommandTag).HasMaxLength(160).IsRequired();
            entity.Property(command => command.Value).HasMaxLength(250).IsRequired();
            entity.Property(command => command.RequestedBy).HasMaxLength(80).IsRequired();
            entity.Property(command => command.Status).HasMaxLength(40).IsRequired();
            entity.HasIndex(command => command.CreatedAt);
        });

        modelBuilder.Entity<DiagnosticNotification>(entity =>
        {
            entity.HasKey(notification => notification.Id);
            entity.Property(notification => notification.Code).HasMaxLength(120).IsRequired();
            entity.Property(notification => notification.Severity).HasMaxLength(40).IsRequired();
            entity.Property(notification => notification.Origin).HasMaxLength(80).IsRequired();
            entity.Property(notification => notification.Title).HasMaxLength(180).IsRequired();
            entity.Property(notification => notification.Description).HasMaxLength(1000).IsRequired();
            entity.Property(notification => notification.PossibleCauses).HasMaxLength(1000).IsRequired();
            entity.Property(notification => notification.RecommendedActions).HasMaxLength(1000).IsRequired();
            entity.Property(notification => notification.Status).HasMaxLength(40).IsRequired();
            entity.Property(notification => notification.SourceTag).HasMaxLength(160);
            entity.HasIndex(notification => new { notification.Code, notification.SourceTag, notification.Status });
            entity.HasIndex(notification => notification.CreatedAt);
        });

        modelBuilder.Entity<MaintenanceHistory>(entity =>
        {
            entity.HasKey(history => history.Id);
            entity.Property(history => history.FailureDescription).HasMaxLength(1000).IsRequired();
            entity.Property(history => history.RealCause).HasMaxLength(300).IsRequired();
            entity.Property(history => history.Component).HasMaxLength(180);
            entity.Property(history => history.ActionTaken).HasMaxLength(500);
            entity.Property(history => history.RegisteredBy).HasMaxLength(80).IsRequired();
            entity.HasIndex(history => history.CreatedAt);
        });

        modelBuilder.Entity<MachineReading>(entity =>
        {
            entity.HasKey(reading => reading.Id);
            entity.Property(reading => reading.MachineCode).HasMaxLength(80).IsRequired();
            entity.Property(reading => reading.Tag).HasMaxLength(120).IsRequired();
            entity.Property(reading => reading.Unit).HasMaxLength(30);
            entity.Property(reading => reading.Status).HasMaxLength(40).IsRequired();
            entity.Property(reading => reading.Source).HasMaxLength(40).IsRequired();
            entity.Property(reading => reading.MqttTopic).HasMaxLength(250);
            entity.HasIndex(reading => new { reading.MachineCode, reading.Tag, reading.CollectedAt });
        });
    }
}
