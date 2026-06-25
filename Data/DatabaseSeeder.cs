using Microsoft.EntityFrameworkCore;

namespace SupervisorioSIMMAQ_NXA.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SupervisorioDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
        await EnsureDiagnosticTablesAsync(dbContext);

        var defaultTags = TagCatalog.CreateDefaultTags();
        var existingTagNames = await dbContext.IndustrialTags
            .Select(tag => tag.Name)
            .ToListAsync();

        var newTags = defaultTags
            .Where(tag => !existingTagNames.Contains(tag.Name))
            .ToList();

        if (newTags.Count == 0)
        {
            return;
        }

        dbContext.IndustrialTags.AddRange(newTags);

        dbContext.TagValueHistory.AddRange(newTags.Select(tag => new Models.TagValueHistory
        {
            TagName = tag.Name,
            Value = tag.CurrentValue,
            Source = "Seed",
            CreatedAt = DateTime.UtcNow
        }));

        await dbContext.SaveChangesAsync();
    }

    private static async Task EnsureDiagnosticTablesAsync(SupervisorioDbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "DiagnosticNotifications" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_DiagnosticNotifications" PRIMARY KEY AUTOINCREMENT,
                "Code" TEXT NOT NULL,
                "Severity" TEXT NOT NULL,
                "Origin" TEXT NOT NULL,
                "Title" TEXT NOT NULL,
                "Description" TEXT NOT NULL,
                "PossibleCauses" TEXT NOT NULL,
                "RecommendedActions" TEXT NOT NULL,
                "Status" TEXT NOT NULL,
                "SourceTag" TEXT NULL,
                "Confidence" REAL NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                "ResolvedAt" TEXT NULL
            );
            """);

        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "MaintenanceHistory" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_MaintenanceHistory" PRIMARY KEY AUTOINCREMENT,
                "DiagnosticNotificationId" INTEGER NULL,
                "FailureDescription" TEXT NOT NULL,
                "RealCause" TEXT NOT NULL,
                "Component" TEXT NOT NULL,
                "ActionTaken" TEXT NOT NULL,
                "RegisteredBy" TEXT NOT NULL,
                "CreatedAt" TEXT NOT NULL
            );
            """);

        await dbContext.Database.ExecuteSqlRawAsync(
            "CREATE INDEX IF NOT EXISTS \"IX_DiagnosticNotifications_Code_SourceTag_Status\" ON \"DiagnosticNotifications\" (\"Code\", \"SourceTag\", \"Status\");");
        await dbContext.Database.ExecuteSqlRawAsync(
            "CREATE INDEX IF NOT EXISTS \"IX_DiagnosticNotifications_CreatedAt\" ON \"DiagnosticNotifications\" (\"CreatedAt\");");
        await dbContext.Database.ExecuteSqlRawAsync(
            "CREATE INDEX IF NOT EXISTS \"IX_MaintenanceHistory_CreatedAt\" ON \"MaintenanceHistory\" (\"CreatedAt\");");
    }
}
