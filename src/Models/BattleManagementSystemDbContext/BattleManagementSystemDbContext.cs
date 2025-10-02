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
        public DbSet<Order> Order { get; set; }
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
        
        private ModelBuilder _modelBuilder;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            _modelBuilder = modelBuilder;
            
            BuildPlayerLocation();
            BuildKill();
            BuildOrder();
            BuildDeath();
            BuildAccount();
            BuildPlayer();
            BuildBattle();
            BuildTeam();
            BuildRoom();
        }
        
        private void BuildPlayerLocation()
        {
            _modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Battle)
                .WithMany(battle => battle.PlayerLocations)
                .HasForeignKey(playerLocation => playerLocation.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Location)
                .WithMany()
                .HasForeignKey(playerLocation => playerLocation.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<PlayerLocation>()
                .HasOne(playerLocation => playerLocation.Player)
                .WithMany(player => player.PlayerLocations)
                .HasForeignKey(playerLocation => playerLocation.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void BuildKill()
        {
            _modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Battle)
                .WithMany(battle => battle.Kills)
                .HasForeignKey(kill => kill.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Location)
                .WithMany()
                .HasForeignKey(kill => kill.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Kill>()
                .HasOne(kill => kill.Player)
                .WithMany(player => player.Kills)
                .HasForeignKey(kill => kill.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        private void BuildOrder()
        {
            _modelBuilder.Entity<Order>()
                .HasOne(order => order.Battle)
                .WithMany(battle => battle.Orders)
                .HasForeignKey(order => order.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Order>()
                .HasOne(order => order.Location)
                .WithMany()
                .HasForeignKey(order => order.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Order>()
                .HasOne(order => order.Player)
                .WithMany(player => player.Orders)
                .HasForeignKey(order => order.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        private void BuildDeath()
        {
            _modelBuilder.Entity<Death>()
                .HasOne(death => death.Battle)
                .WithMany(battle => battle.Deaths)
                .HasForeignKey(death => death.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Death>()
                .HasOne(death => death.Location)
                .WithMany()
                .HasForeignKey(death => death.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            _modelBuilder.Entity<Death>()
                .HasOne(death => death.Player)
                .WithMany(player => player.Deaths)
                .HasForeignKey(death => death.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void BuildAccount()
        {
            _modelBuilder
                .Entity<Account>()
                .Property(a => a.Email)
                .HasMaxLength(320);

            _modelBuilder
                .Entity<Account>()
                .HasOne(account => account.Player)
                .WithOne(player => player.Account)
                .HasForeignKey<Account>(a => a.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void BuildPlayer()
        {
            _modelBuilder
                .Entity<Player>()
                .Property(player => player.Name)
                .HasMaxLength(40);
            
            _modelBuilder
                .Entity<Player>()
                .HasOne(player => player.Room)
                .WithMany(room => room.Players)
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
            
            _modelBuilder
                .Entity<Player>()
                .HasOne(player => player.Team)
                .WithMany(team => team.Players)
                .HasForeignKey(player => player.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        
        private void BuildBattle()
        {
            _modelBuilder
                .Entity<Battle>()
                .Property(battle => battle.Name)
                .HasMaxLength(60);
            
            _modelBuilder
                .Entity<Battle>()
                .HasOne(battle => battle.Room)
                .WithOne(room => room.Battle)
                .HasForeignKey<Battle>(battle => battle.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        
        private void BuildTeam()
        {
            _modelBuilder
                .Entity<Team>()
                .Property(team => team.Name)
                .HasMaxLength(60);

            _modelBuilder
                .Entity<Team>()
                .HasOne(team => team.OfficerPlayer)
                .WithMany()
                .HasForeignKey(team => team.OfficerPlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            _modelBuilder
                .Entity<Team>()
                .HasOne(team => team.Room)
                .WithMany(room => room.Teams)
                .HasForeignKey(team => team.RoomId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        
        private void BuildRoom()
        {
            _modelBuilder
                .Entity<Room>()
                .HasOne(room => room.AdminPlayer)
                .WithOne()
                .HasForeignKey<Room>(room => room.AdminPlayerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
