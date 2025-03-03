using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class BattleManagementSystemDbContext : DbContext, IBattleManagementSystemDbContext
    {
        private readonly IConfiguration _configuration;

        public BattleManagementSystemDbContext(DbContextOptions<BattleManagementSystemDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

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
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Database connection string is missing.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
