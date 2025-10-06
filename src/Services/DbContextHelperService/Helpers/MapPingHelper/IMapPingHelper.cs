using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.MapPingHelper;

public interface IMapPingHelper
{
    public MapPing FindById(int? id);
    public MapPing FindByIdIncludingBattle(int? id);
    public List<MapPing> FindAllOfPlayer(Player player);
    public List<MapPing> FindAllOfTeam(Team team);
    public List<MapPing> FindAllOfTeam(int teamId);
}