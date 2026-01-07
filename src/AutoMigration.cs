using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;

namespace AirsoftBattlefieldManagementSystemAPI
{
    public static class AutoMigration
    {
        public static void ApplyMigrations(IServiceProvider services, ILogger? logger = null)
        {
            try
            {
                using var scope = services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IBattleManagementSystemDbContext>();

                var pendingMigrations = dbContext.Database.GetPendingMigrations();

                if(pendingMigrations == null || !pendingMigrations.Any())
                {
                    logger?.LogInformation("Migrations are up to date :D");
                    return;
                }

                logger?.LogInformation("Starting database migration...");
                dbContext.Database.Migrate();
                logger?.LogInformation("Database migration finished successfully :D");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
    }
}
