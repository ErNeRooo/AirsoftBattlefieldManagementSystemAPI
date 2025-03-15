using AirsoftBattlefieldManagementSystemAPI.Middleware;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Scalar.AspNetCore;

namespace AirsoftBattlefieldManagementSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<IBattleManagementSystemDbContext, BattleManagementSystemDbContext>(
                options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    );
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
            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTheme(ScalarTheme.DeepSpace)
                        .WithDefaultHttpClient(ScalarTarget.Kotlin, ScalarClient.OkHttp);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
