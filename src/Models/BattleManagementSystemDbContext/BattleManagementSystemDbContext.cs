using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext
{
    public class BattleManagementSystemDbContext(DbContextOptions<BattleManagementSystemDbContext> options) : DbContext, IBattleManagementSystemDbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<Battle> Battle { get; set; }
        public DbSet<Kill> Kill { get; set; }
        public DbSet<Death> Death { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<PlayerLocation> PlayerLocation { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Room> Room { get; set; }

        public DatabaseFacade Database => base.Database;
        public ChangeTracker ChangeTracker => base.ChangeTracker;
        public DbContextId ContextId => base.ContextId;
        public IModel Model => base.Model;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Account>()
                .Property(a => a.Email)
                .HasMaxLength(320);

            modelBuilder
                .Entity<Account>()
                .HasOne(a => a.Player)
                .WithOne(p => p.Account)
                .HasForeignKey<Account>(a => a.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Team>()
                .Property(a => a.Name)
                .HasMaxLength(60);

            modelBuilder
                .Entity<Team>()
                .HasOne(t => t.OfficerPlayer)
                .WithMany()
                .HasForeignKey(t => t.OfficerPlayerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Player>()
                .Property(a => a.Name)
                .HasMaxLength(40);

            modelBuilder
                .Entity<Battle>()
                .Property(a => a.Name)
                .HasMaxLength(60);

            modelBuilder
                .Entity<Room>()
                .HasOne(r => r.AdminPlayer)
                .WithMany()
                .HasForeignKey(r => r.AdminPlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Kill>()
                .HasOne(k => k.Room)
                .WithMany()
                .HasForeignKey(kill => kill.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Death>()
                .HasOne(death => death.Room)
                .WithMany()
                .HasForeignKey(death => death.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Room)
                .WithMany()
                .HasForeignKey(playerLocation => playerLocation.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
