using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.DeathHelper;

public interface IDeathHelper
{
    public Death FindById(int? id);
    public List<Death> FindAllOfPlayer(Player player);
}