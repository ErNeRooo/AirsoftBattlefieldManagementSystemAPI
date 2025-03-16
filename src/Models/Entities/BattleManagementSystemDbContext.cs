using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class BattleManagementSystemDbContext(DbContextOptions<BattleManagementSystemDbContext> options, IConfiguration configuration) : DbContext(options), IBattleManagementSystemDbContext
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

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Account>()
                .Property(a => a.Email)
                .HasMaxLength(320);

            modelBuilder
                .Entity<Account>()
                .Property(a => a.Password)
                .HasMaxLength(64);

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Database connection string is missing.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
