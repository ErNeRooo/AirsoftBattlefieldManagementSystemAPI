using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext
{
    public class BattleManagementSystemDbContext(DbContextOptions<BattleManagementSystemDbContext> options) : DbContext(options), IBattleManagementSystemDbContext
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
                .HasOne(account => account.Player)
                .WithOne(player => player.Account)
                .HasForeignKey<Account>(a => a.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Team>()
                .Property(team => team.Name)
                .HasMaxLength(60);

            modelBuilder
                .Entity<Team>()
                .HasOne(team => team.OfficerPlayer)
                .WithMany()
                .HasForeignKey(team => team.OfficerPlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Team>()
                .HasOne(team => team.Room)
                .WithMany()
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Player>()
                .Property(player => player.Name)
                .HasMaxLength(40);
            
            modelBuilder
                .Entity<Player>()
                .HasOne(player => player.Room)
                .WithMany()
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder
                .Entity<Player>()
                .HasOne(player => player.Team)
                .WithMany(team => team.Players)
                .HasForeignKey(player => player.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Battle>()
                .Property(battle => battle.Name)
                .HasMaxLength(60);

            modelBuilder
                .Entity<Battle>()
                .HasOne(battle => battle.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder
                .Entity<Room>()
                .HasOne(room => room.AdminPlayer)
                .WithMany()
                .HasForeignKey(r => r.AdminPlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Room)
                .WithMany()
                .HasForeignKey(kill => kill.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Location)
                .WithMany()
                .HasForeignKey(kill => kill.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Player)
                .WithMany()
                .HasForeignKey(kill => kill.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Death>()
                .HasOne(death => death.Room)
                .WithMany()
                .HasForeignKey(death => death.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Death>()
                .HasOne(death => death.Location)
                .WithMany()
                .HasForeignKey(death => death.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Death>()
                .HasOne(death => death.Player)
                .WithMany()
                .HasForeignKey(death => death.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Room)
                .WithMany()
                .HasForeignKey(playerLocation => playerLocation.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Location)
                .WithMany()
                .HasForeignKey(playerLocation => playerLocation.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Player)
                .WithMany()
                .HasForeignKey(playerLocation => playerLocation.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
