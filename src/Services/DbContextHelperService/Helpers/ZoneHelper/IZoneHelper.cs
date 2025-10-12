using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.ZoneHelper;

public interface IZoneHelper
{
    public Zone FindById(int? id);
    public List<Zone> FindAllOfBattle(Battle battle);
    public List<Zone> FindAllOfBattle(int battleId);
}