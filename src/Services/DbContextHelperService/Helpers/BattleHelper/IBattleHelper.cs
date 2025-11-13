using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.BattleHelper;

public interface IBattleHelper
{
    public Battle FindById(int? id);
    public Battle FindByIdIncludingRoom(int? id);
    public Battle FindByIdIncludingRelated(int? id);
}