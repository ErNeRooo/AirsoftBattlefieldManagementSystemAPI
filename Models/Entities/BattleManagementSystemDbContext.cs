using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class BattleManagementSystemDbContext : DbContext, IBattleManagementSystemDbContext
    {
        private string _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Kill> Kills { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerLocation> PlayerLocations { get; set; }
        public DbSet<Team> Teams { get; set; }

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
                .HasMaxLength(20);

            modelBuilder
                .Entity<Player>()
                .Property(a => a.Name)
                .HasMaxLength(20);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=database,1433;Database=AirsoftBattleManagementSystem;User Id=SA;Password=K0ciaki!;Encrypt=False;");
        }
    }
}
