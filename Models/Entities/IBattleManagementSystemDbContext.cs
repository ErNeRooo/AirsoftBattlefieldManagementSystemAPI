using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public interface IBattleManagementSystemDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Battle> Battles { get; set; }
        DbSet<Kill> Kills { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<PlayerLocation> PlayerLocations { get; set; }
        DbSet<Team> Teams { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
