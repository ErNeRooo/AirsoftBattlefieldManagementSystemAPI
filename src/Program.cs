using System.Text;
using AirsoftBattlefieldManagementSystemAPI.Authorization.JwtPlayerIdHasExistingPlayerEntity;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsRoomAdminOrTargetTeamOfficer;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerOwnsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.TargetPlayerIsInTheSameTeam;
using AirsoftBattlefieldManagementSystemAPI.Middleware;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles;
using AirsoftBattlefieldManagementSystemAPI.Services.AccountService;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.BattleService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.AccountHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.BattleHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.DeathHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.KillHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.LocationHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerLocationHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.TeamHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DeathService;
using AirsoftBattlefieldManagementSystemAPI.Services.JoinCodeService;
using AirsoftBattlefieldManagementSystemAPI.Services.KillService;
using AirsoftBattlefieldManagementSystemAPI.Services.LocationService;
using AirsoftBattlefieldManagementSystemAPI.Services.PlayerService;
using AirsoftBattlefieldManagementSystemAPI.Services.RoomService;
using AirsoftBattlefieldManagementSystemAPI.Services.TeamService;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            var authenticationSettings = new AuthenticationSettings();

            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            builder.Services.AddSingleton<IAuthenticationSettings>(authenticationSettings);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("PlayerIdFromClaimExistsInDatabase", policy => 
                    policy.Requirements.Add(new JwtPlayerIdHasExistingPlayerEntityRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, PlayerOwnsResourceHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, PlayerIsInTheSameRoomAsResourceHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, JwtPlayerIdHasExistingPlayerEntityHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, PlayerIsRoomAdminOrTargetTeamOfficerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, TargetPlayerIsInTheSameTeamHandler>();
            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new JwtPlayerIdHasExistingPlayerEntityRequirement())
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
            builder.Services.AddOpenApi();
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
            builder.Services.AddScoped<IJoinCodeService, JoinCodeService>();
            builder.Services.AddScoped<IAuthorizationHelperService, AuthorizationHelperService>();
            builder.Services.AddScoped<IClaimsHelperService, ClaimsHelperService>();
            builder.Services.AddScoped<IDbContextHelperService, DbContextHelperService>();
            builder.Services.AddScoped<IAccountHelper, AccountHelper>();
            builder.Services.AddScoped<IBattleHelper, BattleHelper>();
            builder.Services.AddScoped<IDeathHelper, DeathHelper>();
            builder.Services.AddScoped<IKillHelper, KillHelper>();
            builder.Services.AddScoped<ILocationHelper, LocationHelper>();
            builder.Services.AddScoped<IPlayerHelper, PlayerHelper>();
            builder.Services.AddScoped<IPlayerLocationHelper, PlayerLocationHelper>();
            builder.Services.AddScoped<IRoomHelper, RoomHelper>();
            builder.Services.AddScoped<ITeamHelper, TeamHelper>();


            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            builder.Services.AddScoped<IPasswordHasher<Room>, PasswordHasher<Room>>();
            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.WebHost.UseUrls("http://0.0.0.0:8080");
            
            builder.Services.AddDbContext<IBattleManagementSystemDbContext, BattleManagementSystemDbContext>(
                options =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new Exception("Database connection string is missing");
                    }
                    
                    options.UseSqlServer(connectionString);
                }
                    
            );
            
            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTheme(ScalarTheme.DeepSpace)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
