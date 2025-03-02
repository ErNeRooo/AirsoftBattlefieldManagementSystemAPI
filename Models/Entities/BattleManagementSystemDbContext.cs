using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class BattleManagementSystemDbContext : DbContext, IBattleManagementSystemDbContext
    {
        private string _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        public DbSet<Account> Account { get; set; }
        public DbSet<Battle> Battle { get; set; }
        public DbSet<Kill> Kill { get; set; }
        public DbSet<Death> Death { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<PlayerLocation> PlayerLocation { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Room> Room { get; set; }

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
