using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.AccountHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.BattleHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.DeathHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.KillHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.LocationHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.MapPingHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.OrderHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerLocationHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.TeamHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

public class DbContextHelperService(
    IAccountHelper accountHelper,
    IBattleHelper battleHelper,
    IDeathHelper deathHelper,
    IKillHelper killHelper,
    IOrderHelper orderHelper,
    IMapPingHelper mapPingHelper,
    ILocationHelper locationHelper,
    IPlayerHelper playerHelper,
    IPlayerLocationHelper playerLocationHelper,
    IRoomHelper roomHelper,
    ITeamHelper teamHelper
    ) : IDbContextHelperService
{
    public IAccountHelper Account { get; } = accountHelper;
    public IBattleHelper Battle { get; } = battleHelper;
    public IDeathHelper Death { get; } = deathHelper;
    public IKillHelper Kill { get; } = killHelper;
    public IOrderHelper Order { get; } = orderHelper;
    public IMapPingHelper MapPing { get; } = mapPingHelper;
    public ILocationHelper Location { get; } = locationHelper;
    public IPlayerHelper Player { get; } = playerHelper;
    public IPlayerLocationHelper PlayerLocation { get; } = playerLocationHelper;
    public IRoomHelper Room { get; } = roomHelper;
    public ITeamHelper Team { get; } = teamHelper;
}