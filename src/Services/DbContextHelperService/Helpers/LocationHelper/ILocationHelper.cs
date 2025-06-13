using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.LocationHelper;

public interface ILocationHelper
{
    public Location FindById(int? id);
    public List<Location> FindAllOfPlayer(Player player);
}