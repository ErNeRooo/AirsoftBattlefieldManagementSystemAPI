using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<IBattleManagementSystemDbContext, BattleManagementSystemDbContext>();
            builder.Services.AddSingleton<IMapper>(sp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PlayerMappingProfile>();
                    cfg.AddProfile<RoomMappingProfile>();
                    cfg.AddProfile<TeamMappingProfile>();
                    cfg.AddProfile<BattleMappingProfile>();
                    cfg.AddProfile<AccountMappingProfile>();
                    cfg.AddProfile<LocationMappingProfile>();
                    cfg.AddProfile<KillMappingProfile>();
                    cfg.AddProfile<DeathMappingProfile>();
                });
                return config.CreateMapper();
            });
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IBattleService, BattleService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IKillService, KillService>();
            builder.Services.AddScoped<IDeathService, DeathService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
