using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public interface IBattleManagementSystemDbContext
    {
        DbSet<Account> Account { get; set; }
        DbSet<Battle> Battle { get; set; }
        DbSet<Kill> Kill { get; set; }
        DbSet<Death> Death { get; set; }
        DbSet<Location> Location { get; set; }
        DbSet<Player> Player { get; set; }
        DbSet<PlayerLocation> PlayerLocation { get; set; }
        DbSet<Team> Team { get; set; }
        DbSet<Room> Room { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
