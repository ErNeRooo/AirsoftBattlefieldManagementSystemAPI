using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext
{
    public interface IBattleManagementSystemDbContext
    {
        DbSet<Account> Account { get; set; }
        DbSet<Battle> Battle { get; set; }
        DbSet<Kill> Kill { get; set; }
        DbSet<Order> Order { get; set; }
        DbSet<Death> Death { get; set; }
        DbSet<Location> Location { get; set; }
        DbSet<Player> Player { get; set; }
        DbSet<PlayerLocation> PlayerLocation { get; set; }
        DbSet<Team> Team { get; set; }
        DbSet<Room> Room { get; set; }

        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        DbContextId ContextId { get; }
        IModel Model { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
    }
}
