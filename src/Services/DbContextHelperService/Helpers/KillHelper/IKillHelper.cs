using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.KillHelper;

public interface IKillHelper
{
    public Kill FindById(int? id);
    public Kill FindByIdIncludingBattle(int? id);
    public List<Kill> FindAllOfPlayer(Player player);
}