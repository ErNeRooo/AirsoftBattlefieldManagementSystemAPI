using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
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
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.ZoneHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

public interface IDbContextHelperService
{
    public IAccountHelper Account { get; }
    public IBattleHelper Battle { get; }
    public IDeathHelper Death { get; }
    public IKillHelper Kill { get; }
    public IOrderHelper Order { get; }
    public IMapPingHelper MapPing { get; }
    public IZoneHelper Zone { get; }
    public ILocationHelper Location { get; }
    public IPlayerHelper Player { get; }
    public IPlayerLocationHelper PlayerLocation { get; }
    public IRoomHelper Room { get; }
    public ITeamHelper Team { get; }
}